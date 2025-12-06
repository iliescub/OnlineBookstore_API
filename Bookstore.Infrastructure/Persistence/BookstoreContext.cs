using System.Text.Json;
using BookstoreAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BookstoreAPI.Infrastructure.Persistence;

public class BookstoreContext : DbContext
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = false
    };

    public BookstoreContext(DbContextOptions<BookstoreContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).HasMaxLength(50);
            entity.Property(u => u.Email).HasMaxLength(256).IsRequired();
            entity.Property(u => u.PasswordHash).HasColumnName("PasswordHash").HasMaxLength(200).IsRequired();
            entity.Property(u => u.Name).HasMaxLength(128).IsRequired();
            entity.Property(u => u.Role).HasMaxLength(32).IsRequired();
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToTable("Books");
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Id).HasMaxLength(50);
            entity.Property(b => b.Title).HasMaxLength(256).IsRequired();
            entity.Property(b => b.Author).HasMaxLength(128).IsRequired();
            entity.Property(b => b.Price).HasColumnType("decimal(10,2)");
            entity.Property(b => b.Category).HasMaxLength(64).IsRequired();
            entity.Property(b => b.Image).HasColumnName("ImageUrl").HasMaxLength(512);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders");
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Id).HasMaxLength(50);
            entity.Property(o => o.UserId).HasMaxLength(50).IsRequired();
            entity.Property(o => o.UserName).HasMaxLength(128).IsRequired();
            entity.Property(o => o.Status).HasMaxLength(32).IsRequired();
            entity.Property(o => o.Total).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Date).HasColumnName("OrderDate").HasDefaultValueSql("NOW()");

            // Ignore the public Items property since it's read-only
            entity.Ignore(o => o.Items);

            // Map only the private _items backing field
            entity.Property<List<OrderItem>>("_items")
                  .HasColumnName("Items")
                  .HasConversion(
                      v => JsonSerializer.Serialize(v, SerializerOptions),
                      v => string.IsNullOrWhiteSpace(v)
                          ? new List<OrderItem>()
                          : JsonSerializer.Deserialize<List<OrderItem>>(v, SerializerOptions) ?? new List<OrderItem>()
                  )
                  .Metadata.SetValueComparer(new ValueComparer<List<OrderItem>>(
                      (c1, c2) => c1.SequenceEqual(c2),
                      c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                      c => c.ToList()
                  ));

            entity.OwnsOne(o => o.PaymentInfo, owned =>
            {
                owned.Property(p => p.CardNumber).HasColumnName("CardNumber").HasMaxLength(64);
                owned.Property(p => p.CardName).HasColumnName("CardName").HasMaxLength(128);
                owned.Property(p => p.Expiry).HasColumnName("Expiry").HasMaxLength(16);
                owned.Property(p => p.Cvv).HasColumnName("Cvv").HasMaxLength(8);
                owned.Property(p => p.Address).HasColumnName("Address").HasMaxLength(256);
            });

            entity.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(o => o.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.ToTable("Genres");
            entity.HasKey(g => g.Id);
            entity.Property(g => g.Id).HasMaxLength(50);
            entity.Property(g => g.Name).HasMaxLength(100).IsRequired();
            entity.Property(g => g.Description).HasMaxLength(500);
            entity.Property(g => g.CreatedAt).HasDefaultValueSql("NOW()");
        });
    }
}
