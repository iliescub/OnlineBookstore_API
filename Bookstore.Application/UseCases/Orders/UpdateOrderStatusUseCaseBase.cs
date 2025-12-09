using BookstoreAPI.Domain.Repositories;

namespace BookstoreAPI.Application.UseCases.Orders;

public abstract class UpdateOrderStatusUseCaseBase
{
    protected readonly IOrderRepository OrderRepository;

    protected UpdateOrderStatusUseCaseBase(IOrderRepository orderRepository)
    {
        OrderRepository = orderRepository;
    }

    protected abstract string RequiredCurrentStatus { get; }
    protected abstract string NewStatus { get; }
    protected abstract string OperationName { get; }

    public async Task<bool> ExecuteAsync(string orderId)
    {
        var order = await OrderRepository.GetByIdAsync(orderId);

        if (order == null)
        {
            return false;
        }

        if (order.Status.ToLower() != RequiredCurrentStatus.ToLower())
        {
            throw new InvalidOperationException(
                $"Cannot {OperationName} order with status '{order.Status}'. " +
                $"Only {RequiredCurrentStatus} orders can be {OperationName}d.");
        }

        order.UpdateStatus(NewStatus);
        await OrderRepository.UpdateAsync(order);

        return true;
    }
}
