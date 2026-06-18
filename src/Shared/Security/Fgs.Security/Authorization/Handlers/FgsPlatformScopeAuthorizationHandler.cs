using Fgs.Security.Authorization.Requirements;
using Fgs.Security.Constants;
using Fgs.Security.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Fgs.Security.Authorization.Handlers;

public sealed class FgsPlatformScopeAuthorizationHandler : AuthorizationHandler<FgsPlatformScopeRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        FgsPlatformScopeRequirement requirement)
    {
        if (context.User.Identity?.IsAuthenticated != true
            || context.Resource is not HttpContext httpContext)
        {
            return Task.CompletedTask;
        }

        var (tenantId, companyId) = FgsRequestAuthContext.ExtractTenantScope(httpContext);
        if (tenantId is long resolvedTenantId
            && companyId is long resolvedCompanyId
            && FgsScopeConstants.IsPlatformScope(resolvedTenantId, resolvedCompanyId))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
