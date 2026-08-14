namespace Fgs.Observability.Options;

/// <summary>
/// Provider-neutral observability settings (OpenTelemetry traces + metrics).
/// </summary>
public sealed class ObservabilityOptions
{
    public const string SectionName = "Observability";

    public const string MeterName = "Fgs";

    public bool Enabled { get; set; } = true;

    public string? ServiceName { get; set; }

    public string Env { get; set; } = "local";

    public string Version { get; set; } = "0.0.0";

    /// <summary>
    /// OTLP gRPC endpoint, e.g. http://datadog-agent:4317.
    /// When empty, traces/metrics are not exported (in-process instrumentation may still run).
    /// </summary>
    public string? OtlpEndpoint { get; set; }

    public bool EnableTracing { get; set; } = true;

    public bool EnableMetrics { get; set; } = true;

    public bool EnableRuntimeMetrics { get; set; } = true;
}
