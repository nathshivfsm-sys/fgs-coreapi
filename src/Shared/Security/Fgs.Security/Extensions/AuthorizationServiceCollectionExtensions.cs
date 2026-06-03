using Fgs.Security.Authorization;
using Fgs.Security.Authorization.Handlers;
using Fgs.Security.Authorization.Requirements;
using Fgs.Security.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Security.Extensions;

public static class AuthorizationServiceCollectionExtensions
{
    public static IServiceCollection AddFgsAuthorization(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, FgsRoleAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, FgsTenantAccessAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, FgsPlatformScopeAuthorizationHandler>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy(FgsAuthorizationPolicies.RequireTenantAdmin, policy =>
            {
                policy.AddRequirements(
                    new FgsRoleRequirement(FgsRoleCodes.TenantAdmin),
                    new FgsTenantAccessRequirement());
            });

            options.AddPolicy(FgsAuthorizationPolicies.RequirePlatformAdmin, policy =>
            {
                policy.AddRequirements(
                    new FgsRoleRequirement(FgsRoleCodes.PlatformAdmin),
                    new FgsPlatformScopeRequirement());
            });

            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        return services;
    }
}
