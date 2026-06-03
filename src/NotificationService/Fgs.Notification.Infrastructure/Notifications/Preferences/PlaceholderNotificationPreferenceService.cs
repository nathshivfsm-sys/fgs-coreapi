using Fgs.Notification.Application.Notifications.Preferences;

namespace Fgs.Notification.Infrastructure.Notifications.Preferences;

public sealed class PlaceholderNotificationPreferenceService : INotificationPreferenceService
{
    public Task<bool> IsChannelEnabledAsync(
        Guid tenantId,
        Guid userId,
        string channel,
        CancellationToken cancellationToken = default) =>
        Task.FromResult(true);
}
