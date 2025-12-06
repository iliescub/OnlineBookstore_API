namespace BookstoreAPI.Domain.Entities;

public class User
{
    public string Id { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string Name { get; private set; }
    public string Role { get; private set; }

    private User()
    {
        Id = string.Empty;
        Email = string.Empty;
        PasswordHash = string.Empty;
        Name = string.Empty;
        Role = string.Empty;
    }

    public User(string email, string passwordHash, string name, string role = "user")
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash cannot be empty", nameof(passwordHash));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        Id = Guid.NewGuid().ToString();
        Email = email;
        PasswordHash = passwordHash;
        Name = name;
        Role = role;
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        Name = name;
    }

    public void UpdatePassword(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash cannot be empty", nameof(passwordHash));

        PasswordHash = passwordHash;
    }

    public void UpdateRole(string role)
    {
        if (string.IsNullOrWhiteSpace(role))
            throw new ArgumentException("Role cannot be empty", nameof(role));

        Role = role;
    }

    public bool IsAdmin() => Role.Equals("admin", StringComparison.OrdinalIgnoreCase);
}
