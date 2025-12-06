using BookstoreAPI.Domain.Entities;
using BookstoreAPI.Domain.Repositories;
using BookstoreAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookstoreAPI.Infrastructure.Persistence.Repositories;

public class BookRepository : IBookRepository
{
    private readonly BookstoreContext _context;

    public BookRepository(BookstoreContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _context.Books.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetByCategoryAsync(string category)
    {
        return await _context.Books
            .AsNoTracking()
            .Where(b => b.Category == category)
            .ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(string id)
    {
        return await _context.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task AddAsync(Book book)
    {
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(string id)
    {
        return await _context.Books.AnyAsync(b => b.Id == id);
    }
}
