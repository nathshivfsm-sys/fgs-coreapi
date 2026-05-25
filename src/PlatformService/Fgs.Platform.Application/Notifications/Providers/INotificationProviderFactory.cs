using Fgs.Platform.Application.Notifications.Channels;
using Fgs.Platform.Domain.Notifications;

namespace Fgs.Platform.Application.Notifications.Providers;

public interface INotificationProviderFactory
{
    IEmailProvider ResolveEmailProvider(long tenantId);

    ISmsProvider ResolveSmsProvider(long tenantId);

    IPushProvider ResolvePushProvider(long tenantId);

    EmailProviderKind ResolveEmailProviderKind(long tenantId);
}
