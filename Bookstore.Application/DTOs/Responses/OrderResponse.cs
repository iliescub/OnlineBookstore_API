namespace BookstoreAPI.Application.DTOs.Responses;

public record OrderResponse(
    string Id,
    string UserId,
    string UserName,
    List<OrderItemResponse> Items,
    decimal Total,
    DateTime Date,
    string Status,
    string? PaymentProviderId,
    string? ShippingAddress);

public record OrderItemResponse(
    string Id,
    string Title,
    string Author,
    decimal Price,
    int Quantity);
