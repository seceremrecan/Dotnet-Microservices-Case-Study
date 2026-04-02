using Auth.Application.Abstractions.Persistence;
using Auth.Application.Abstractions.Tokens;
using Auth.Application.DTOs;

namespace Auth.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler
{
    private readonly IAuthRepository _authRepository;
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(
        IAuthRepository authRepository,
        ITokenService tokenService)
    {
        _authRepository = authRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDto?> HandleAsync(RefreshTokenCommand command, CancellationToken cancellationToken = default)
    {
        var existingRefreshToken = await _authRepository.GetRefreshTokenAsync(command.RefreshToken, cancellationToken);

        if (existingRefreshToken is null || existingRefreshToken.IsRevoked || existingRefreshToken.ExpiresAtUtc < DateTime.UtcNow)
        {
            return null;
        }

        existingRefreshToken.IsRevoked = true;
        await _authRepository.UpdateRefreshTokenAsync(existingRefreshToken, cancellationToken);

        return null;
    }
}