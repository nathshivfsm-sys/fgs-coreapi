namespace Fgs.Messaging.Consumer;

public interface IConsumerMessageRouter
{
    bool CanRoute(string routingKey);

    Task RouteAsync(
        string routingKey,
        ReadOnlyMemory<byte> body,
        ConsumerMessageContext context,
        CancellationToken cancellationToken);
}
