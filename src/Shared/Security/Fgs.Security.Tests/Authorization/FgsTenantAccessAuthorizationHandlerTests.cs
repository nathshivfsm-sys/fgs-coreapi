using Fgs.Security.Abstractions;
using Fgs.Security.Authorization.Handlers;
using Fgs.Security.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Fgs.Security.Tests.Authorization;

public sealed class FgsTenantAccessAuthorizationHandlerTests
{
    [Fact]
    public async Task HandleRequirementAsync_WhenScopeValid_Succeeds()
    {
        var handler = CreateHandler(isValidScope: true);
        var httpContext = CreateHttpContext("10", "1");
        var context = new AuthorizationHandlerContext(
            [new FgsTenantAccessRequirement()],
            CreateAuthenticatedPrincipal(),
            httpContext);

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeTrue();
    }

    [Fact]
    public async Task HandleRequirementAsync_WhenHeadersMissing_DoesNotSucceed()
    {
        var handler = CreateHandler(isValidScope: true);
        var httpContext = new DefaultHttpContext();
        var context = new AuthorizationHandlerContext(
            [new FgsTenantAccessRequirement()],
            CreateAuthenticatedPrincipal(),
            httpContext);

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }

    [Fact]
    public async Task HandleRequirementAsync_WhenScopeInvalid_DoesNotSucceed()
    {
        var handler = CreateHandler(isValidScope: false);
        var httpContext = CreateHttpContext("10", "1");
        var context = new AuthorizationHandlerContext(
            [new FgsTenantAccessRequirement()],
            CreateAuthenticatedPrincipal(),
            httpContext);

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }

    private static FgsTenantAccessAuthorizationHandler CreateHandler(bool isValidScope)
    {
        var scopeValidator = new Mock<IFgsTenantScopeValidator>();
        scopeValidator
            .Setup(v => v.IsValidScopeAsync(10, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(isValidScope);

        var services = new ServiceCollection();
        services.AddSingleton(scopeValidator.Object);
        var provider = services.BuildServiceProvider();
        return new FgsTenantAccessAuthorizationHandler(provider.GetRequiredService<IServiceScopeFactory>());
    }

    private static DefaultHttpContext CreateHttpContext(string tenantId, string companyId)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["X-Tenant-Id"] = tenantId;
        httpContext.Request.Headers["X-Company-Id"] = companyId;
        return httpContext;
    }

    private static System.Security.Claims.ClaimsPrincipal CreateAuthenticatedPrincipal() =>
        new(new System.Security.Claims.ClaimsIdentity(authenticationType: "Bearer"));
}
