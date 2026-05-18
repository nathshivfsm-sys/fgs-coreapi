namespace Fgs.User.Infrastructure.Messaging;

public interface IRabbitMqPublisher
{
    Task PublishAsync(
        string routingKey,
        string payload,
        string? correlationId,
        CancellationToken cancellationToken = default);
}
