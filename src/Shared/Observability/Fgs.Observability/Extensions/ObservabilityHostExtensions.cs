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
    /// Configures shared Serilog (JSON), OpenTelemetry traces/metrics (OTLP), and health checks.
    /// Datadog remains the local/prod log sink and OTLP backend via agent when configured.
    /// </summary>
    public static WebApplicationBuilder AddFgsObservability(
        this WebApplicationBuilder builder,
        string? serviceName = null)
    {
        var (observability, datadog) = ObservabilityOptionsResolver.Resolve(
            builder.Configuration,
            serviceName);

        var resolvedServiceName = observability.ServiceName ?? "fgs-service";

        builder.Services.Configure<ObservabilityOptions>(
            builder.Configuration.GetSection(ObservabilityOptions.SectionName));
        builder.Services.Configure<DatadogOptions>(
            builder.Configuration.GetSection(DatadogOptions.SectionName));
        builder.Services.PostConfigure<ObservabilityOptions>(o => ApplyResolved(o, observability));
        builder.Services.PostConfigure<DatadogOptions>(o =>
        {
            o.ServiceName = resolvedServiceName;
            o.Env = observability.Env;
            o.Version = observability.Version;
            if (string.IsNullOrWhiteSpace(o.ApiKey))
            {
                o.ApiKey = datadog.ApiKey;
            }
        });

        builder.AddFgsSerilog(resolvedServiceName, observability, datadog);
        builder.Services.AddFgsOpenTelemetry(observability);

        builder.Services.AddHealthChecks();
        builder.Services.RemoveAll<IFgsMetrics>();
        if (observability.Enabled && observability.EnableMetrics)
        {
            builder.Services.AddSingleton<IFgsMetrics, OpenTelemetryFgsMetrics>();
        }
        else
        {
            builder.Services.AddSingleton<IFgsMetrics, NoOpFgsMetrics>();
        }

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IStartupFilter, ActivitySpanTagStartupFilter>());

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
        var (observability, _) = ObservabilityOptionsResolver.Resolve(configuration, serviceName);

        services.Configure<ObservabilityOptions>(configuration.GetSection(ObservabilityOptions.SectionName));
        services.Configure<DatadogOptions>(configuration.GetSection(DatadogOptions.SectionName));
        services.PostConfigure<ObservabilityOptions>(o => ApplyResolved(o, observability));
        services.AddFgsOpenTelemetry(observability);

        services.AddHealthChecks();
        services.RemoveAll<IFgsMetrics>();
        if (observability.Enabled && observability.EnableMetrics)
        {
            services.AddSingleton<IFgsMetrics, OpenTelemetryFgsMetrics>();
        }
        else
        {
            services.AddSingleton<IFgsMetrics, NoOpFgsMetrics>();
        }

        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IStartupFilter, ActivitySpanTagStartupFilter>());

        return services;
    }

    public static IApplicationBuilder UseFgsActivitySpanTags(this IApplicationBuilder app) =>
        app.UseMiddleware<ActivitySpanTagMiddleware>();

    /// <summary>Obsolete alias for <see cref="UseFgsActivitySpanTags"/>.</summary>
    [Obsolete("Use UseFgsActivitySpanTags instead.")]
    public static IApplicationBuilder UseFgsDatadogSpanTags(this IApplicationBuilder app) =>
        app.UseFgsActivitySpanTags();

    private static void ApplyResolved(ObservabilityOptions target, ObservabilityOptions resolved)
    {
        target.ServiceName = resolved.ServiceName;
        target.Env = resolved.Env;
        target.Version = resolved.Version;
        target.Enabled = resolved.Enabled;
        target.EnableTracing = resolved.EnableTracing;
        target.EnableMetrics = resolved.EnableMetrics;
        target.EnableRuntimeMetrics = resolved.EnableRuntimeMetrics;
        target.OtlpEndpoint = resolved.OtlpEndpoint;
    }
}
