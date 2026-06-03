using Fgs.Notification.Application.Notifications.Channels;
using Fgs.Notification.Application.Notifications.Channels.Models;
using Microsoft.Extensions.Logging;

namespace Fgs.Notification.Infrastructure.Notifications.Providers.Email;

public sealed class SmtpEmailProvider(ILogger<SmtpEmailProvider> logger) : IEmailProvider
{
    public string ProviderName => "Smtp";

    public Task<NotificationDispatchResult> SendAsync(
        EmailNotificationMessage message,
        CancellationToken cancellationToken = default)
    {
        logger.LogWarning(
            "SMTP provider is not configured; email not sent (TenantId={TenantId}, To={To}, CorrelationId={CorrelationId}).",
            message.TenantId,
            message.ToAddress,
            message.CorrelationId);

        return Task.FromResult(new NotificationDispatchResult(
            false,
            null,
            "SMTP provider is a placeholder and is not configured."));
    }
}
