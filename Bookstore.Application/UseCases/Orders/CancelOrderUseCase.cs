using BookstoreAPI.Domain.Repositories;

namespace BookstoreAPI.Application.UseCases.Orders;

public class CancelOrderUseCase
{
    private readonly IOrderRepository _orderRepository;

    public CancelOrderUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<bool> ExecuteAsync(string orderId, string userId, bool isAdmin)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);

        if (order == null)
        {
            return false;
        }

        // Only the order owner or admin can cancel
        if (order.UserId != userId && !isAdmin)
        {
            throw new UnauthorizedAccessException("You don't have permission to cancel this order");
        }

        // Only pending orders can be cancelled
        if (order.Status.ToLower() != "pending")
        {
            throw new InvalidOperationException($"Cannot cancel order with status '{order.Status}'");
        }

        order.UpdateStatus("cancelled");
        await _orderRepository.UpdateAsync(order);

        return true;
    }
}
