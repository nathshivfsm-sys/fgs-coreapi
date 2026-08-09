using Datadog.Trace;
using Datadog.Trace.Configuration;
using Fgs.Observability.Options;

namespace Fgs.Observability.Tracing;

public static class DatadogTracing
{
    public static void Configure(string serviceName, DatadogOptions options)
    {
        if (!options.Enabled || !options.EnableApm || string.IsNullOrWhiteSpace(options.AgentHost))
        {
            return;
        }

        Environment.SetEnvironmentVariable("DD_SERVICE", serviceName);
        Environment.SetEnvironmentVariable("DD_ENV", options.Env);
        Environment.SetEnvironmentVariable("DD_VERSION", options.Version);
        Environment.SetEnvironmentVariable("DD_SITE", options.Site);
        Environment.SetEnvironmentVariable("DD_AGENT_HOST", options.AgentHost);
        Environment.SetEnvironmentVariable("DD_TRACE_AGENT_PORT", options.AgentPort.ToString());
        Environment.SetEnvironmentVariable("DD_LOGS_INJECTION", "true");
        Environment.SetEnvironmentVariable(
            "DD_RUNTIME_METRICS_ENABLED",
            options.EnableRuntimeMetrics ? "true" : "false");
        Environment.SetEnvironmentVariable("DD_TRACE_PROPAGATION_STYLE", "datadog,tracecontext");

        var settings = TracerSettings.FromDefaultSources();
        settings.ServiceName = serviceName;
        settings.Environment = options.Env;
        settings.ServiceVersion = options.Version;
        settings.AgentUri = new Uri($"http://{options.AgentHost}:{options.AgentPort}");
        settings.LogsInjectionEnabled = true;
        settings.TracerMetricsEnabled = options.EnableRuntimeMetrics;

        Tracer.Configure(settings);
    }

    public static void TagActiveSpan(string key, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        Tracer.Instance?.ActiveScope?.Span.SetTag(key, value);
    }
}
