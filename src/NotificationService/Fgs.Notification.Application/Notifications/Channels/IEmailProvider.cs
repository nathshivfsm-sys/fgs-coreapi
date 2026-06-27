using Fgs.Notification.Application.Notifications.Channels.Models;

namespace Fgs.Notification.Application.Notifications.Channels;

public interface IEmailProvider
{
    string ProviderName { get; }

    Task<NotificationDispatchResult> SendAsync(
        EmailNotificationMessage message,
        CancellationToken cancellationToken = default);
}

public sealed record EmailNotificationMessage(
    long TenantId,
    string ToAddress,
    string? ToName,
    string Subject,
    string HtmlBody,
    string? PlainTextBody,
    string? CorrelationId);
