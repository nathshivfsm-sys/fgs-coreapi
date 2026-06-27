using Fgs.Contracts.Options;
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
        string? defaultBaseUrl = null,
        Action<IHttpClientBuilder>? configureBuilder = null)
        where TClient : class
    {
        var baseUrl = configuration[baseUrlConfigurationKey] ?? defaultBaseUrl
            ?? throw new InvalidOperationException(
                $"Configuration key '{baseUrlConfigurationKey}' (or defaultBaseUrl) is required for Refit client {typeof(TClient).Name}.");

        var resilience = configuration.GetSection(HttpResilienceOptions.SectionName).Get<HttpResilienceOptions>()
            ?? new HttpResilienceOptions();

        var builder = services
            .AddRefitClient<TClient>()
            .ConfigureHttpClient(client => client.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/"));

        configureBuilder?.Invoke(builder);

        builder.AddStandardResilienceHandler(options => ConfigureResilience(options, resilience));

        return services;
    }

    public static IServiceCollection AddFgsRefitClient<TClient>(
        this IServiceCollection services,
        Uri baseAddress,
        Action<IHttpClientBuilder>? configureBuilder = null)
        where TClient : class
    {
        var resilience = new HttpResilienceOptions();

        var builder = services
            .AddRefitClient<TClient>()
            .ConfigureHttpClient(client => client.BaseAddress = baseAddress);

        configureBuilder?.Invoke(builder);

        builder.AddStandardResilienceHandler(options => ConfigureResilience(options, resilience));

        return services;
    }

    private static void ConfigureResilience(
        HttpStandardResilienceOptions options,
        HttpResilienceOptions resilience)
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
    }
}
