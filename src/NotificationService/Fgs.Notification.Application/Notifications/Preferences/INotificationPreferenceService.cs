namespace Fgs.Notification.Application.Notifications.Preferences;

public interface INotificationPreferenceService
{
    Task<bool> IsChannelEnabledAsync(
        Guid tenantId,
        Guid userId,
        string channel,
        CancellationToken cancellationToken = default);
}
