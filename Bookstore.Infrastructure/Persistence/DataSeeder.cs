using BookstoreAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookstoreAPI.Infrastructure.Persistence;

public static class DataSeeder
{
    public static async Task SeedAsync(BookstoreContext context)
    {
        // Check if data already exists
        if (await context.Genres.AnyAsync() || await context.Books.AnyAsync())
        {
            return; // Database already seeded
        }

        // Create 8 genres
        var genres = new List<Genre>
        {
            new Genre("Fiction", "Imaginative storytelling and narrative literature"),
            new Genre("Science Fiction", "Futuristic and scientific exploration in literature"),
            new Genre("Mystery", "Suspenseful stories involving crime and investigation"),
            new Genre("Romance", "Love stories and romantic relationships"),
            new Genre("Fantasy", "Magical and supernatural adventures"),
            new Genre("Thriller", "Fast-paced, edge-of-your-seat suspense"),
            new Genre("Historical Fiction", "Stories set in past time periods"),
            new Genre("Biography", "True stories of remarkable lives")
        };

        await context.Genres.AddRangeAsync(genres);
        await context.SaveChangesAsync();

        // Create 50 books across the 8 genres
        // Constructor: Book(string title, string author, decimal price, string category, int stock, string image)
        var books = new List<Book>
        {
            // Fiction (7 books)
            new Book("To Kill a Mockingbird", "Harper Lee", 18.99m, "Fiction", 25, "https://images.unsplash.com/photo-1544947950-fa07a98d237f?w=400"),
            new Book("1984", "George Orwell", 16.99m, "Fiction", 30, "https://images.unsplash.com/photo-1543002588-bfa74002ed7e?w=400"),
            new Book("The Great Gatsby", "F. Scott Fitzgerald", 15.99m, "Fiction", 20, "https://images.unsplash.com/photo-1512820790803-83ca734da794?w=400"),
            new Book("Pride and Prejudice", "Jane Austen", 14.99m, "Fiction", 18, "https://images.unsplash.com/photo-1544716278-ca5e3f4abd8c?w=400"),
            new Book("The Catcher in the Rye", "J.D. Salinger", 17.99m, "Fiction", 22, "https://images.unsplash.com/photo-1541963463532-d68292c34b19?w=400"),
            new Book("Brave New World", "Aldous Huxley", 16.50m, "Fiction", 15, "https://images.unsplash.com/photo-1589998059171-988d887df646?w=400"),
            new Book("The Handmaid's Tale", "Margaret Atwood", 19.99m, "Fiction", 28, "https://images.unsplash.com/photo-1506880018603-83d5b814b5a6?w=400"),

            // Science Fiction (6 books)
            new Book("Dune", "Frank Herbert", 24.99m, "Science Fiction", 35, "https://images.unsplash.com/photo-1518373714866-3f1478910cc0?w=400"),
            new Book("Foundation", "Isaac Asimov", 22.99m, "Science Fiction", 20, "https://images.unsplash.com/photo-1495446815901-a7297e633e8d?w=400"),
            new Book("The Martian", "Andy Weir", 21.99m, "Science Fiction", 40, "https://images.unsplash.com/photo-1516979187457-637abb4f9353?w=400"),
            new Book("Ender's Game", "Orson Scott Card", 19.99m, "Science Fiction", 25, "https://images.unsplash.com/photo-1532012197267-da84d127e765?w=400"),
            new Book("Neuromancer", "William Gibson", 20.99m, "Science Fiction", 18, "https://images.unsplash.com/photo-1524995997946-a1c2e315a42f?w=400"),
            new Book("The Left Hand of Darkness", "Ursula K. Le Guin", 18.99m, "Science Fiction", 15, "https://images.unsplash.com/photo-1497633762265-9d179a990aa6?w=400"),

            // Mystery (6 books)
            new Book("The Girl with the Dragon Tattoo", "Stieg Larsson", 23.99m, "Mystery", 30, "https://images.unsplash.com/photo-1543002588-bfa74002ed7e?w=400"),
            new Book("Gone Girl", "Gillian Flynn", 22.99m, "Mystery", 28, "https://images.unsplash.com/photo-1544947950-fa07a98d237f?w=400"),
            new Book("The Da Vinci Code", "Dan Brown", 21.99m, "Mystery", 35, "https://images.unsplash.com/photo-1512820790803-83ca734da794?w=400"),
            new Book("Big Little Lies", "Liane Moriarty", 20.99m, "Mystery", 25, "https://images.unsplash.com/photo-1544716278-ca5e3f4abd8c?w=400"),
            new Book("In the Woods", "Tana French", 19.99m, "Mystery", 20, "https://images.unsplash.com/photo-1541963463532-d68292c34b19?w=400"),
            new Book("The Silent Patient", "Alex Michaelides", 24.99m, "Mystery", 32, "https://images.unsplash.com/photo-1589998059171-988d887df646?w=400"),

            // Romance (6 books)
            new Book("The Notebook", "Nicholas Sparks", 17.99m, "Romance", 28, "https://images.unsplash.com/photo-1506880018603-83d5b814b5a6?w=400"),
            new Book("Outlander", "Diana Gabaldon", 25.99m, "Romance", 22, "https://images.unsplash.com/photo-1518373714866-3f1478910cc0?w=400"),
            new Book("Me Before You", "Jojo Moyes", 18.99m, "Romance", 30, "https://images.unsplash.com/photo-1495446815901-a7297e633e8d?w=400"),
            new Book("The Fault in Our Stars", "John Green", 16.99m, "Romance", 35, "https://images.unsplash.com/photo-1516979187457-637abb4f9353?w=400"),
            new Book("Call Me by Your Name", "André Aciman", 19.99m, "Romance", 18, "https://images.unsplash.com/photo-1532012197267-da84d127e765?w=400"),
            new Book("The Time Traveler's Wife", "Audrey Niffenegger", 20.99m, "Romance", 24, "https://images.unsplash.com/photo-1524995997946-a1c2e315a42f?w=400"),

            // Fantasy (7 books)
            new Book("The Hobbit", "J.R.R. Tolkien", 22.99m, "Fantasy", 40, "https://images.unsplash.com/photo-1497633762265-9d179a990aa6?w=400"),
            new Book("Harry Potter and the Sorcerer's Stone", "J.K. Rowling", 21.99m, "Fantasy", 50, "https://images.unsplash.com/photo-1543002588-bfa74002ed7e?w=400"),
            new Book("The Name of the Wind", "Patrick Rothfuss", 24.99m, "Fantasy", 28, "https://images.unsplash.com/photo-1544947950-fa07a98d237f?w=400"),
            new Book("A Game of Thrones", "George R.R. Martin", 26.99m, "Fantasy", 35, "https://images.unsplash.com/photo-1512820790803-83ca734da794?w=400"),
            new Book("The Way of Kings", "Brandon Sanderson", 27.99m, "Fantasy", 30, "https://images.unsplash.com/photo-1544716278-ca5e3f4abd8c?w=400"),
            new Book("The Lion, the Witch and the Wardrobe", "C.S. Lewis", 18.99m, "Fantasy", 25, "https://images.unsplash.com/photo-1541963463532-d68292c34b19?w=400"),
            new Book("American Gods", "Neil Gaiman", 23.99m, "Fantasy", 22, "https://images.unsplash.com/photo-1589998059171-988d887df646?w=400"),

            // Thriller (6 books)
            new Book("The Girl on the Train", "Paula Hawkins", 21.99m, "Thriller", 32, "https://images.unsplash.com/photo-1506880018603-83d5b814b5a6?w=400"),
            new Book("The Silence of the Lambs", "Thomas Harris", 22.99m, "Thriller", 28, "https://images.unsplash.com/photo-1518373714866-3f1478910cc0?w=400"),
            new Book("The Bourne Identity", "Robert Ludlum", 20.99m, "Thriller", 25, "https://images.unsplash.com/photo-1495446815901-a7297e633e8d?w=400"),
            new Book("The Shining", "Stephen King", 23.99m, "Thriller", 30, "https://images.unsplash.com/photo-1516979187457-637abb4f9353?w=400"),
            new Book("Sharp Objects", "Gillian Flynn", 19.99m, "Thriller", 24, "https://images.unsplash.com/photo-1532012197267-da84d127e765?w=400"),
            new Book("Before I Go to Sleep", "S.J. Watson", 18.99m, "Thriller", 20, "https://images.unsplash.com/photo-1524995997946-a1c2e315a42f?w=400"),

            // Historical Fiction (6 books)
            new Book("All the Light We Cannot See", "Anthony Doerr", 24.99m, "Historical Fiction", 28, "https://images.unsplash.com/photo-1497633762265-9d179a990aa6?w=400"),
            new Book("The Book Thief", "Markus Zusak", 22.99m, "Historical Fiction", 32, "https://images.unsplash.com/photo-1543002588-bfa74002ed7e?w=400"),
            new Book("The Nightingale", "Kristin Hannah", 23.99m, "Historical Fiction", 30, "https://images.unsplash.com/photo-1544947950-fa07a98d237f?w=400"),
            new Book("The Pillars of the Earth", "Ken Follett", 26.99m, "Historical Fiction", 25, "https://images.unsplash.com/photo-1512820790803-83ca734da794?w=400"),
            new Book("Wolf Hall", "Hilary Mantel", 25.99m, "Historical Fiction", 20, "https://images.unsplash.com/photo-1544716278-ca5e3f4abd8c?w=400"),
            new Book("The Help", "Kathryn Stockett", 21.99m, "Historical Fiction", 35, "https://images.unsplash.com/photo-1541963463532-d68292c34b19?w=400"),

            // Biography (6 books)
            new Book("Steve Jobs", "Walter Isaacson", 28.99m, "Biography", 22, "https://images.unsplash.com/photo-1589998059171-988d887df646?w=400"),
            new Book("Educated", "Tara Westover", 26.99m, "Biography", 30, "https://images.unsplash.com/photo-1506880018603-83d5b814b5a6?w=400"),
            new Book("Becoming", "Michelle Obama", 29.99m, "Biography", 35, "https://images.unsplash.com/photo-1518373714866-3f1478910cc0?w=400"),
            new Book("The Diary of a Young Girl", "Anne Frank", 15.99m, "Biography", 40, "https://images.unsplash.com/photo-1495446815901-a7297e633e8d?w=400"),
            new Book("Long Walk to Freedom", "Nelson Mandela", 27.99m, "Biography", 18, "https://images.unsplash.com/photo-1516979187457-637abb4f9353?w=400"),
            new Book("Born a Crime", "Trevor Noah", 24.99m, "Biography", 28, "https://images.unsplash.com/photo-1532012197267-da84d127e765?w=400")
        };

        await context.Books.AddRangeAsync(books);
        await context.SaveChangesAsync();
    }
}
