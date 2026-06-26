using Fgs.Notification.Application.Notifications.Channels.Models;

namespace Fgs.Notification.Application.Notifications.Channels;

public interface IPushProvider
{
    string ProviderName { get; }

    Task<NotificationDispatchResult> SendAsync(
        PushNotificationMessage message,
        CancellationToken cancellationToken = default);
}

public sealed record PushNotificationMessage(
    long TenantId,
    string DeviceToken,
    string Title,
    string Body,
    IReadOnlyDictionary<string, string>? Data,
    string? CorrelationId);
