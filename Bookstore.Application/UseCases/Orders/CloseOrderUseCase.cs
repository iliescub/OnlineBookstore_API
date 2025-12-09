using BookstoreAPI.Domain.Repositories;

namespace BookstoreAPI.Application.UseCases.Orders;

public class CloseOrderUseCase : UpdateOrderStatusUseCaseBase
{
    public CloseOrderUseCase(IOrderRepository orderRepository) : base(orderRepository) { }

    protected override string RequiredCurrentStatus => "completed";
    protected override string NewStatus => "closed";
    protected override string OperationName => "close";
}
