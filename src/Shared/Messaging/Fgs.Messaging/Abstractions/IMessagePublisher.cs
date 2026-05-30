namespace Fgs.Messaging.Abstractions;

public interface IMessagePublisher
{
    Task PublishAsync(
        string exchangeName,
        string routingKey,
        ReadOnlyMemory<byte> body,
        IDictionary<string, object?>? headers = null,
        CancellationToken cancellationToken = default);
}
