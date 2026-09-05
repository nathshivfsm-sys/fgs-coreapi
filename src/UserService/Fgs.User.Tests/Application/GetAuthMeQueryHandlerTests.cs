using Fgs.Contracts.Auth;
using Fgs.Security.Abstractions;
using Fgs.Security.UserAuth;
using Fgs.User.Application.Features.Auth;
using Fgs.User.Application.Features.Auth.Queries.GetAuthMe;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace Fgs.User.Tests.Application;

public sealed class GetAuthMeQueryHandlerTests
{
    private static readonly UserAuthProfileDto ActiveProfile = new(
        Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
        "user@test.com",
        "oid-123",
        10,
        20,
        true,
        false,
        ["TENANT_ADMIN"],
        ["USERS.READ"],
        ["ALL_LOCATIONS"],
        [new PublicEndpointAuthDto("API", "PROD", "https://api.example.com", "Prod API")]);

    [Fact]
    public async Task Handle_WhenUnauthenticated_ReturnsUnauthorized()
    {
        var handler = CreateHandler(isAuthenticated: false);
        var response = await handler.Handle(new GetAuthMeQuery(), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(401);
        response.Errors.Should().Contain(AuthErrorMessages.Unauthenticated);
    }

    [Fact]
    public async Task Handle_WhenMissingEntraObjectId_ReturnsNotFound()
    {
        var handler = CreateHandler(entraObjectId: null);
        var response = await handler.Handle(new GetAuthMeQuery(), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
        response.Errors.Should().Contain(AuthErrorMessages.UserNotFound);
    }

    [Fact]
    public async Task Handle_WhenProfileMissing_ReturnsNotFound()
    {
        var profileStore = new Mock<IUserAuthProfileStore>();
        profileStore.Setup(s => s.GetOrLoadAsync("oid-123", It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserAuthProfileDto?)null);

        var handler = CreateHandler(profileStore: profileStore.Object);
        var response = await handler.Handle(new GetAuthMeQuery(), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task Handle_WhenProfileInactive_ReturnsNotFound()
    {
        var inactive = ActiveProfile with { IsActive = false };
        var profileStore = new Mock<IUserAuthProfileStore>();
        profileStore.Setup(s => s.GetOrLoadAsync("oid-123", It.IsAny<CancellationToken>()))
            .ReturnsAsync(inactive);

        var handler = CreateHandler(profileStore: profileStore.Object);
        var response = await handler.Handle(new GetAuthMeQuery(), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task Handle_WithUserContextScope_ReturnsAuthMe()
    {
        var handler = CreateHandler(tenantId: 10, companyId: 20);
        var response = await handler.Handle(new GetAuthMeQuery(), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Email.Should().Be("user@test.com");
        response.Data.TenantId.Should().Be(10);
        response.Data.CompanyId.Should().Be(20);
        response.Data.Roles.Should().Contain("TENANT_ADMIN");
    }

    [Fact]
    public async Task Handle_WithClaimScopeFallback_UsesClaims()
    {
        var claims = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim("tenant_id", "11"),
            new Claim("company_id", "21")
        ], "test"));

        var handler = CreateHandler(tenantId: null, companyId: null, claims: claims);
        var response = await handler.Handle(new GetAuthMeQuery(), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.TenantId.Should().Be(11);
        response.Data.CompanyId.Should().Be(21);
    }

    [Fact]
    public async Task Handle_WithProfileScopeFallback_UsesProfile()
    {
        var handler = CreateHandler(tenantId: null, companyId: null);
        var response = await handler.Handle(new GetAuthMeQuery(), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.TenantId.Should().Be(10);
        response.Data.CompanyId.Should().Be(20);
    }

    private static GetAuthMeQueryHandler CreateHandler(
        bool isAuthenticated = true,
        string? entraObjectId = "oid-123",
        long? tenantId = 10,
        long? companyId = 20,
        ClaimsPrincipal? claims = null,
        IUserAuthProfileStore? profileStore = null)
    {
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(c => c.IsAuthenticated).Returns(isAuthenticated);
        userContext.SetupGet(c => c.EntraObjectId).Returns(entraObjectId);
        userContext.SetupGet(c => c.TenantId).Returns(tenantId);
        userContext.SetupGet(c => c.CompanyId).Returns(companyId);

        var store = profileStore ?? CreateDefaultProfileStore();
        var httpContext = new DefaultHttpContext { User = claims ?? new ClaimsPrincipal() };
        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        httpContextAccessor.SetupGet(a => a.HttpContext).Returns(httpContext);

        return new GetAuthMeQueryHandler(userContext.Object, store, httpContextAccessor.Object);
    }

    private static IUserAuthProfileStore CreateDefaultProfileStore()
    {
        var profileStore = new Mock<IUserAuthProfileStore>();
        profileStore.Setup(s => s.GetOrLoadAsync("oid-123", It.IsAny<CancellationToken>()))
            .ReturnsAsync(ActiveProfile);
        return profileStore.Object;
    }
}
