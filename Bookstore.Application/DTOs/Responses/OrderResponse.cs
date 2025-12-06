namespace BookstoreAPI.Application.DTOs.Responses;

public record OrderResponse(
    string Id,
    string UserId,
    string UserName,
    List<OrderItemResponse> Items,
    decimal Total,
    DateTime Date,
    string Status,
    PaymentInfoResponse? PaymentInfo);

public record OrderItemResponse(
    string Id,
    string Title,
    string Author,
    decimal Price,
    int Quantity);

public record PaymentInfoResponse(
    string CardNumber,
    string CardName,
    string Expiry,
    string Cvv,
    string Address);
