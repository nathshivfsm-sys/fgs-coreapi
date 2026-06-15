using Fgs.Security.Authorization.Requirements;
using Fgs.Security.Services;
using Microsoft.AspNetCore.Authorization;

namespace Fgs.Security.Authorization.Handlers;

public sealed class FgsDbValidatedUserAuthorizationHandler
    : AuthorizationHandler<FgsDbValidatedUserRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        FgsDbValidatedUserRequirement requirement)
    {
        if (context.User.Identity?.IsAuthenticated == true
            && FgsClaimsEnrichment.IsEnriched(context.User))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
