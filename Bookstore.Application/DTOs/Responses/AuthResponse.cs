namespace BookstoreAPI.Application.DTOs.Responses;

public record AuthResponse(string Token, UserResponse User);
