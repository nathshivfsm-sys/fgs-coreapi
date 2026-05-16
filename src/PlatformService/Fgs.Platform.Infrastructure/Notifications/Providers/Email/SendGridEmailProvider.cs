using Fgs.Platform.Application.Integrations.SendGrid;
using Fgs.Platform.Application.Notifications.Channels;
using Fgs.Platform.Application.Notifications.Channels.Models;

namespace Fgs.Platform.Infrastructure.Notifications.Providers.Email;

public sealed class SendGridEmailProvider(ISendGridIntegrationClient sendGrid) : IEmailProvider
{
    public string ProviderName => "SendGrid";

    public Task<NotificationDispatchResult> SendAsync(
        EmailNotificationMessage message,
        CancellationToken cancellationToken = default) =>
        sendGrid.SendEmailAsync(message, cancellationToken);
}
