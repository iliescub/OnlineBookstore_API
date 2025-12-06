namespace BookstoreAPI.Domain.Entities;

public class Book
{
    public string Id { get; private set; }
    public string Title { get; private set; }
    public string Author { get; private set; }
    public decimal Price { get; private set; }
    public string Category { get; private set; }
    public int Stock { get; private set; }
    public string Image { get; private set; }

    private Book()
    {
        Id = string.Empty;
        Title = string.Empty;
        Author = string.Empty;
        Category = string.Empty;
        Image = string.Empty;
    }

    public Book(string title, string author, decimal price, string category, int stock, string image)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));
        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException("Author cannot be empty", nameof(author));
        if (price <= 0)
            throw new ArgumentException("Price must be greater than zero", nameof(price));
        if (stock < 0)
            throw new ArgumentException("Stock cannot be negative", nameof(stock));

        Id = Guid.NewGuid().ToString();
        Title = title;
        Author = author;
        Price = price;
        Category = category;
        Stock = stock;
        Image = image;
    }

    public void UpdateDetails(string title, string author, decimal price, string category, string image)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));
        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException("Author cannot be empty", nameof(author));
        if (price <= 0)
            throw new ArgumentException("Price must be greater than zero", nameof(price));

        Title = title;
        Author = author;
        Price = price;
        Category = category;
        Image = image;
    }

    public void UpdateStock(int stock)
    {
        if (stock < 0)
            throw new ArgumentException("Stock cannot be negative", nameof(stock));

        Stock = stock;
    }

    public bool ReduceStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        if (Stock < quantity)
            return false;

        Stock -= quantity;
        return true;
    }

    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        Stock += quantity;
    }

    public bool IsInStock(int quantity) => Stock >= quantity;
}
