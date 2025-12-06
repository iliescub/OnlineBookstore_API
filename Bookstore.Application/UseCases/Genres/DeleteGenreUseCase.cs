using BookstoreAPI.Domain.Repositories;

namespace BookstoreAPI.Application.UseCases.Genres;

public class DeleteGenreUseCase
{
    private readonly IGenreRepository _genreRepository;

    public DeleteGenreUseCase(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<bool> ExecuteAsync(string id)
    {
        var genre = await _genreRepository.GetByIdAsync(id);
        if (genre == null)
            return false;

        await _genreRepository.DeleteAsync(id);
        return true;
    }
}
