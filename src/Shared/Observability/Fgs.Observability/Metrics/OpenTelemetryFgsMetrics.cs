using System.Collections.Concurrent;
using System.Diagnostics.Metrics;
using Fgs.Contracts.Observability;
using Fgs.Observability.Options;

namespace Fgs.Observability.Metrics;

/// <summary>
/// <see cref="IFgsMetrics"/> backed by an OpenTelemetry <see cref="Meter"/>.
/// </summary>
public sealed class OpenTelemetryFgsMetrics : IFgsMetrics
{
    private readonly Meter _meter;
    private readonly ConcurrentDictionary<string, Counter<long>> _counters = new(StringComparer.Ordinal);
    private readonly ConcurrentDictionary<string, Gauge<double>> _gauges = new(StringComparer.Ordinal);
    private readonly ConcurrentDictionary<string, Histogram<double>> _histograms = new(StringComparer.Ordinal);

    public OpenTelemetryFgsMetrics()
    {
        _meter = new Meter(ObservabilityOptions.MeterName);
    }

    public void Increment(string name, long value = 1, params (string Key, string Value)[] tags)
    {
        var counter = _counters.GetOrAdd(name, static (n, meter) => meter.CreateCounter<long>(n), _meter);
        counter.Add(value, ToTags(tags));
    }

    public void Gauge(string name, double value, params (string Key, string Value)[] tags)
    {
        var gauge = _gauges.GetOrAdd(name, static (n, meter) => meter.CreateGauge<double>(n), _meter);
        gauge.Record(value, ToTags(tags));
    }

    public void Histogram(string name, double value, params (string Key, string Value)[] tags)
    {
        var histogram = _histograms.GetOrAdd(name, static (n, meter) => meter.CreateHistogram<double>(n), _meter);
        histogram.Record(value, ToTags(tags));
    }

    private static KeyValuePair<string, object?>[] ToTags((string Key, string Value)[] tags)
    {
        if (tags.Length == 0)
        {
            return [];
        }

        var result = new KeyValuePair<string, object?>[tags.Length];
        for (var i = 0; i < tags.Length; i++)
        {
            result[i] = new KeyValuePair<string, object?>(tags[i].Key, tags[i].Value);
        }

        return result;
    }
}
