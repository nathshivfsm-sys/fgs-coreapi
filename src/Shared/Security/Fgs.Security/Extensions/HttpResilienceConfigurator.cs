using Fgs.Contracts.Options;
using Microsoft.Extensions.Http.Resilience;

namespace Fgs.Security.Extensions;

internal static class HttpResilienceConfigurator
{
    public static void Configure(
        HttpStandardResilienceOptions options,
        HttpResilienceOptions resilience)
    {
        var attemptTimeout = TimeSpan.FromSeconds(resilience.AttemptTimeoutSeconds);
        var minimumSamplingDuration = TimeSpan.FromTicks(attemptTimeout.Ticks * 2);
        var samplingDuration = TimeSpan.FromSeconds(resilience.CircuitBreakerSamplingDurationSeconds);
        if (samplingDuration < minimumSamplingDuration)
        {
            samplingDuration = minimumSamplingDuration;
        }

        options.Retry.MaxRetryAttempts = resilience.MaxRetryAttempts;
        options.Retry.Delay = TimeSpan.FromSeconds(resilience.RetryDelaySeconds);
        options.AttemptTimeout.Timeout = attemptTimeout;
        options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(resilience.TotalRequestTimeoutSeconds);
        options.CircuitBreaker.FailureRatio = resilience.CircuitBreakerFailureRatio / 100.0;
        options.CircuitBreaker.MinimumThroughput = resilience.CircuitBreakerMinimumThroughput;
        options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(resilience.CircuitBreakerBreakDurationSeconds);
        options.CircuitBreaker.SamplingDuration = samplingDuration;
    }
}
