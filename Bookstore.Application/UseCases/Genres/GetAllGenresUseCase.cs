using BookstoreAPI.Application.DTOs.Responses;
using BookstoreAPI.Domain.Repositories;

namespace BookstoreAPI.Application.UseCases.Genres;

public class GetAllGenresUseCase
{
    private readonly IGenreRepository _genreRepository;

    public GetAllGenresUseCase(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<IEnumerable<GenreResponse>> ExecuteAsync()
    {
        var genres = await _genreRepository.GetAllAsync();
        return genres.Select(g => new GenreResponse(
            g.Id,
            g.Name,
            g.Description,
            g.CreatedAt
        ));
    }
}
