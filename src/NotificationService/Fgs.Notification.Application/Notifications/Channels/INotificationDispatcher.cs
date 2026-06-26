using Fgs.Notification.Application.Notifications.Channels.Models;

namespace Fgs.Notification.Application.Notifications.Channels;

public interface INotificationDispatcher
{
    Task<NotificationDispatchResult> DispatchAsync(
        NotificationDispatchRequest request,
        CancellationToken cancellationToken = default);
}
