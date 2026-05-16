using Fgs.Platform.Application.Notifications.Channels.Models;

namespace Fgs.Platform.Application.Notifications.Queues;

public interface IIntegrationEventMapper
{
    bool CanMap(string routingKey);

    NotificationDispatchRequest? Map(string routingKey, string payload, string? correlationId, string messageId);
}
