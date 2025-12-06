namespace BookstoreAPI.Application.DTOs.Responses;

public record BookResponse(
    string Id,
    string Title,
    string Author,
    decimal Price,
    string Category,
    int Stock,
    string Image);
