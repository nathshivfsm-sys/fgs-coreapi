using Fgs.Contracts.Observability;
using Fgs.Observability.Options;
using Microsoft.Extensions.Options;
using StatsdClient;

namespace Fgs.Observability.Metrics;

public sealed class DogStatsDFgsMetrics : IFgsMetrics, IDisposable
{
    private readonly bool _enabled;

    public DogStatsDFgsMetrics(IOptions<DatadogOptions> options)
    {
        var datadog = options.Value;
        _enabled = datadog.Enabled
            && !string.IsNullOrWhiteSpace(datadog.AgentHost);

        if (!_enabled)
        {
            return;
        }

        var dogstatsdConfig = new StatsdConfig
        {
            StatsdServerName = datadog.AgentHost,
            StatsdPort = datadog.DogStatsDPort,
            ConstantTags =
            [
                $"env:{datadog.Env}",
                $"service:{datadog.ServiceName ?? "fgs-service"}",
                $"version:{datadog.Version}"
            ]
        };

        DogStatsd.Configure(dogstatsdConfig);
    }

    public void Increment(string name, long value = 1, params (string Key, string Value)[] tags)
    {
        if (!_enabled)
        {
            return;
        }

        DogStatsd.Counter(name, value, tags: ToTags(tags));
    }

    public void Gauge(string name, double value, params (string Key, string Value)[] tags)
    {
        if (!_enabled)
        {
            return;
        }

        DogStatsd.Gauge(name, value, tags: ToTags(tags));
    }

    public void Histogram(string name, double value, params (string Key, string Value)[] tags)
    {
        if (!_enabled)
        {
            return;
        }

        DogStatsd.Histogram(name, value, tags: ToTags(tags));
    }

    public void Dispose()
    {
        if (_enabled)
        {
            DogStatsd.Dispose();
        }
    }

    private static string[]? ToTags((string Key, string Value)[] tags)
    {
        if (tags.Length == 0)
        {
            return null;
        }

        return tags.Select(t => $"{t.Key}:{t.Value}").ToArray();
    }
}
