using Microsoft.Extensions.Configuration;

namespace Fgs.Observability.Options;

internal static class ObservabilityOptionsResolver
{
    public static (ObservabilityOptions Observability, DatadogOptions Datadog) Resolve(
        IConfiguration configuration,
        string? serviceName)
    {
        var observabilitySection = configuration.GetSection(ObservabilityOptions.SectionName);
        var datadogSection = configuration.GetSection(DatadogOptions.SectionName);

        var observability = observabilitySection.Get<ObservabilityOptions>() ?? new ObservabilityOptions();
        var datadog = datadogSection.Get<DatadogOptions>() ?? new DatadogOptions();

        // Datadog section remains the primary local config; Observability overrides when set.
        if (observabilitySection["Enabled"] is null)
        {
            observability.Enabled = datadog.Enabled;
        }

        if (observabilitySection["Env"] is null && !string.IsNullOrWhiteSpace(datadog.Env))
        {
            observability.Env = datadog.Env;
        }

        if (observabilitySection["Version"] is null && !string.IsNullOrWhiteSpace(datadog.Version))
        {
            observability.Version = datadog.Version;
        }

        if (observabilitySection["EnableTracing"] is null)
        {
            observability.EnableTracing = datadog.EnableApm;
        }

        if (observabilitySection["EnableRuntimeMetrics"] is null)
        {
            observability.EnableRuntimeMetrics = datadog.EnableRuntimeMetrics;
        }

        if (string.IsNullOrWhiteSpace(observability.OtlpEndpoint)
            && !string.IsNullOrWhiteSpace(datadog.AgentHost))
        {
            observability.OtlpEndpoint = $"http://{datadog.AgentHost.Trim().TrimEnd('/')}:4317";
        }

        var resolvedServiceName = serviceName
            ?? observability.ServiceName
            ?? datadog.ServiceName
            ?? "fgs-service";

        observability.ServiceName = resolvedServiceName;
        datadog.ServiceName = resolvedServiceName;
        datadog.Env = observability.Env;
        datadog.Version = observability.Version;

        return (observability, datadog);
    }
}
