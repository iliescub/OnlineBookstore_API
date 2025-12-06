using BookstoreAPI.Domain.Entities;

namespace BookstoreAPI.Domain.Repositories;

public interface IGenreRepository
{
    Task<IEnumerable<Genre>> GetAllAsync();
    Task<Genre?> GetByIdAsync(string id);
    Task<Genre?> GetByNameAsync(string name);
    Task AddAsync(Genre genre);
    Task UpdateAsync(Genre genre);
    Task DeleteAsync(string id);
    Task<bool> ExistsByNameAsync(string name);
}
