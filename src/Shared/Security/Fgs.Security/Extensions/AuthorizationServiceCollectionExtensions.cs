using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Security.Extensions;

public static class AuthorizationServiceCollectionExtensions
{
    public static IServiceCollection AddFgsAuthorization(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var disableTokenValidation = FgsAuthenticationDisable.IsEnabled(configuration);

        services.AddAuthorization(options =>
        {
            if (disableTokenValidation)
            {
                // No fallback policy — endpoints are anonymous unless they have [Authorize].
                options.FallbackPolicy = null;
                return;
            }

            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        return services;
    }
}
