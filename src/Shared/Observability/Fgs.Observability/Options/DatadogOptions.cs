namespace Fgs.Observability.Options;

public sealed class DatadogOptions
{
    public const string SectionName = "Datadog";

    public bool Enabled { get; set; } = true;

    public string? ApiKey { get; set; }

    public string Site { get; set; } = "datadoghq.com";

    public string? AgentHost { get; set; }

    public int AgentPort { get; set; } = 8126;

    public int DogStatsDPort { get; set; } = 8125;

    public string Env { get; set; } = "local";

    public string Version { get; set; } = "0.0.0";

    public bool EnableApm { get; set; } = true;

    public bool EnableRuntimeMetrics { get; set; } = true;

    public string? ServiceName { get; set; }
}
