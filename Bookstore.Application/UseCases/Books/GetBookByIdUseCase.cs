using BookstoreAPI.Application.DTOs.Responses;
using BookstoreAPI.Domain.Repositories;

namespace BookstoreAPI.Application.UseCases.Books;

public class GetBookByIdUseCase
{
    private readonly IBookRepository _bookRepository;

    public GetBookByIdUseCase(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<BookResponse?> ExecuteAsync(string id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null)
            return null;

        return new BookResponse(
            book.Id,
            book.Title,
            book.Author,
            book.Price,
            book.Category,
            book.Stock,
            book.Image
        );
    }
}
