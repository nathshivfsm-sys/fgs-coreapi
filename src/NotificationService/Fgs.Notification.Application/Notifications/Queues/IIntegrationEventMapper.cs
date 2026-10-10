using Fgs.Notification.Application.Notifications.Channels.Models;

namespace Fgs.Notification.Application.Notifications.Queues;

public interface IIntegrationEventMapper
{
    bool CanMap(string routingKey);

    /// <summary>
    /// Maps a known routing key. Returns null when the payload deserializes to null so callers can fail the request.
    /// </summary>
    NotificationDispatchRequest? Map(string routingKey, string payload, string? correlationId, string messageId);
}
