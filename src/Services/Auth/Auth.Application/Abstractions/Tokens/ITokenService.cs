using Auth.Domain.Entities;

namespace Auth.Application.Abstractions.Tokens;

public interface ITokenService
{
    string CreateAccessToken(AppUser user);
    string CreateRefreshToken();
}