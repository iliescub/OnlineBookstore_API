using BookstoreAPI.Application.DTOs.Requests;
using BookstoreAPI.Application.DTOs.Responses;
using BookstoreAPI.Domain.Repositories;

namespace BookstoreAPI.Application.UseCases.Books;

public class UpdateBookUseCase
{
    private readonly IBookRepository _bookRepository;

    public UpdateBookUseCase(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<BookResponse?> ExecuteAsync(string id, UpdateBookRequest request)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null)
            return null;

        book.UpdateDetails(
            request.Title,
            request.Author,
            request.Price,
            request.Category,
            request.Image
        );

        book.UpdateStock(request.Stock);

        await _bookRepository.UpdateAsync(book);

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
