using Fgs.Security.Abstractions;
using Fgs.Security.Authorization.Requirements;
using Fgs.Security.Services;
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
        if (context.User.Identity?.IsAuthenticated != true
            || context.Resource is not HttpContext httpContext)
        {
            return;
        }

        var (tenantId, companyId) = FgsRequestAuthContext.ExtractTenantScope(httpContext);
        if (tenantId is not long resolvedTenantId || companyId is not long resolvedCompanyId)
        {
            return;
        }

        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var scopeValidator = scope.ServiceProvider.GetRequiredService<IFgsTenantScopeValidator>();
        if (!await scopeValidator.IsValidScopeAsync(
                resolvedTenantId,
                resolvedCompanyId,
                httpContext.RequestAborted))
        {
            return;
        }

        context.Succeed(requirement);
    }
}
