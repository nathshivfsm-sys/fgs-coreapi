using System.Security.Claims;
using Fgs.Security.Authorization.Handlers;
using Fgs.Security.Authorization.Requirements;
using Fgs.Security.Services;
using Microsoft.AspNetCore.Authorization;

namespace Fgs.Security.Tests.Authorization;

public sealed class FgsDbValidatedUserAuthorizationHandlerTests
{
    private readonly FgsDbValidatedUserAuthorizationHandler _handler = new();

    [Fact]
    public async Task HandleRequirementAsync_WhenEnriched_Succeeds()
    {
        var user = CreateEnrichedUser();
        var context = new AuthorizationHandlerContext(
            [new FgsDbValidatedUserRequirement()],
            user,
            null);

        await _handler.HandleAsync(context);

        context.HasSucceeded.Should().BeTrue();
    }

    [Fact]
    public async Task HandleRequirementAsync_WhenNotEnriched_DoesNotSucceed()
    {
        var identity = new ClaimsIdentity([new Claim("oid", "oid-123")], authenticationType: "Bearer");
        var context = new AuthorizationHandlerContext(
            [new FgsDbValidatedUserRequirement()],
            new ClaimsPrincipal(identity),
            null);

        await _handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
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
                10,
                1,
                ["TENANT_ADMIN"]));
        return principal;
    }
}
