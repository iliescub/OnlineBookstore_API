namespace BookstoreAPI.Application.DTOs.Requests;

public record CreateGenreRequest(
    string Name,
    string Description);
