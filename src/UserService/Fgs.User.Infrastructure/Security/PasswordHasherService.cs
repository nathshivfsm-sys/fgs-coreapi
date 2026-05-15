using Fgs.User.Application.Abstractions.Security;
using Microsoft.AspNetCore.Identity;

namespace Fgs.User.Infrastructure.Security;

public sealed class PasswordHasherService : IPasswordHasher
{
    private readonly PasswordHasher<object> _hasher = new();

    public string HashPassword(string password) =>
        _hasher.HashPassword(new object(), password);

    public bool VerifyPassword(string hashedPassword, string providedPassword) =>
        _hasher.VerifyHashedPassword(new object(), hashedPassword, providedPassword)
            != PasswordVerificationResult.Failed;
}
