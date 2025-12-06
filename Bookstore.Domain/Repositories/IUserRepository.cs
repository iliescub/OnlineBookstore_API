using BookstoreAPI.Domain.Entities;

namespace BookstoreAPI.Domain.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(string id);
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(string id);
    Task<bool> ExistsByEmailAsync(string email);
}
