using System.Security.Claims;
using Fgs.Security.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Fgs.Security.Authorization.Handlers;

public sealed class FgsRoleAuthorizationHandler : AuthorizationHandler<FgsRoleRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        FgsRoleRequirement requirement)
    {
        if (context.User.Identity?.IsAuthenticated != true)
        {
            return Task.CompletedTask;
        }

        var userRoles = context.User
            .FindAll(ClaimTypes.Role)
            .Select(c => c.Value)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var satisfied = requirement.RequireAll
            ? requirement.RoleCodes.All(userRoles.Contains)
            : requirement.RoleCodes.Any(userRoles.Contains);

        if (satisfied)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
