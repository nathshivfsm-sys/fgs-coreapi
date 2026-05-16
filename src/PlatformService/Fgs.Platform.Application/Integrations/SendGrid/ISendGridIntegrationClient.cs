using Fgs.Platform.Application.Notifications.Channels;
using Fgs.Platform.Application.Notifications.Channels.Models;

namespace Fgs.Platform.Application.Integrations.SendGrid;

public interface ISendGridIntegrationClient : IIntegrationClient
{
    Task<NotificationDispatchResult> SendEmailAsync(
        EmailNotificationMessage message,
        CancellationToken cancellationToken = default);
}
