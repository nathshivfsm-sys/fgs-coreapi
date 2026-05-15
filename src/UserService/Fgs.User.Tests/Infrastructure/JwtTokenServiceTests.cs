using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using Fgs.User.Infrastructure.Options;
using Fgs.User.Infrastructure.Security;
using Microsoft.Extensions.Options;

namespace Fgs.User.Tests.Infrastructure;

public sealed class JwtTokenServiceTests
{
    [Fact]
    public void CreateToken_ReturnsSignedJwt()
    {
        var service = new JwtTokenService(Options.Create(new JwtOptions
        {
            Issuer = "test-issuer",
            Audience = "test-audience",
            SigningKey = "super-secret-signing-key-32chars!!",
            ExpiryMinutes = 30
        }));

        var token = service.CreateToken(new FgsUser
        {
            Id = Guid.NewGuid(),
            TenantId = Guid.NewGuid(),
            CompanyId = Guid.NewGuid(),
            Email = "user@test.com",
            NormalizedEmail = "USER@TEST.COM",
            DisplayName = "User",
            Role = UserRoleType.Admin,
            EntraObjectId = "oid-1"
        });

        token.Should().NotBeNullOrWhiteSpace();
        token.Split('.').Should().HaveCount(3);
    }
}
