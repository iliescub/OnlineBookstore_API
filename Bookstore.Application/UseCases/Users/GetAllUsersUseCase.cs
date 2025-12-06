using BookstoreAPI.Application.DTOs.Responses;
using BookstoreAPI.Domain.Repositories;

namespace BookstoreAPI.Application.UseCases.Users;

public class GetAllUsersUseCase
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserResponse>> ExecuteAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(u => new UserResponse(
            u.Id,
            u.Email,
            u.Name,
            u.Role
        ));
    }
}
