using Auth.Application.Abstractions.Persistence;
using Auth.Application.Abstractions.Security;
using Auth.Domain.Entities;

namespace Auth.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler
{
    private readonly IAuthRepository _authRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(IAuthRepository authRepository, IPasswordHasher passwordHasher)
    {
        _authRepository = authRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> HandleAsync(RegisterCommand command, CancellationToken cancellationToken = default)
    {
        var existingUser = await _authRepository.GetUserByEmailAsync(command.Email, cancellationToken);

        if (existingUser is not null)
        {
            return false;
        }

        var user = new AppUser
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            PasswordHash = _passwordHasher.Hash(command.Password),
            Role = "User"
        };

        await _authRepository.AddUserAsync(user, cancellationToken);

        return true;
    }
}