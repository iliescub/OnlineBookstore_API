using BookstoreAPI.Application.DTOs.Responses;
using BookstoreAPI.Domain.Repositories;

namespace BookstoreAPI.Application.UseCases.Orders;

public class GetOrdersByUserIdUseCase
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersByUserIdUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<OrderResponse>> ExecuteAsync(string userId)
    {
        var orders = await _orderRepository.GetByUserIdAsync(userId);
        return orders.Select(order => new OrderResponse(
            order.Id,
            order.UserId,
            order.UserName,
            order.Items.Select(item => new OrderItemResponse(
                item.Id,
                item.Title,
                item.Author,
                item.Price,
                item.Quantity
            )).ToList(),
            order.Total,
            order.Date,
            order.Status,
            order.PaymentProviderId,
            order.ShippingAddress
        ));
    }
}
