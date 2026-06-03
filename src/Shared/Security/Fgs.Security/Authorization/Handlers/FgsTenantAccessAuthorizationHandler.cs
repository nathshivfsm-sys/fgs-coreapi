using Fgs.Security.Authorization.Requirements;
using Fgs.Security.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Fgs.Security.Authorization.Handlers;

public sealed class FgsTenantAccessAuthorizationHandler : AuthorizationHandler<FgsTenantAccessRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        FgsTenantAccessRequirement requirement)
    {
        if (context.User.Identity?.IsAuthenticated != true)
        {
            return Task.CompletedTask;
        }

        var tenantClaim = context.User.FindFirst(JwtClaimTypes.TenantId)?.Value;
        var companyClaim = context.User.FindFirst(JwtClaimTypes.CompanyId)?.Value;

        if (!long.TryParse(tenantClaim, out var claimTenantId)
            || !long.TryParse(companyClaim, out var claimCompanyId))
        {
            return Task.CompletedTask;
        }

        if (context.Resource is HttpContext httpContext)
        {
            var headerTenant = httpContext.Request.Headers["X-Tenant-Id"].FirstOrDefault();
            var headerCompany = httpContext.Request.Headers["X-Company-Id"].FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(headerTenant)
                && long.TryParse(headerTenant, out var headerTenantId)
                && headerTenantId != claimTenantId)
            {
                return Task.CompletedTask;
            }

            if (!string.IsNullOrWhiteSpace(headerCompany)
                && long.TryParse(headerCompany, out var headerCompanyId)
                && headerCompanyId != claimCompanyId)
            {
                return Task.CompletedTask;
            }
        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
