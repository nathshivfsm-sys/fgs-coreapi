using Fgs.Platform.Application.Notifications.Channels;
using Fgs.Platform.Application.Notifications.Channels.Models;
using Microsoft.Extensions.Logging;

namespace Fgs.Platform.Infrastructure.Notifications.Providers.Sms;

public sealed class TwilioSmsProvider(ILogger<TwilioSmsProvider> logger) : ISmsProvider
{
    public string ProviderName => "Twilio";

    public Task<NotificationDispatchResult> SendAsync(
        SmsNotificationMessage message,
        CancellationToken cancellationToken = default)
    {
        logger.LogWarning(
            "Twilio SMS provider is a placeholder (TenantId={TenantId}, CorrelationId={CorrelationId}).",
            message.TenantId,
            message.CorrelationId);

        return Task.FromResult(new NotificationDispatchResult(
            false,
            null,
            "Twilio SMS provider is a placeholder and is not configured."));
    }
}
