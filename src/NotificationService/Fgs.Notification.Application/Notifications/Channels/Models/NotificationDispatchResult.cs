namespace Fgs.Notification.Application.Notifications.Channels.Models;

public sealed record NotificationDispatchResult(
    bool Success,
    string? ProviderMessageId,
    string? Error);
