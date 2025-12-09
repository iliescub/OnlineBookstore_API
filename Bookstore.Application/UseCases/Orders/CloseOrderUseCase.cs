using BookstoreAPI.Domain.Repositories;

namespace BookstoreAPI.Application.UseCases.Orders;

public class CloseOrderUseCase
{
    private readonly IOrderRepository _orderRepository;

    public CloseOrderUseCase(IOrderRepository orderRepository)
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

        // Only completed orders can be closed (delivery confirmed)
        if (order.Status.ToLower() != "completed")
        {
            throw new InvalidOperationException($"Cannot close order with status '{order.Status}'. Only completed orders can be closed.");
        }

        order.UpdateStatus("closed");
        await _orderRepository.UpdateAsync(order);

        return true;
    }
}
