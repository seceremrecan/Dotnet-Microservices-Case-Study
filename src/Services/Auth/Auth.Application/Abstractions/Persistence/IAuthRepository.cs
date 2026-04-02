using Auth.Domain.Entities;

namespace Auth.Application.Abstractions.Persistence;

public interface IAuthRepository
{
    Task AddUserAsync(AppUser user, CancellationToken cancellationToken = default);
    Task<AppUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    Task<RefreshToken?> GetRefreshTokenAsync(string token, CancellationToken cancellationToken = default);
    Task UpdateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
}