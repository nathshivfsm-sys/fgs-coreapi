using Fgs.Platform.Application.Configuration;
using Fgs.Platform.Application.Notifications.Channels;
using Fgs.Platform.Application.Notifications.Providers;
using Fgs.Platform.Domain.Notifications;
using Fgs.Platform.Infrastructure.Notifications.Providers.Email;
using Fgs.Platform.Infrastructure.Notifications.Providers.Push;
using Fgs.Platform.Infrastructure.Notifications.Providers.Sms;

namespace Fgs.Platform.Infrastructure.Notifications.Providers;

public sealed class NotificationProviderFactory(
    ITenantConfigurationResolver tenantConfiguration,
    SendGridEmailProvider sendGridEmail,
    SmtpEmailProvider smtpEmail,
    TwilioSmsProvider twilioSms,
    FirebasePushProvider firebasePush) : INotificationProviderFactory
{
    public IEmailProvider ResolveEmailProvider(long tenantId) =>
        ResolveEmailProviderKind(tenantId) switch
        {
            EmailProviderKind.Smtp => smtpEmail,
            _ => sendGridEmail
        };

    public ISmsProvider ResolveSmsProvider(long tenantId) => twilioSms;

    public IPushProvider ResolvePushProvider(long tenantId) => firebasePush;

    public EmailProviderKind ResolveEmailProviderKind(long tenantId) =>
        tenantConfiguration.GetProviderConfiguration(tenantId).EmailProvider;
}
