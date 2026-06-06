namespace Fgs.Messaging.Consumer;

public sealed class ConsumerRouteEntry
{
    public required string RoutingKey { get; init; }

    public required Type MessageType { get; init; }

    public required Func<object, ConsumerMessageContext, object> CreateCommand { get; init; }
}
