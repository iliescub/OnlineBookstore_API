using BookstoreAPI.Application.DTOs.Requests;
using BookstoreAPI.Application.DTOs.Responses;
using BookstoreAPI.Domain.Entities;
using BookstoreAPI.Domain.Repositories;

namespace BookstoreAPI.Application.UseCases.Orders;

public class CreateOrderUseCase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IBookRepository _bookRepository;

    public CreateOrderUseCase(IOrderRepository orderRepository, IBookRepository bookRepository)
    {
        _orderRepository = orderRepository;
        _bookRepository = bookRepository;
    }

    public async Task<OrderResponse?> ExecuteAsync(CreateOrderRequest request)
    {
        // Step 1: Validate stock availability for all items
        var booksToUpdate = new List<Book>();
        var stockErrors = new List<string>();

        foreach (var item in request.Items)
        {
            var book = await _bookRepository.GetByIdAsync(item.Id);

            if (book == null)
            {
                stockErrors.Add($"Book '{item.Title}' not found");
                continue;
            }

            if (!book.IsInStock(item.Quantity))
            {
                stockErrors.Add($"Insufficient stock for '{book.Title}'. Available: {book.Stock}, Requested: {item.Quantity}");
                continue;
            }

            booksToUpdate.Add(book);
        }

        // If any stock validation failed, throw exception
        if (stockErrors.Any())
        {
            throw new InvalidOperationException(string.Join("; ", stockErrors));
        }

        // Step 2: Reduce stock for all books (this happens in memory first)
        for (int i = 0; i < request.Items.Count; i++)
        {
            var item = request.Items[i];
            var book = booksToUpdate[i];

            if (!book.ReduceStock(item.Quantity))
            {
                // This shouldn't happen since we validated stock, but safety check
                throw new InvalidOperationException($"Failed to reduce stock for '{book.Title}'");
            }
        }

        // Step 3: Create the order
        var orderItems = request.Items.Select(item => new OrderItem(
            item.Id,
            item.Title,
            item.Author,
            item.Price,
            item.Quantity
        )).ToList();

        var order = new Order(
            request.UserId,
            request.UserName,
            orderItems,
            request.ShippingAddress
        );

        // Step 4: Save everything (order + updated book stocks)
        // Note: EF Core change tracking will handle this as a single transaction
        await _orderRepository.AddAsync(order);

        // Update all book stocks
        foreach (var book in booksToUpdate)
        {
            await _bookRepository.UpdateAsync(book);
        }

        // Step 5: Return the order response
        var responseItems = order.Items.Select(item => new OrderItemResponse(
            item.Id,
            item.Title,
            item.Author,
            item.Price,
            item.Quantity
        )).ToList();

        return new OrderResponse(
            order.Id,
            order.UserId,
            order.UserName,
            responseItems,
            order.Total,
            order.Date,
            order.Status,
            order.PaymentProviderId,
            order.ShippingAddress
        );
    }
}
