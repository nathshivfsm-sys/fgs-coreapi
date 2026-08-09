namespace Fgs.Contracts.Observability;

public sealed class NoOpFgsMetrics : IFgsMetrics
{
    public static NoOpFgsMetrics Instance { get; } = new();

    public void Increment(string name, long value = 1, params (string Key, string Value)[] tags)
    {
    }

    public void Gauge(string name, double value, params (string Key, string Value)[] tags)
    {
    }

    public void Histogram(string name, double value, params (string Key, string Value)[] tags)
    {
    }
}
