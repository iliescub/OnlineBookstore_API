using BookstoreAPI.Application.DTOs.Responses;
using BookstoreAPI.Domain.Repositories;

namespace BookstoreAPI.Application.UseCases.Genres;

public class GetGenreByIdUseCase
{
    private readonly IGenreRepository _genreRepository;

    public GetGenreByIdUseCase(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<GenreResponse?> ExecuteAsync(string id)
    {
        var genre = await _genreRepository.GetByIdAsync(id);
        if (genre == null)
            return null;

        return new GenreResponse(
            genre.Id,
            genre.Name,
            genre.Description,
            genre.CreatedAt
        );
    }
}
