using BookstoreAPI.Domain.Repositories;

namespace BookstoreAPI.Application.UseCases.Orders;

public class CompleteOrderUseCase : UpdateOrderStatusUseCaseBase
{
    public CompleteOrderUseCase(IOrderRepository orderRepository) : base(orderRepository) { }

    protected override string RequiredCurrentStatus => "pending";
    protected override string NewStatus => "completed";
    protected override string OperationName => "complete";
}
