using Fgs.Platform.Application.Notifications.Channels;
using Fgs.Platform.Domain.Notifications;

namespace Fgs.Platform.Application.Notifications.Providers;

public interface INotificationProviderFactory
{
    IEmailProvider ResolveEmailProvider(Guid tenantId);

    ISmsProvider ResolveSmsProvider(Guid tenantId);

    IPushProvider ResolvePushProvider(Guid tenantId);

    EmailProviderKind ResolveEmailProviderKind(Guid tenantId);
}
