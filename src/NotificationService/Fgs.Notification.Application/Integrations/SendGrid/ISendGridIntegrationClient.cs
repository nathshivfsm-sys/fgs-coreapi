using Fgs.Notification.Application.Notifications.Channels;
using Fgs.Notification.Application.Notifications.Channels.Models;

namespace Fgs.Notification.Application.Integrations.SendGrid;

public interface ISendGridIntegrationClient : IIntegrationClient
{
    Task<NotificationDispatchResult> SendEmailAsync(
        EmailNotificationMessage message,
        CancellationToken cancellationToken = default);
}
