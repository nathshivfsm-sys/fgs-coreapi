using Fgs.Notification.Application.Configuration;
using Fgs.Notification.Application.Notifications.Channels;
using Fgs.Notification.Application.Notifications.Providers;
using Fgs.Notification.Domain.Notifications;
using Fgs.Notification.Infrastructure.Notifications.Providers.Email;
using Fgs.Notification.Infrastructure.Notifications.Providers.Push;
using Fgs.Notification.Infrastructure.Notifications.Providers.Sms;

namespace Fgs.Notification.Infrastructure.Notifications.Providers;

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
