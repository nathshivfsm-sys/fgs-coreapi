using Fgs.Platform.Application.Notifications.Preferences;

namespace Fgs.Platform.Infrastructure.Notifications.Preferences;

public sealed class PlaceholderNotificationPreferenceService : INotificationPreferenceService
{
    public Task<bool> IsChannelEnabledAsync(
        Guid tenantId,
        Guid userId,
        string channel,
        CancellationToken cancellationToken = default) =>
        Task.FromResult(true);
}
