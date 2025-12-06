using BookstoreAPI.Application.DTOs.Requests;
using BookstoreAPI.Application.DTOs.Responses;
using BookstoreAPI.Domain.Entities;
using BookstoreAPI.Domain.Repositories;

namespace BookstoreAPI.Application.UseCases.Genres;

public class CreateGenreUseCase
{
    private readonly IGenreRepository _genreRepository;

    public CreateGenreUseCase(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<GenreResponse?> ExecuteAsync(CreateGenreRequest request)
    {
        if (await _genreRepository.ExistsByNameAsync(request.Name))
            return null;

        var genre = new Genre(request.Name, request.Description);
        await _genreRepository.AddAsync(genre);

        return new GenreResponse(
            genre.Id,
            genre.Name,
            genre.Description,
            genre.CreatedAt
        );
    }
}
