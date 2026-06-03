using Fgs.Notification.Application.Integrations.SendGrid;
using Fgs.Notification.Application.Notifications.Channels;
using Fgs.Notification.Application.Notifications.Channels.Models;

namespace Fgs.Notification.Infrastructure.Notifications.Providers.Email;

public sealed class SendGridEmailProvider(ISendGridIntegrationClient sendGrid) : IEmailProvider
{
    public string ProviderName => "SendGrid";

    public Task<NotificationDispatchResult> SendAsync(
        EmailNotificationMessage message,
        CancellationToken cancellationToken = default) =>
        sendGrid.SendEmailAsync(message, cancellationToken);
}
