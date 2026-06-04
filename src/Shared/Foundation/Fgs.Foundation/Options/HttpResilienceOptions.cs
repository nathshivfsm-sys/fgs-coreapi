namespace Fgs.Foundation.Options;

public sealed class HttpResilienceOptions
{
    public const string SectionName = "Resilience:Http";

    public int MaxRetryAttempts { get; set; } = 5;

    public int RetryDelaySeconds { get; set; } = 2;

    public int AttemptTimeoutSeconds { get; set; } = 30;

    public int TotalRequestTimeoutSeconds { get; set; } = 60;

    public int CircuitBreakerFailureRatio { get; set; } = 50;

    public int CircuitBreakerMinimumThroughput { get; set; } = 10;

    public int CircuitBreakerBreakDurationSeconds { get; set; } = 30;

    public int CircuitBreakerSamplingDurationSeconds { get; set; } = 30;
}
