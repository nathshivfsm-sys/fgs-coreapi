using Fgs.Security.Abstractions;
using Fgs.Security.Authorization.Requirements;
using Fgs.Security.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Security.Authorization.Handlers;

public sealed class FgsDbValidatedUserAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    : AuthorizationHandler<FgsDbValidatedUserRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        FgsDbValidatedUserRequirement requirement)
    {
        if (context.User.Identity?.IsAuthenticated != true
            || !FgsClaimsEnrichment.IsEnriched(context.User))
        {
            return;
        }

        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var statusValidator = scope.ServiceProvider.GetRequiredService<IFgsUserStatusValidator>();
        var cancellationToken = context.Resource is Microsoft.AspNetCore.Http.HttpContext httpContext
            ? httpContext.RequestAborted
            : CancellationToken.None;

        if (await statusValidator.IsActiveAsync(cancellationToken))
        {
            context.Succeed(requirement);
        }
    }
}
