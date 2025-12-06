namespace BookstoreAPI.Application.DTOs.Requests;

public record UpdateBookRequest(
    string Title,
    string Author,
    decimal Price,
    string Category,
    int Stock,
    string Image);
