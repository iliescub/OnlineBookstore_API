using BookstoreAPI.Application.DTOs.Responses;
using BookstoreAPI.Domain.Repositories;

namespace BookstoreAPI.Application.UseCases.Books;

public class GetAllBooksUseCase
{
    private readonly IBookRepository _bookRepository;

    public GetAllBooksUseCase(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<IEnumerable<BookResponse>> ExecuteAsync(string? category = null)
    {
        var books = string.IsNullOrWhiteSpace(category)
            ? await _bookRepository.GetAllAsync()
            : await _bookRepository.GetByCategoryAsync(category);

        return books.Select(b => new BookResponse(
            b.Id,
            b.Title,
            b.Author,
            b.Price,
            b.Category,
            b.Stock,
            b.Image
        ));
    }
}
