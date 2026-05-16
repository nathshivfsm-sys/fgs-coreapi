using Fgs.Platform.Application.Notifications.Channels.Models;

namespace Fgs.Platform.Application.Notifications.Channels;

public interface IEmailProvider
{
    string ProviderName { get; }

    Task<NotificationDispatchResult> SendAsync(
        EmailNotificationMessage message,
        CancellationToken cancellationToken = default);
}

public sealed record EmailNotificationMessage(
    Guid TenantId,
    string ToAddress,
    string? ToName,
    string Subject,
    string HtmlBody,
    string? PlainTextBody,
    string? CorrelationId);
