using System.Security.Cryptography;
using System.Text;
using Auth.Application.Abstractions.Security;

namespace Auth.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    public bool Verify(string password, string passwordHash)
    {
        var hashedInput = Hash(password);
        return hashedInput == passwordHash;
    }
}