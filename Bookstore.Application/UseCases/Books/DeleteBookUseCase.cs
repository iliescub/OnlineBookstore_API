using BookstoreAPI.Domain.Repositories;

namespace BookstoreAPI.Application.UseCases.Books;

public class DeleteBookUseCase
{
    private readonly IBookRepository _bookRepository;

    public DeleteBookUseCase(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<bool> ExecuteAsync(string id)
    {
        if (!await _bookRepository.ExistsAsync(id))
            return false;

        await _bookRepository.DeleteAsync(id);
        return true;
    }
}
