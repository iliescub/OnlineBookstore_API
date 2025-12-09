namespace BookstoreAPI.Domain.Entities;

public class Order
{
    public string Id { get; private set; }
    public string UserId { get; private set; }
    public string UserName { get; private set; }
    private readonly List<OrderItem> _items = new();
    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();
    public decimal Total { get; private set; }
    public DateTime Date { get; private set; }
    public string Status { get; private set; }
    public string? PaymentProviderId { get; private set; }
    public string? ShippingAddress { get; private set; }

    private Order()
    {
        Id = string.Empty;
        UserId = string.Empty;
        UserName = string.Empty;
        Status = string.Empty;
    }

    public Order(string userId, string userName, List<OrderItem> items, string? shippingAddress = null)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("User ID cannot be empty", nameof(userId));
        if (string.IsNullOrWhiteSpace(userName))
            throw new ArgumentException("User name cannot be empty", nameof(userName));
        if (items == null || items.Count == 0)
            throw new ArgumentException("Order must have at least one item", nameof(items));

        Id = Guid.NewGuid().ToString();
        UserId = userId;
        UserName = userName;
        _items = items;
        Date = DateTime.UtcNow;
        Status = "pending";
        ShippingAddress = shippingAddress;

        CalculateTotal();
    }

    public void AddItem(OrderItem item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        _items.Add(item);
        CalculateTotal();
    }

    public void RemoveItem(string itemId)
    {
        var item = _items.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            _items.Remove(item);
            CalculateTotal();
        }
    }

    public void UpdateStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            throw new ArgumentException("Status cannot be empty", nameof(status));

        Status = status;
    }

    private void CalculateTotal()
    {
        Total = _items.Sum(item => item.Price * item.Quantity);
    }

    public void SetPaymentProviderId(string paymentProviderId)
    {
        if (string.IsNullOrWhiteSpace(paymentProviderId))
            throw new ArgumentException("Payment provider ID cannot be empty", nameof(paymentProviderId));

        PaymentProviderId = paymentProviderId;
    }

    public void SetShippingAddress(string shippingAddress)
    {
        if (string.IsNullOrWhiteSpace(shippingAddress))
            throw new ArgumentException("Shipping address cannot be empty", nameof(shippingAddress));

        ShippingAddress = shippingAddress;
    }
}

public class OrderItem
{
    public string Id { get; private set; }
    public string Title { get; private set; }
    public string Author { get; private set; }
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }

    private OrderItem()
    {
        Id = string.Empty;
        Title = string.Empty;
        Author = string.Empty;
    }

    public OrderItem(string id, string title, string author, decimal price, int quantity)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Item ID cannot be empty", nameof(id));
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));
        if (price <= 0)
            throw new ArgumentException("Price must be greater than zero", nameof(price));
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        Id = id;
        Title = title;
        Author = author;
        Price = price;
        Quantity = quantity;
    }

    public void UpdateQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        Quantity = quantity;
    }
}
