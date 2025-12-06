namespace BookstoreAPI.Application.DTOs.Responses;

public record GenreResponse(
    string Id,
    string Name,
    string Description,
    DateTime CreatedAt);
