namespace Fgs.Contracts.Observability;

/// <summary>
/// Low-cardinality metrics facade. Avoid tags such as UserId, WorkOrderId, or RequestId.
/// </summary>
public interface IFgsMetrics
{
    void Increment(string name, long value = 1, params (string Key, string Value)[] tags);

    void Gauge(string name, double value, params (string Key, string Value)[] tags);

    void Histogram(string name, double value, params (string Key, string Value)[] tags);
}
