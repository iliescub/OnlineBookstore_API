using BookstoreAPI.Application.DTOs.Requests;
using BookstoreAPI.Application.DTOs.Responses;
using BookstoreAPI.Domain.Entities;
using BookstoreAPI.Domain.Repositories;
using BookstoreAPI.Domain.Services;

namespace BookstoreAPI.Application.UseCases.Auth;

public class RegisterUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public RegisterUseCase(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator tokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<AuthResponse?> ExecuteAsync(RegisterRequest request)
    {
        if (await _userRepository.ExistsByEmailAsync(request.Email))
            return null;

        var passwordHash = _passwordHasher.HashPassword(request.Password);
        var user = new User(request.Email, passwordHash, request.Name);

        await _userRepository.AddAsync(user);

        var token = _tokenGenerator.GenerateToken(user);

        return new AuthResponse(
            token,
            new UserResponse(user.Id, user.Email, user.Name, user.Role)
        );
    }
}
