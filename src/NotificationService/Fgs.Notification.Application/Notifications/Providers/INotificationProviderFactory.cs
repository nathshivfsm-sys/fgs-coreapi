using Fgs.Notification.Application.Notifications.Channels;
using Fgs.Notification.Domain.Notifications;

namespace Fgs.Notification.Application.Notifications.Providers;

public interface INotificationProviderFactory
{
    IEmailProvider ResolveEmailProvider(long tenantId);

    ISmsProvider ResolveSmsProvider(long tenantId);

    IPushProvider ResolvePushProvider(long tenantId);

    EmailProviderKind ResolveEmailProviderKind(long tenantId);
}
