namespace Fgs.Observability.Options;

/// <summary>
/// Datadog-specific settings used for Serilog log shipping and as a legacy alias
/// for OpenTelemetry OTLP endpoint resolution (<see cref="AgentHost"/>).
/// </summary>
public sealed class DatadogOptions
{
    public const string SectionName = "Datadog";

    public bool Enabled { get; set; } = true;

    public string? ApiKey { get; set; }

    public string Site { get; set; } = "datadoghq.com";

    /// <summary>
    /// Legacy APM/agent host. When <see cref="ObservabilityOptions.OtlpEndpoint"/> is empty,
    /// resolved to http://{AgentHost}:4317 for OTLP.
    /// </summary>
    public string? AgentHost { get; set; }

    public int AgentPort { get; set; } = 8126;

    public int DogStatsDPort { get; set; } = 8125;

    public string Env { get; set; } = "local";

    public string Version { get; set; } = "0.0.0";

    public bool EnableApm { get; set; } = true;

    public bool EnableRuntimeMetrics { get; set; } = true;

    /// <summary>
    /// Datadog LLM Observability / AI. Always forced off via <c>DD_LLMOBS_ENABLED=false</c>; keep false.
    /// </summary>
    public bool EnableLlmObs { get; set; }

    public string? ServiceName { get; set; }
}
