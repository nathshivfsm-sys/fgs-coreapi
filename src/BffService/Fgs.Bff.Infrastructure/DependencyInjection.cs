using Fgs.Contracts.Clients;
using Fgs.Credentials.Extensions;
using Fgs.Credentials.Http;
using Fgs.Foundation.Caching.Extensions;
using Fgs.Foundation.Caching.Options;
using Fgs.Foundation.Extensions;
using Fgs.Security.Extensions;
using Fgs.Security.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Fgs.Bff.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsBffInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddFgsCredentialConsumer(
            configuration,
            configuration,
            options =>
            {
                options.ServiceName = "fgs-bff-service";
                options.RequiredProviders = ["ENTRA_EXTERNAL_ID", "REDIS"];
                // Register ISetupClient once below with correlation + internal service key handlers.
                options.RegisterSetupClient = false;
            },
            typeof(EntraExternalIdAuthOptions),
            typeof(RedisCacheOptions));

        services.AddFgsEntraAuthentication(configuration);
        services.AddFgsActiveUserValidation(configuration);
        services.AddFgsUserAuthProfileClient(configuration);

        services.TryAddTransient<InternalServiceKeyDelegatingHandler>();

        // CorrelationIdPropagationHandler is attached by AddFgsRefitClient for all outbound calls.
        services.AddFgsRefitClient<IUserSignupClient>(
            configuration,
            "UserService:BaseUrl",
            "http://user-service:5001");

        services.AddFgsInternalServiceRefitClient<IUserTenantClient>(
            configuration,
            "UserService:BaseUrl",
            "http://user-service:5001");

        services.AddFgsRefitClient<ISetupClient>(
            configuration,
            "SetupService:BaseUrl",
            "http://setup-service:5004",
            builder => builder.AddHttpMessageHandler<InternalServiceKeyDelegatingHandler>());

        services.AddFgsRedisCache(configuration);
        return services;
    }
}
