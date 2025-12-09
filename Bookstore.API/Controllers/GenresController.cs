using BookstoreAPI.Application.DTOs.Requests;
using BookstoreAPI.Application.UseCases.Genres;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BookstoreAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("api")]
public class GenresController : ControllerBase
{
    private readonly GetAllGenresUseCase _getAllGenresUseCase;
    private readonly GetGenreByIdUseCase _getGenreByIdUseCase;
    private readonly CreateGenreUseCase _createGenreUseCase;
    private readonly UpdateGenreUseCase _updateGenreUseCase;
    private readonly DeleteGenreUseCase _deleteGenreUseCase;

    public GenresController(
        GetAllGenresUseCase getAllGenresUseCase,
        GetGenreByIdUseCase getGenreByIdUseCase,
        CreateGenreUseCase createGenreUseCase,
        UpdateGenreUseCase updateGenreUseCase,
        DeleteGenreUseCase deleteGenreUseCase)
    {
        _getAllGenresUseCase = getAllGenresUseCase;
        _getGenreByIdUseCase = getGenreByIdUseCase;
        _createGenreUseCase = createGenreUseCase;
        _updateGenreUseCase = updateGenreUseCase;
        _deleteGenreUseCase = deleteGenreUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetGenres()
    {
        var genres = await _getAllGenresUseCase.ExecuteAsync();
        return Ok(genres);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGenre(string id)
    {
        var genre = await _getGenreByIdUseCase.ExecuteAsync(id);
        if (genre == null)
        {
            return NotFound(new { error = "Genre not found" });
        }
        return Ok(genre);
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<IActionResult> CreateGenre([FromBody] CreateGenreRequest request)
    {
        var genre = await _createGenreUseCase.ExecuteAsync(request);
        if (genre == null)
        {
            return BadRequest(new { error = "Genre already exists" });
        }
        return CreatedAtAction(nameof(GetGenre), new { id = genre.Id }, genre);
    }

    [Authorize(Roles = "admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGenre(string id, [FromBody] UpdateGenreRequest request)
    {
        var genre = await _updateGenreUseCase.ExecuteAsync(id, request);
        if (genre == null)
        {
            return NotFound(new { error = "Genre not found" });
        }
        return Ok(genre);
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGenre(string id)
    {
        var success = await _deleteGenreUseCase.ExecuteAsync(id);
        if (!success)
        {
            return NotFound(new { error = "Genre not found" });
        }
        return Ok(new { message = "Genre deleted" });
    }
}
