namespace BookstoreAPI.Application.DTOs.Requests;

public record CreateOrderRequest(
    string UserId,
    string UserName,
    List<OrderItemRequest> Items,
    string? ShippingAddress);

public record OrderItemRequest(
    string Id,
    string Title,
    string Author,
    decimal Price,
    int Quantity);
