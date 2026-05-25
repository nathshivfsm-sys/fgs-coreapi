using Fgs.Platform.Application.Notifications.Channels.Models;

namespace Fgs.Platform.Application.Notifications.Channels;

public interface ISmsProvider
{
    string ProviderName { get; }

    Task<NotificationDispatchResult> SendAsync(
        SmsNotificationMessage message,
        CancellationToken cancellationToken = default);
}

public sealed record SmsNotificationMessage(
    long TenantId,
    string PhoneNumber,
    string Body,
    string? CorrelationId);
