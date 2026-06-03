namespace Fgs.Observability.Options;

public sealed class ObservabilityOptions
{
    public const string SectionName = "Observability";

    public string ServiceName { get; set; } = "fgs-service";

    public bool EnableOpenTelemetry { get; set; } = true;

    public string? OtlpEndpoint { get; set; }
}
