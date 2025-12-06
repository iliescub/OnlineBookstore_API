using BookstoreAPI.Domain.Entities;

namespace BookstoreAPI.Domain.Repositories;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<IEnumerable<Order>> GetByUserIdAsync(string userId);
    Task<Order?> GetByIdAsync(string id);
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
}
