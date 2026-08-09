using Fgs.Contracts.Observability;
using Fgs.Observability.Logging;
using Fgs.Observability.Metrics;
using Fgs.Observability.Options;
using Fgs.Observability.Tracing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Fgs.Observability.Extensions;

public static class ObservabilityHostExtensions
{
    /// <summary>
    /// Configures shared Serilog (JSON), Datadog APM, DogStatsD metrics, and health checks.
    /// </summary>
    public static WebApplicationBuilder AddFgsObservability(
        this WebApplicationBuilder builder,
        string? serviceName = null)
    {
        var options = builder.Configuration.GetSection(DatadogOptions.SectionName).Get<DatadogOptions>()
            ?? new DatadogOptions();

        var resolvedServiceName = serviceName
            ?? options.ServiceName
            ?? "fgs-service";

        options.ServiceName = resolvedServiceName;
        builder.Services.Configure<DatadogOptions>(builder.Configuration.GetSection(DatadogOptions.SectionName));
        builder.Services.PostConfigure<DatadogOptions>(o =>
        {
            o.ServiceName ??= resolvedServiceName;
            if (string.IsNullOrWhiteSpace(o.ServiceName))
            {
                o.ServiceName = resolvedServiceName;
            }
        });

        builder.AddFgsSerilog(resolvedServiceName, options);
        DatadogTracing.Configure(resolvedServiceName, options);

        builder.Services.AddHealthChecks();
        builder.Services.TryAddSingleton<IFgsMetrics, NoOpFgsMetrics>();
        if (options.Enabled && !string.IsNullOrWhiteSpace(options.AgentHost))
        {
            builder.Services.RemoveAll<IFgsMetrics>();
            builder.Services.AddSingleton<IFgsMetrics, DogStatsDFgsMetrics>();
        }

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IStartupFilter, DatadogSpanTagStartupFilter>());

        return builder;
    }

    /// <summary>
    /// Legacy DI-only entry point. Prefer <see cref="AddFgsObservability(WebApplicationBuilder, string?)"/>.
    /// </summary>
    public static IServiceCollection AddFgsObservability(
        this IServiceCollection services,
        IConfiguration configuration,
        string? serviceName = null)
    {
        var options = configuration.GetSection(DatadogOptions.SectionName).Get<DatadogOptions>()
            ?? new DatadogOptions();

        var resolvedServiceName = serviceName
            ?? options.ServiceName
            ?? "fgs-service";

        options.ServiceName = resolvedServiceName;
        services.Configure<DatadogOptions>(configuration.GetSection(DatadogOptions.SectionName));
        DatadogTracing.Configure(resolvedServiceName, options);

        services.AddHealthChecks();
        services.TryAddSingleton<IFgsMetrics, NoOpFgsMetrics>();
        if (options.Enabled && !string.IsNullOrWhiteSpace(options.AgentHost))
        {
            services.RemoveAll<IFgsMetrics>();
            services.AddSingleton<IFgsMetrics, DogStatsDFgsMetrics>();
        }

        return services;
    }

    public static IApplicationBuilder UseFgsDatadogSpanTags(this IApplicationBuilder app) =>
        app.UseMiddleware<DatadogSpanTagMiddleware>();
}
