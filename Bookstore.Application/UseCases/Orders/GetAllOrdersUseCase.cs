using BookstoreAPI.Application.DTOs.Responses;
using BookstoreAPI.Domain.Repositories;

namespace BookstoreAPI.Application.UseCases.Orders;

public class GetAllOrdersUseCase
{
    private readonly IOrderRepository _orderRepository;

    public GetAllOrdersUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<OrderResponse>> ExecuteAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
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
            order.PaymentInfo != null ? new PaymentInfoResponse(
                order.PaymentInfo.CardNumber,
                order.PaymentInfo.CardName,
                order.PaymentInfo.Expiry,
                order.PaymentInfo.Cvv,
                order.PaymentInfo.Address
            ) : null
        ));
    }
}
