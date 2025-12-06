namespace BookstoreAPI.Application.DTOs.Requests;

public record CreateOrderRequest(
    string UserId,
    string UserName,
    List<OrderItemRequest> Items,
    PaymentInfoRequest? PaymentInfo);

public record OrderItemRequest(
    string Id,
    string Title,
    string Author,
    decimal Price,
    int Quantity);

public record PaymentInfoRequest(
    string CardNumber,
    string CardName,
    string Expiry,
    string Cvv,
    string Address);
