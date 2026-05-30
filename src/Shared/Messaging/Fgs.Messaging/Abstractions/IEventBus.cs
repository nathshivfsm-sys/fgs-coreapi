namespace Fgs.Messaging.Abstractions;

public interface IEventBus
{
    Task PublishAsync<TEvent>(
        TEvent integrationEvent,
        string routingKey,
        Guid correlationId,
        CancellationToken cancellationToken = default)
        where TEvent : class;
}
