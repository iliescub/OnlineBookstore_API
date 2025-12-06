using BookstoreAPI.Domain.Entities;

namespace BookstoreAPI.Domain.Services;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
