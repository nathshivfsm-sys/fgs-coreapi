using Fgs.User.Infrastructure.Security;

namespace Fgs.User.Tests.Infrastructure;

public sealed class PasswordHasherServiceTests
{
    private readonly PasswordHasherService _hasher = new();

    [Fact]
    public void HashAndVerify_WithSamePassword_ReturnsTrue()
    {
        var hash = _hasher.HashPassword("Str0ng!Passw0rd");
        _hasher.VerifyPassword(hash, "Str0ng!Passw0rd").Should().BeTrue();
    }

    [Fact]
    public void Verify_WithWrongPassword_ReturnsFalse()
    {
        var hash = _hasher.HashPassword("Str0ng!Passw0rd");
        _hasher.VerifyPassword(hash, "Wrong!Passw0rd").Should().BeFalse();
    }
}
