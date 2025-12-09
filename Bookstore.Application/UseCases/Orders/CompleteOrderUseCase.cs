using BookstoreAPI.Domain.Repositories;

namespace BookstoreAPI.Application.UseCases.Orders;

public class CompleteOrderUseCase
{
    private readonly IOrderRepository _orderRepository;

    public CompleteOrderUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<bool> ExecuteAsync(string orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);

        if (order == null)
        {
            return false;
        }

        // Only pending orders can be marked as completed
        if (order.Status.ToLower() != "pending")
        {
            throw new InvalidOperationException($"Cannot complete order with status '{order.Status}'. Only pending orders can be completed.");
        }

        order.UpdateStatus("completed");
        await _orderRepository.UpdateAsync(order);

        return true;
    }
}
