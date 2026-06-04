using Fgs.Foundation.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Refit;

namespace Fgs.Foundation.Extensions;

public static class RefitClientExtensions
{
    /// <summary>
    /// Registers a Refit client with Polly-based retry, timeout, and circuit breaker.
    /// </summary>
    public static IServiceCollection AddFgsRefitClient<TClient>(
        this IServiceCollection services,
        IConfiguration configuration,
        string baseUrlConfigurationKey,
        string? defaultBaseUrl = null)
        where TClient : class
    {
        var baseUrl = configuration[baseUrlConfigurationKey] ?? defaultBaseUrl
            ?? throw new InvalidOperationException(
                $"Configuration key '{baseUrlConfigurationKey}' (or defaultBaseUrl) is required for Refit client {typeof(TClient).Name}.");

        services.Configure<HttpResilienceOptions>(configuration.GetSection(HttpResilienceOptions.SectionName));

        services
            .AddRefitClient<TClient>()
            .ConfigureHttpClient(client => client.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/"))
            .AddStandardResilienceHandler();

        return services;
    }

    public static IServiceCollection AddFgsRefitClient<TClient>(
        this IServiceCollection services,
        Uri baseAddress)
        where TClient : class
    {
        services.Configure<HttpResilienceOptions>(_ => { });

        services
            .AddRefitClient<TClient>()
            .ConfigureHttpClient(client => client.BaseAddress = baseAddress)
            .AddStandardResilienceHandler();

        return services;
    }
}
