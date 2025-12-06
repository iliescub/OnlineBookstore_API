using BookstoreAPI.Application.DTOs.Requests;
using BookstoreAPI.Application.DTOs.Responses;
using BookstoreAPI.Domain.Repositories;
using BookstoreAPI.Domain.Services;

namespace BookstoreAPI.Application.UseCases.Auth;

public class LoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public LoginUseCase(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator tokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<AuthResponse?> ExecuteAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
            return null;

        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            return null;

        var token = _tokenGenerator.GenerateToken(user);

        return new AuthResponse(
            token,
            new UserResponse(user.Id, user.Email, user.Name, user.Role)
        );
    }
}
