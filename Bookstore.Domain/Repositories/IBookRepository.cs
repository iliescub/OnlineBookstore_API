using BookstoreAPI.Domain.Entities;

namespace BookstoreAPI.Domain.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<IEnumerable<Book>> GetByCategoryAsync(string category);
    Task<Book?> GetByIdAsync(string id);
    Task AddAsync(Book book);
    Task UpdateAsync(Book book);
    Task DeleteAsync(string id);
    Task<bool> ExistsAsync(string id);
}
