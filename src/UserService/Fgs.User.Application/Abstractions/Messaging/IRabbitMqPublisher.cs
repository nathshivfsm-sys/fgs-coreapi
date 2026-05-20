namespace Fgs.User.Application.Abstractions.Messaging;

public interface IRabbitMqPublisher
{
    Task PublishAsync(
        string routingKey,
        string payload,
        string? correlationId,
        CancellationToken cancellationToken = default);
}
