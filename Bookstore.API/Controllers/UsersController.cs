using BookstoreAPI.Application.UseCases.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "admin")]
public class UsersController : ControllerBase
{
    private readonly GetAllUsersUseCase _getAllUsersUseCase;
    private readonly DeleteUserUseCase _deleteUserUseCase;

    public UsersController(
        GetAllUsersUseCase getAllUsersUseCase,
        DeleteUserUseCase deleteUserUseCase)
    {
        _getAllUsersUseCase = getAllUsersUseCase;
        _deleteUserUseCase = deleteUserUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _getAllUsersUseCase.ExecuteAsync();
        return Ok(users);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var success = await _deleteUserUseCase.ExecuteAsync(id);
        if (!success)
        {
            return NotFound(new { error = "User not found" });
        }
        return Ok(new { message = "User deleted" });
    }
}
