using Fgs.Contracts.Auth;
using Fgs.Contracts.Api;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Fgs.Security.UserAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace Fgs.Security.Tests.Authorization;

public sealed class PermissionAuthorizationFilterTests
{
    [Fact]
    public async Task Allows_TenantAdmin_WithoutExplicitPermission()
    {
        var context = CreateContext(CreateProfile(roles: [FgsRoleCodes.TenantAdmin]));
        var filter = new PermissionAuthorizationFilter([FgsPermissionCodes.SetupCreate]);

        await filter.OnAuthorizationAsync(context);

        context.Result.Should().BeNull();
    }

    [Fact]
    public async Task Allows_WhenProfileHasRequiredPermission()
    {
        var context = CreateContext(CreateProfile(permissions: [FgsPermissionCodes.SetupCreate]));
        var filter = new PermissionAuthorizationFilter([FgsPermissionCodes.SetupCreate]);

        await filter.OnAuthorizationAsync(context);

        context.Result.Should().BeNull();
    }

    [Fact]
    public async Task Denies_WhenAuthenticatedProfileLacksPermission()
    {
        var context = CreateContext(CreateProfile(permissions: [FgsPermissionCodes.SetupView]), authenticated: true);
        var filter = new PermissionAuthorizationFilter([FgsPermissionCodes.SetupCreate]);

        await filter.OnAuthorizationAsync(context);

        context.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
    }

    [Fact]
    public async Task Allows_InternalCaller_WithoutProfile()
    {
        var context = CreateContext(profile: null, authenticated: false);
        var filter = new PermissionAuthorizationFilter([FgsPermissionCodes.SetupCreate]);

        await filter.OnAuthorizationAsync(context);

        context.Result.Should().BeNull();
    }

    private static AuthorizationFilterContext CreateContext(
        UserAuthProfileDto? profile,
        bool authenticated = true)
    {
        var httpContext = new DefaultHttpContext();
        if (profile is not null)
        {
            httpContext.Items[UserAuthHttpContextKeys.Profile] = profile;
        }

        if (authenticated)
        {
            httpContext.User = new System.Security.Claims.ClaimsPrincipal(
                new System.Security.Claims.ClaimsIdentity(
                    [new System.Security.Claims.Claim("sub", "user")],
                    authenticationType: "Test"));
        }

        var actionContext = new ActionContext(
            httpContext,
            new RouteData(),
            new ActionDescriptor());

        return new AuthorizationFilterContext(actionContext, []);
    }

    private static UserAuthProfileDto CreateProfile(
        IReadOnlyList<string>? roles = null,
        IReadOnlyList<string>? permissions = null) =>
        new(
            Guid.NewGuid(),
            "user@example.com",
            "oid",
            1,
            1,
            IsActive: true,
            IsDeleted: false,
            roles ?? [],
            permissions ?? [],
            [],
            []);
}
