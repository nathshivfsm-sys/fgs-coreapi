using Fgs.Observability.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Fgs.Observability.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFgsObservability(
        this IServiceCollection services,
        IConfiguration configuration,
        string? serviceName = null)
    {
        services.AddHealthChecks();

        var options = configuration.GetSection(ObservabilityOptions.SectionName).Get<ObservabilityOptions>()
            ?? new ObservabilityOptions();

        var resolvedServiceName = serviceName
            ?? options.ServiceName
            ?? "fgs-service";

        if (!options.EnableOpenTelemetry)
        {
            return services;
        }

        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(resolvedServiceName))
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();

                if (!string.IsNullOrWhiteSpace(options.OtlpEndpoint))
                {
                    tracing.AddOtlpExporter(exporter => exporter.Endpoint = new Uri(options.OtlpEndpoint));
                }
            });

        return services;
    }
}
