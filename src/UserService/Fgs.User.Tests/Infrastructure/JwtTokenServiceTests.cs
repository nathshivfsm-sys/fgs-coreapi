using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Fgs.User.Application.Features.Signup;
using Fgs.User.Domain.Entities;
using Fgs.Security.Constants;
using Fgs.Security.Options;
using Fgs.User.Infrastructure.Common.Security;
using Microsoft.Extensions.Options;

namespace Fgs.User.Tests.Infrastructure;

public sealed class JwtTokenServiceTests
{
    private static JwtTokenService CreateService() =>
        new(Options.Create(new JwtOptions
        {
            Issuer = "test-issuer",
            Audience = "test-audience",
            SigningKey = "super-secret-signing-key-32chars!!",
            ExpiryMinutes = 30
        }));

    private static FgsUser SampleUser() => new()
    {
        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
        TenantId = 1,
        CompanyId = 1,
        Email = "USER@TEST.COM",
        DisplayName = "User",
        EntraObjectId = "oid-1"
    };

    [Fact]
    public void CreateToken_ReturnsSignedJwt()
    {
        var token = CreateService().CreateToken(SampleUser(), [SignupConstants.TenantAdminRoleCode]);

        token.Should().NotBeNullOrWhiteSpace();
        token.Split('.').Should().HaveCount(3);
    }

    [Fact]
    public void CreateToken_IncludesRoleClaims()
    {
        var token = CreateService().CreateToken(
            SampleUser(),
            [SignupConstants.TenantAdminRoleCode, "COMPANY_ADMIN"]);

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        jwt.Claims.Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .Should().BeEquivalentTo([SignupConstants.TenantAdminRoleCode, "COMPANY_ADMIN"]);
    }

    [Fact]
    public void CreateToken_WithNoRoles_OmitsRoleClaims()
    {
        var token = CreateService().CreateToken(SampleUser(), []);

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        jwt.Claims.Should().NotContain(c => c.Type == ClaimTypes.Role);
        jwt.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Email && c.Value == "USER@TEST.COM");
    }

    [Fact]
    public void CreateToken_WithoutEntraObjectId_OmitsEntraClaim()
    {
        var user = SampleUser();
        user.EntraObjectId = null;

        var token = CreateService().CreateToken(user, []);

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        jwt.Claims.Should().NotContain(c => c.Type == JwtClaimTypes.EntraObjectId);
    }
}
