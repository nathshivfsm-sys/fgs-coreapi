namespace Fgs.Messaging.Consumer;

public sealed class ConsumerMessageContext
{
    public required string RoutingKey { get; init; }

    public required string MessageId { get; init; }

    public string? CorrelationId { get; init; }

    public int RetryCount { get; init; }

    public required string RawBody { get; init; }

    public IReadOnlyDictionary<string, object?> Headers { get; init; } =
        new Dictionary<string, object?>(StringComparer.Ordinal);
}
