using BookstoreAPI.Application.DTOs.Requests;
using BookstoreAPI.Application.UseCases.Books;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly GetAllBooksUseCase _getAllBooksUseCase;
    private readonly GetBookByIdUseCase _getBookByIdUseCase;
    private readonly CreateBookUseCase _createBookUseCase;
    private readonly UpdateBookUseCase _updateBookUseCase;
    private readonly DeleteBookUseCase _deleteBookUseCase;

    public BooksController(
        GetAllBooksUseCase getAllBooksUseCase,
        GetBookByIdUseCase getBookByIdUseCase,
        CreateBookUseCase createBookUseCase,
        UpdateBookUseCase updateBookUseCase,
        DeleteBookUseCase deleteBookUseCase)
    {
        _getAllBooksUseCase = getAllBooksUseCase;
        _getBookByIdUseCase = getBookByIdUseCase;
        _createBookUseCase = createBookUseCase;
        _updateBookUseCase = updateBookUseCase;
        _deleteBookUseCase = deleteBookUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetBooks([FromQuery] string? category = null)
    {
        var books = await _getAllBooksUseCase.ExecuteAsync(category);
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBook(string id)
    {
        var book = await _getBookByIdUseCase.ExecuteAsync(id);
        if (book == null)
        {
            return NotFound(new { error = "Book not found" });
        }
        return Ok(book);
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<IActionResult> CreateBook([FromBody] CreateBookRequest request)
    {
        var book = await _createBookUseCase.ExecuteAsync(request);
        return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
    }

    [Authorize(Roles = "admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(string id, [FromBody] UpdateBookRequest request)
    {
        var book = await _updateBookUseCase.ExecuteAsync(id, request);
        if (book == null)
        {
            return NotFound(new { error = "Book not found" });
        }
        return Ok(book);
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(string id)
    {
        var success = await _deleteBookUseCase.ExecuteAsync(id);
        if (!success)
        {
            return NotFound(new { error = "Book not found" });
        }
        return Ok(new { message = "Book deleted" });
    }
}
