using BookstoreAPI.Application.DTOs.Requests;
using BookstoreAPI.Application.DTOs.Responses;
using BookstoreAPI.Domain.Entities;
using BookstoreAPI.Domain.Repositories;

namespace BookstoreAPI.Application.UseCases.Orders;

public class CreateOrderUseCase
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderResponse> ExecuteAsync(CreateOrderRequest request)
    {
        var orderItems = request.Items.Select(item => new OrderItem(
            item.Id,
            item.Title,
            item.Author,
            item.Price,
            item.Quantity
        )).ToList();

        PaymentInfo? paymentInfo = null;
        if (request.PaymentInfo != null)
        {
            paymentInfo = new PaymentInfo(
                request.PaymentInfo.CardNumber,
                request.PaymentInfo.CardName,
                request.PaymentInfo.Expiry,
                request.PaymentInfo.Cvv,
                request.PaymentInfo.Address
            );
        }

        var order = new Order(
            request.UserId,
            request.UserName,
            orderItems,
            paymentInfo
        );

        await _orderRepository.AddAsync(order);

        var responseItems = order.Items.Select(item => new OrderItemResponse(
            item.Id,
            item.Title,
            item.Author,
            item.Price,
            item.Quantity
        )).ToList();

        PaymentInfoResponse? paymentInfoResponse = null;
        if (order.PaymentInfo != null)
        {
            paymentInfoResponse = new PaymentInfoResponse(
                order.PaymentInfo.CardNumber,
                order.PaymentInfo.CardName,
                order.PaymentInfo.Expiry,
                order.PaymentInfo.Cvv,
                order.PaymentInfo.Address
            );
        }

        return new OrderResponse(
            order.Id,
            order.UserId,
            order.UserName,
            responseItems,
            order.Total,
            order.Date,
            order.Status,
            paymentInfoResponse
        );
    }
}
