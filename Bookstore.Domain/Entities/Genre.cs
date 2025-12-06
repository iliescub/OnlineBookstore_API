namespace BookstoreAPI.Domain.Entities;

public class Genre
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Genre()
    {
        Id = string.Empty;
        Name = string.Empty;
        Description = string.Empty;
        CreatedAt = DateTime.UtcNow;
    }

    public Genre(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Genre name cannot be empty", nameof(name));

        Id = Guid.NewGuid().ToString();
        Name = name;
        Description = description ?? string.Empty;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Genre name cannot be empty", nameof(name));

        Name = name;
        Description = description ?? string.Empty;
    }
}
