using BookstoreAPI.Domain.Entities;
using BookstoreAPI.Domain.Repositories;
using BookstoreAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookstoreAPI.Infrastructure.Persistence.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly BookstoreContext _context;

    public GenreRepository(BookstoreContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Genre>> GetAllAsync()
    {
        return await _context.Genres.AsNoTracking().ToListAsync();
    }

    public async Task<Genre?> GetByIdAsync(string id)
    {
        return await _context.Genres.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<Genre?> GetByNameAsync(string name)
    {
        return await _context.Genres.AsNoTracking().FirstOrDefaultAsync(g => g.Name == name);
    }

    public async Task AddAsync(Genre genre)
    {
        await _context.Genres.AddAsync(genre);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Genre genre)
    {
        _context.Genres.Update(genre);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);
        if (genre != null)
        {
            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _context.Genres.AnyAsync(g => g.Name == name);
    }
}
