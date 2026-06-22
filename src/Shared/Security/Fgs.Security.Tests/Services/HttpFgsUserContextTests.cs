using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Fgs.Security.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Fgs.Security.Tests.Services;

public sealed class HttpFgsUserContextTests
{
    [Fact]
    public void DisplayName_UsesNameClaim()
    {
        var context = CreateContext(new Claim("name", "Pat Garcia"));

        context.DisplayName.Should().Be("Pat Garcia");
    }

    [Fact]
    public void DisplayName_CombinesGivenAndFamilyNames()
    {
        var context = CreateContext(
            new Claim("given_name", "Pat"),
            new Claim("family_name", "Garcia"));

        context.DisplayName.Should().Be("Pat Garcia");
    }

    private static HttpFgsUserContext CreateContext(params Claim[] claims)
    {
        var identity = new ClaimsIdentity(claims, authenticationType: "Bearer");
        var principal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = principal };
        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(x => x.HttpContext).Returns(httpContext);
        return new HttpFgsUserContext(accessor.Object);
    }
}
