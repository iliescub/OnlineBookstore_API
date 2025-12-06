namespace BookstoreAPI.Application.DTOs.Requests;

public record UpdateGenreRequest(
    string Name,
    string Description);
