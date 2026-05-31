using Fgs.Security.Authorization.Requirements;
using Fgs.Security.Constants;
using Microsoft.AspNetCore.Authorization;

namespace Fgs.Security.Authorization.Handlers;

public sealed class FgsPlatformScopeAuthorizationHandler : AuthorizationHandler<FgsPlatformScopeRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        FgsPlatformScopeRequirement requirement)
    {
        if (context.User.Identity?.IsAuthenticated != true)
        {
            return Task.CompletedTask;
        }

        var tenantClaim = context.User.FindFirst(JwtClaimTypes.TenantId)?.Value;
        var companyClaim = context.User.FindFirst(JwtClaimTypes.CompanyId)?.Value;

        if (long.TryParse(tenantClaim, out var tenantId)
            && long.TryParse(companyClaim, out var companyId)
            && FgsScopeConstants.IsPlatformScope(tenantId, companyId))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
