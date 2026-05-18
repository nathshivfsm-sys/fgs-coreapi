using Fgs.Platform.Application.Notifications.Channels.Models;

namespace Fgs.Platform.Application.Notifications.Channels;

public interface INotificationDispatcher
{
    Task<NotificationDispatchResult> DispatchAsync(
        NotificationDispatchRequest request,
        CancellationToken cancellationToken = default);
}
