using Fgs.Security.Abstractions;
using Fgs.Security.Authorization.Requirements;
using Fgs.Security.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Security.Authorization.Handlers;

public sealed class FgsTenantAccessAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    : AuthorizationHandler<FgsTenantAccessRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        FgsTenantAccessRequirement requirement)
    {
        if (context.User.Identity?.IsAuthenticated != true)
        {
            return;
        }

        var tenantClaim = context.User.FindFirst(JwtClaimTypes.TenantId)?.Value;
        var companyClaim = context.User.FindFirst(JwtClaimTypes.CompanyId)?.Value;

        if (!long.TryParse(tenantClaim, out var claimTenantId)
            || !long.TryParse(companyClaim, out var claimCompanyId))
        {
            return;
        }

        if (context.Resource is HttpContext httpContext)
        {
            var headerTenant = httpContext.Request.Headers["X-Tenant-Id"].FirstOrDefault();
            var headerCompany = httpContext.Request.Headers["X-Company-Id"].FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(headerTenant)
                && long.TryParse(headerTenant, out var headerTenantId)
                && headerTenantId != claimTenantId)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(headerCompany)
                && long.TryParse(headerCompany, out var headerCompanyId)
                && headerCompanyId != claimCompanyId)
            {
                return;
            }
        }

        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var scopeValidator = scope.ServiceProvider.GetRequiredService<IFgsTenantScopeValidator>();
        var cancellationToken = context.Resource is HttpContext ctx
            ? ctx.RequestAborted
            : CancellationToken.None;

        if (!await scopeValidator.IsValidScopeAsync(claimTenantId, claimCompanyId, cancellationToken))
        {
            return;
        }

        context.Succeed(requirement);
    }
}
