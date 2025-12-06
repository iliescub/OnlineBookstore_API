using BookstoreAPI.Domain.Entities;
using BookstoreAPI.Domain.Repositories;
using BookstoreAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookstoreAPI.Infrastructure.Persistence.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly BookstoreContext _context;

    public OrderRepository(BookstoreContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _context.Orders
            .AsNoTracking()
            .OrderByDescending(o => o.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetByUserIdAsync(string userId)
    {
        return await _context.Orders
            .AsNoTracking()
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.Date)
            .ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(string id)
    {
        return await _context.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }
}
