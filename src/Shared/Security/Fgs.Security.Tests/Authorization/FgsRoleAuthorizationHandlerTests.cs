using System.Security.Claims;
using Fgs.Security.Authorization;
using Fgs.Security.Authorization.Handlers;
using Fgs.Security.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Fgs.Security.Tests.Authorization;

public sealed class FgsRoleAuthorizationHandlerTests
{
    private readonly FgsRoleAuthorizationHandler _handler = new();

    [Fact]
    public async Task HandleRequirementAsync_WhenUserHasRequiredRole_Succeeds()
    {
        var user = CreateUser(new Claim(ClaimTypes.Role, FgsRoleCodes.TenantAdmin));
        var context = new AuthorizationHandlerContext(
            [new FgsRoleRequirement(FgsRoleCodes.TenantAdmin)],
            user,
            null);

        await _handler.HandleAsync(context);

        context.HasSucceeded.Should().BeTrue();
    }

    [Fact]
    public async Task HandleRequirementAsync_WhenUserMissingRole_DoesNotSucceed()
    {
        var user = CreateUser(new Claim(ClaimTypes.Role, "OTHER_ROLE"));
        var context = new AuthorizationHandlerContext(
            [new FgsRoleRequirement(FgsRoleCodes.TenantAdmin)],
            user,
            null);

        await _handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }

    [Fact]
    public async Task HandleRequirementAsync_WhenRequireAll_MatchesAllRoles()
    {
        var user = CreateUser(
            new Claim(ClaimTypes.Role, FgsRoleCodes.TenantAdmin),
            new Claim(ClaimTypes.Role, FgsRoleCodes.PlatformAdmin));
        var context = new AuthorizationHandlerContext(
            [new FgsRoleRequirement(requireAll: true, FgsRoleCodes.TenantAdmin, FgsRoleCodes.PlatformAdmin)],
            user,
            null);

        await _handler.HandleAsync(context);

        context.HasSucceeded.Should().BeTrue();
    }

    private static ClaimsPrincipal CreateUser(params Claim[] claims)
    {
        var identity = new ClaimsIdentity(claims, authenticationType: "Bearer");
        return new ClaimsPrincipal(identity);
    }
}
