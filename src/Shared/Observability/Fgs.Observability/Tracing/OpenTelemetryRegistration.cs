using Fgs.Observability.Options;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Fgs.Observability.Tracing;

internal static class OpenTelemetryRegistration
{
    public static IServiceCollection AddFgsOpenTelemetry(
        this IServiceCollection services,
        ObservabilityOptions options)
    {
        if (!options.Enabled)
        {
            return services;
        }

        var serviceName = options.ServiceName ?? "fgs-service";
        var otlpEndpoint = TryCreateOtlpUri(options.OtlpEndpoint);

        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource
                .AddService(
                    serviceName: serviceName,
                    serviceVersion: options.Version)
                .AddAttributes(
                [
                    new KeyValuePair<string, object>("deployment.environment", options.Env)
                ]))
            .WithTracing(tracing =>
            {
                if (!options.EnableTracing)
                {
                    return;
                }

                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSource(serviceName);

                if (otlpEndpoint is not null)
                {
                    tracing.AddOtlpExporter(exporter =>
                    {
                        exporter.Endpoint = otlpEndpoint;
                    });
                }
            })
            .WithMetrics(metrics =>
            {
                if (!options.EnableMetrics)
                {
                    return;
                }

                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddMeter(ObservabilityOptions.MeterName);

                if (options.EnableRuntimeMetrics)
                {
                    metrics.AddRuntimeInstrumentation();
                }

                if (otlpEndpoint is not null)
                {
                    metrics.AddOtlpExporter(exporter =>
                    {
                        exporter.Endpoint = otlpEndpoint;
                    });
                }
            });

        return services;
    }

    private static Uri? TryCreateOtlpUri(string? endpoint)
    {
        if (string.IsNullOrWhiteSpace(endpoint))
        {
            return null;
        }

        return Uri.TryCreate(endpoint, UriKind.Absolute, out var uri) ? uri : null;
    }
}
