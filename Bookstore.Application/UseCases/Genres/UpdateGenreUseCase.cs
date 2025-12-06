using BookstoreAPI.Application.DTOs.Requests;
using BookstoreAPI.Application.DTOs.Responses;
using BookstoreAPI.Domain.Repositories;

namespace BookstoreAPI.Application.UseCases.Genres;

public class UpdateGenreUseCase
{
    private readonly IGenreRepository _genreRepository;

    public UpdateGenreUseCase(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<GenreResponse?> ExecuteAsync(string id, UpdateGenreRequest request)
    {
        var genre = await _genreRepository.GetByIdAsync(id);
        if (genre == null)
            return null;

        genre.Update(request.Name, request.Description);
        await _genreRepository.UpdateAsync(genre);

        return new GenreResponse(
            genre.Id,
            genre.Name,
            genre.Description,
            genre.CreatedAt
        );
    }
}
