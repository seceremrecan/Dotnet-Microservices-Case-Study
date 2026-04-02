using Auth.Application.Abstractions.Persistence;
using Auth.Application.Abstractions.Security;
using Auth.Application.Abstractions.Tokens;
using Auth.Application.DTOs;
using RefreshTokenEntity = Auth.Domain.Entities.RefreshToken;

namespace Auth.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler
{
    private readonly IAuthRepository _authRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(
        IAuthRepository authRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _authRepository = authRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDto?> HandleAsync(LoginCommand command, CancellationToken cancellationToken = default)
    {
        var user = await _authRepository.GetUserByEmailAsync(command.Email, cancellationToken);

        if (user is null)
        {
            return null;
        }

        var isPasswordValid = _passwordHasher.Verify(command.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            return null;
        }

        var accessToken = _tokenService.CreateAccessToken(user);
        var refreshTokenValue = _tokenService.CreateRefreshToken();

        var refreshToken = new RefreshTokenEntity
        {
            UserId = user.Id,
            Token = refreshTokenValue,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(7),
            IsRevoked = false
        };

        await _authRepository.AddRefreshTokenAsync(refreshToken, cancellationToken);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenValue,
            Email = user.Email,
            Role = user.Role
        };
    }
}