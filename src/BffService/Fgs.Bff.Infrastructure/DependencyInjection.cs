using Fgs.Bff.Application.Features.Lookups;
using Fgs.Bff.Application.Features.Lookups.Abstractions;
using Fgs.Bff.Infrastructure.Http;
using Fgs.Bff.Infrastructure.Lookups;
using Fgs.Contracts.Clients;
using Fgs.Contracts.Options;
using Fgs.Credentials.Extensions;
using Fgs.Credentials.Http;
using Fgs.Foundation.Caching.Extensions;
using Fgs.Foundation.Caching.Options;
using Fgs.Foundation.Extensions;
using Fgs.Foundation.Http;
using Fgs.Security.Extensions;
using Fgs.Security.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http.Resilience;

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
        services.AddFgsAuthorization();
        services.AddFgsActiveUserValidation(configuration);
        services.AddFgsUserAuthProfileClient(configuration);

        services.TryAddTransient<InternalServiceKeyDelegatingHandler>();
        services.TryAddTransient<CallerContextPropagationHandler>();
        services.TryAddTransient<CorrelationIdPropagationHandler>();
        services.AddHttpContextAccessor();

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

        RegisterLookupHttpClients(services, configuration);
        services.AddSingleton<ILookupGateway, LookupGateway>();

        services.AddFgsRedisCache(configuration);
        return services;
    }

    private static void RegisterLookupHttpClients(
        IServiceCollection services,
        IConfiguration configuration)
    {
        var resilience = configuration.GetSection(HttpResilienceOptions.SectionName).Get<HttpResilienceOptions>()
            ?? new HttpResilienceOptions();

        AddLookupClient(
            services,
            LookupServiceNames.Setup,
            configuration["SetupService:BaseUrl"] ?? "http://setup-service:5004",
            resilience);

        AddLookupClient(
            services,
            LookupServiceNames.User,
            configuration["UserService:BaseUrl"] ?? "http://user-service:5001",
            resilience);

        AddLookupClient(
            services,
            LookupServiceNames.Asset,
            configuration["AssetService:BaseUrl"] ?? "http://asset-service:5015",
            resilience);

        AddLookupClient(
            services,
            LookupServiceNames.Inventory,
            configuration["InventoryService:BaseUrl"] ?? "http://inventory-service:5012",
            resilience);

        AddLookupClient(
            services,
            LookupServiceNames.Billing,
            configuration["BillingService:BaseUrl"] ?? "http://billing-service:5011",
            resilience);

        AddLookupClient(
            services,
            LookupServiceNames.Crm,
            configuration["CrmService:BaseUrl"] ?? "http://crm-service:5009",
            resilience);
    }

    private static void AddLookupClient(
        IServiceCollection services,
        string name,
        string baseUrl,
        HttpResilienceOptions resilience)
    {
        var builder = services
            .AddHttpClient(name, client =>
            {
                client.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/");
            })
            .AddHttpMessageHandler<CorrelationIdPropagationHandler>()
            .AddHttpMessageHandler<CallerContextPropagationHandler>();

        builder.AddStandardResilienceHandler(options =>
        {
            var attemptTimeout = TimeSpan.FromSeconds(resilience.AttemptTimeoutSeconds);
            var minimumSamplingDuration = TimeSpan.FromTicks(attemptTimeout.Ticks * 2);
            var samplingDuration = TimeSpan.FromSeconds(resilience.CircuitBreakerSamplingDurationSeconds);
            if (samplingDuration < minimumSamplingDuration)
            {
                samplingDuration = minimumSamplingDuration;
            }

            options.Retry.MaxRetryAttempts = resilience.MaxRetryAttempts;
            options.Retry.Delay = TimeSpan.FromSeconds(resilience.RetryDelaySeconds);
            options.AttemptTimeout.Timeout = attemptTimeout;
            options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(resilience.TotalRequestTimeoutSeconds);
            options.CircuitBreaker.FailureRatio = resilience.CircuitBreakerFailureRatio / 100.0;
            options.CircuitBreaker.MinimumThroughput = resilience.CircuitBreakerMinimumThroughput;
            options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(resilience.CircuitBreakerBreakDurationSeconds);
            options.CircuitBreaker.SamplingDuration = samplingDuration;
        });
    }
}
