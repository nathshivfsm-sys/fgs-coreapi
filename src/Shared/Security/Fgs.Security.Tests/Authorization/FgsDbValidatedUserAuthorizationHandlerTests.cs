using System.Security.Claims;
using Fgs.Security.Abstractions;
using Fgs.Security.Authorization.Handlers;
using Fgs.Security.Authorization.Requirements;
using Fgs.Security.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Security.Tests.Authorization;

public sealed class FgsDbValidatedUserAuthorizationHandlerTests
{
    [Fact]
    public async Task HandleRequirementAsync_WhenEnrichedAndActive_Succeeds()
    {
        var handler = CreateHandler(isActive: true);
        var context = new AuthorizationHandlerContext(
            [new FgsDbValidatedUserRequirement()],
            CreateEnrichedUser(),
            null);

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeTrue();
    }

    [Fact]
    public async Task HandleRequirementAsync_WhenEnrichedButInactive_DoesNotSucceed()
    {
        var handler = CreateHandler(isActive: false);
        var context = new AuthorizationHandlerContext(
            [new FgsDbValidatedUserRequirement()],
            CreateEnrichedUser(),
            null);

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }

    [Fact]
    public async Task HandleRequirementAsync_WhenNotEnriched_DoesNotSucceed()
    {
        var handler = CreateHandler(isActive: true);
        var identity = new ClaimsIdentity([new Claim("oid", "oid-123")], authenticationType: "Bearer");
        var context = new AuthorizationHandlerContext(
            [new FgsDbValidatedUserRequirement()],
            new ClaimsPrincipal(identity),
            null);

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }

    private static FgsDbValidatedUserAuthorizationHandler CreateHandler(bool isActive)
    {
        var services = new ServiceCollection();
        services.AddSingleton<IFgsUserStatusValidator>(new TestUserStatusValidator(isActive));
        var provider = services.BuildServiceProvider();
        return new FgsDbValidatedUserAuthorizationHandler(provider.GetRequiredService<IServiceScopeFactory>());
    }

    private static ClaimsPrincipal CreateEnrichedUser()
    {
        var identity = new ClaimsIdentity(authenticationType: "Bearer");
        var principal = new ClaimsPrincipal(identity);
        FgsClaimsEnrichment.Apply(
            principal,
            new Models.FgsAuthenticatedUserProfile(
                Guid.NewGuid(),
                "admin@test.com",
                "oid-123",
                ["TENANT_ADMIN"]));
        return principal;
    }

    private sealed class TestUserStatusValidator(bool isActive) : IFgsUserStatusValidator
    {
        public Task<bool> IsActiveAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult(isActive);
    }
}
