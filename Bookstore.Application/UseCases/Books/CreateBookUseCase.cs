using BookstoreAPI.Application.DTOs.Requests;
using BookstoreAPI.Application.DTOs.Responses;
using BookstoreAPI.Domain.Entities;
using BookstoreAPI.Domain.Repositories;

namespace BookstoreAPI.Application.UseCases.Books;

public class CreateBookUseCase
{
    private readonly IBookRepository _bookRepository;

    public CreateBookUseCase(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<BookResponse> ExecuteAsync(CreateBookRequest request)
    {
        var book = new Book(
            request.Title,
            request.Author,
            request.Price,
            request.Category,
            request.Stock,
            request.Image
        );

        await _bookRepository.AddAsync(book);

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
