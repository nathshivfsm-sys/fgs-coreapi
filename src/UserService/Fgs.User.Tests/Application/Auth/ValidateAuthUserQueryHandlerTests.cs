using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Features.Auth;
using Fgs.User.Application.Features.Auth.Queries.ValidateAuthUser;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Fgs.User.Tests.Application.Auth;

public sealed class ValidateAuthUserQueryHandlerTests
{
    [Fact]
    public async Task Handle_WhenProfileAndHeadersMatch_ReturnsSuccess()
    {
        var profile = new FgsUserProfile(
            Guid.NewGuid(),
            "admin@test.com",
            "oid-123",
            10,
            1,
            ["TENANT_ADMIN"]);

        var resolver = new Mock<IFgsUserProfileResolver>();
        resolver
            .Setup(r => r.ResolveByEntraObjectIdAsync("oid-123", It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);

        var handler = new ValidateAuthUserQueryHandler(
            CreateAccessor(CreateAuthenticatedContext("oid-123", "10", "1")),
            resolver.Object);

        var response = await handler.Handle(new ValidateAuthUserQuery(), CancellationToken.None);

        response.Success.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenHeadersMismatch_ReturnsUnauthorized()
    {
        var profile = new FgsUserProfile(
            Guid.NewGuid(),
            "admin@test.com",
            "oid-123",
            10,
            1,
            ["TENANT_ADMIN"]);

        var resolver = new Mock<IFgsUserProfileResolver>();
        resolver
            .Setup(r => r.ResolveByEntraObjectIdAsync("oid-123", It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);

        var handler = new ValidateAuthUserQueryHandler(
            CreateAccessor(CreateAuthenticatedContext("oid-123", "99", "1")),
            resolver.Object);

        var response = await handler.Handle(new ValidateAuthUserQuery(), CancellationToken.None);

        response.Success.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_WhenProfileMissing_ReturnsUnauthorized()
    {
        var resolver = new Mock<IFgsUserProfileResolver>();
        resolver
            .Setup(r => r.ResolveByEntraObjectIdAsync("oid-123", It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsUserProfile?)null);

        var handler = new ValidateAuthUserQueryHandler(
            CreateAccessor(CreateAuthenticatedContext("oid-123", "10", "1")),
            resolver.Object);

        var response = await handler.Handle(new ValidateAuthUserQuery(), CancellationToken.None);

        response.Success.Should().BeFalse();
    }

    [Fact]
    public void TryValidateHeadersAgainstProfile_WhenHeadersMissing_ReturnsFalse()
    {
        var profile = new FgsUserProfile(
            Guid.NewGuid(),
            "admin@test.com",
            "oid-123",
            10,
            1,
            ["TENANT_ADMIN"]);

        var valid = AuthScopeValidation.TryValidateHeadersAgainstProfile(null, 1, profile, out var errors);

        valid.Should().BeFalse();
        errors.Should().Contain("X-Tenant-Id and X-Company-Id headers are required.");
    }

    private static DefaultHttpContext CreateAuthenticatedContext(string oid, string tenantId, string companyId)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.User = new System.Security.Claims.ClaimsPrincipal(
            new System.Security.Claims.ClaimsIdentity(
            [
                new System.Security.Claims.Claim("oid", oid)
            ],
            authenticationType: "Bearer"));
        httpContext.Request.Headers["X-Tenant-Id"] = tenantId;
        httpContext.Request.Headers["X-Company-Id"] = companyId;
        return httpContext;
    }

    private static IHttpContextAccessor CreateAccessor(HttpContext httpContext)
    {
        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(a => a.HttpContext).Returns(httpContext);
        return accessor.Object;
    }
}
