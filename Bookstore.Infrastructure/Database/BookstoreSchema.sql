USE master;
GO

IF DB_ID('BookstoreDB') IS NOT NULL
BEGIN
    ALTER DATABASE BookstoreDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE BookstoreDB;
END
GO

CREATE DATABASE BookstoreDB;
GO

USE BookstoreDB;
GO

IF OBJECT_ID('dbo.Orders', 'U') IS NOT NULL DROP TABLE dbo.Orders;
IF OBJECT_ID('dbo.Books', 'U') IS NOT NULL DROP TABLE dbo.Books;
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL DROP TABLE dbo.Users;
GO

CREATE TABLE dbo.Users
(
    Id NVARCHAR(50) NOT NULL PRIMARY KEY,
    Email NVARCHAR(256) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(200) NOT NULL,
    Name NVARCHAR(128) NOT NULL,
    Role NVARCHAR(32) NOT NULL
);

CREATE TABLE dbo.Books
(
    Id NVARCHAR(50) NOT NULL PRIMARY KEY,
    Title NVARCHAR(256) NOT NULL,
    Author NVARCHAR(128) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    Category NVARCHAR(64) NOT NULL,
    Stock INT NOT NULL,
    ImageUrl NVARCHAR(512) NULL
);

CREATE TABLE dbo.Orders
(
    Id NVARCHAR(50) NOT NULL PRIMARY KEY,
    UserId NVARCHAR(50) NOT NULL,
    UserName NVARCHAR(128) NOT NULL,
    Items NVARCHAR(MAX) NOT NULL,
    Total DECIMAL(10, 2) NOT NULL,
    OrderDate DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    Status NVARCHAR(32) NOT NULL,
    CardNumber NVARCHAR(64) NULL,
    CardName NVARCHAR(128) NULL,
    Expiry NVARCHAR(16) NULL,
    Cvv NVARCHAR(8) NULL,
    Address NVARCHAR(256) NULL,
    CONSTRAINT FK_Orders_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(Id)
);

INSERT INTO dbo.Users (Id, Email, PasswordHash, Name, Role)
VALUES
    ('1', 'admin@bookstore.com', '$2a$11$sy8dywryMYbPLdwvqel/TO7BfIJLF4nhxWVMad5NSFgmqbzJCm1pC', 'Admin User', 'admin'),
    ('2', 'user@example.com', '$2a$11$T7bvLWaoITbUHqsTGPSyIOR66XCGFQohNt5g1QF2t/qVbDaX86eFm', 'John Doe', 'user');

INSERT INTO dbo.Books (Id, Title, Author, Price, Category, Stock, ImageUrl)
VALUES
    ('1', 'The Great Gatsby', 'F. Scott Fitzgerald', 12.99, 'Classic', 15, 'https://images.unsplash.com/photo-1544947950-fa07a98d237f?w=200&h=300&fit=crop'),
    ('2', '1984', 'George Orwell', 14.99, 'Classic', 20, 'https://images.unsplash.com/photo-1495640452828-3df6795cf69b?w=200&h=300&fit=crop'),
    ('3', 'To Kill a Mockingbird', 'Harper Lee', 13.99, 'Classic', 18, 'https://images.unsplash.com/photo-1543002588-bfa74002ed7e?w=200&h=300&fit=crop'),
    ('4', 'The Catcher in the Rye', 'J.D. Salinger', 11.99, 'Classic', 12, 'https://images.unsplash.com/photo-1512820790803-83ca734da794?w=200&h=300&fit=crop'),
    ('5', 'Pride and Prejudice', 'Jane Austen', 10.99, 'Romance', 25, 'https://images.unsplash.com/photo-1524578271613-d550eacf6090?w=200&h=300&fit=crop'),
    ('6', 'The Hobbit', 'J.R.R. Tolkien', 15.99, 'Fantasy', 30, 'https://images.unsplash.com/photo-1621351183012-e2f9972dd9bf?w=200&h=300&fit=crop'),
    ('7', 'Brave New World', 'Aldous Huxley', 13.49, 'Classic', 22, 'https://images.unsplash.com/photo-1473860445360-1d1383f86efc?w=200&h=300&fit=crop'),
    ('8', 'The Name of the Wind', 'Patrick Rothfuss', 16.99, 'Fantasy', 28, 'https://images.unsplash.com/photo-1516979187457-637abb4f9353?w=200&h=300&fit=crop'),
    ('9', 'The Alchemist', 'Paulo Coelho', 11.49, 'Philosophy', 32, 'https://images.unsplash.com/photo-1528209392023-0f259f36c5db?w=200&h=300&fit=crop'),
    ('10', 'Sapiens: A Brief History of Humankind', 'Yuval Noah Harari', 18.99, 'Non-Fiction', 27, 'https://images.unsplash.com/photo-1507842217343-583bb7270b66?w=200&h=300&fit=crop'),
    ('11', 'Becoming', 'Michelle Obama', 17.49, 'Biography', 24, 'https://images.unsplash.com/photo-1523731407965-2430cd12f5e4?w=200&h=300&fit=crop'),
    ('12', 'Atomic Habits', 'James Clear', 16.49, 'Self-Help', 35, 'https://images.unsplash.com/photo-1472289065668-ce650ac443d2?w=200&h=300&fit=crop'),
    ('13', 'Dune', 'Frank Herbert', 19.99, 'Science Fiction', 26, 'https://images.unsplash.com/photo-1521587760476-6c12a4b040da?w=200&h=300&fit=crop'),
    ('14', 'Educated', 'Tara Westover', 15.49, 'Memoir', 21, 'https://images.unsplash.com/photo-1512820790803-83ca734da794?w=200&h=300&fit=crop'),
    ('15', 'The Midnight Library', 'Matt Haig', 14.59, 'Fiction', 29, 'https://images.unsplash.com/photo-1528209391386-9886a673166b?w=200&h=300&fit=crop'),
    ('16', 'Project Hail Mary', 'Andy Weir', 18.49, 'Science Fiction', 23, 'https://images.unsplash.com/photo-1449158416918-03dd43b09a10?w=200&h=300&fit=crop');
