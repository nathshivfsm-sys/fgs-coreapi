using Fgs.User.Infrastructure.Common.Security;

namespace Fgs.User.Tests.Infrastructure;

public sealed class InvitationTokenServiceTests
{
    private readonly InvitationTokenService _service = new();

    [Fact]
    public void GenerateToken_ReturnsNonEmptyUrlSafeValue()
    {
        var token = _service.GenerateToken();
        token.Should().NotBeNullOrWhiteSpace();
        token.Should().NotContain("+").And.NotContain("/");
    }

    [Fact]
    public void VerifyToken_WithMatchingHash_ReturnsTrue()
    {
        var token = _service.GenerateToken();
        var hash = _service.HashToken(token);
        _service.VerifyToken(token, hash).Should().BeTrue();
    }

    [Fact]
    public void VerifyToken_WithWrongToken_ReturnsFalse()
    {
        var hash = _service.HashToken(_service.GenerateToken());
        _service.VerifyToken("wrong-token", hash).Should().BeFalse();
    }
}
