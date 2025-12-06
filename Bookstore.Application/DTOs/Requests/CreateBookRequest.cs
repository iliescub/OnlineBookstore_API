namespace BookstoreAPI.Application.DTOs.Requests;

public record CreateBookRequest(
    string Title,
    string Author,
    decimal Price,
    string Category,
    int Stock,
    string Image);
