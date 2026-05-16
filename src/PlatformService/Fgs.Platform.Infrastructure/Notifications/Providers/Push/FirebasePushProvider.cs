using Fgs.Platform.Application.Notifications.Channels;
using Fgs.Platform.Application.Notifications.Channels.Models;
using Microsoft.Extensions.Logging;

namespace Fgs.Platform.Infrastructure.Notifications.Providers.Push;

public sealed class FirebasePushProvider(ILogger<FirebasePushProvider> logger) : IPushProvider
{
    public string ProviderName => "Firebase";

    public Task<NotificationDispatchResult> SendAsync(
        PushNotificationMessage message,
        CancellationToken cancellationToken = default)
    {
        logger.LogWarning(
            "Firebase push provider is a placeholder (TenantId={TenantId}, CorrelationId={CorrelationId}).",
            message.TenantId,
            message.CorrelationId);

        return Task.FromResult(new NotificationDispatchResult(
            false,
            null,
            "Firebase push provider is a placeholder and is not configured."));
    }
}
