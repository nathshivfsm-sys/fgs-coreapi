using Fgs.Notification.Domain.Notifications;
using Fgs.Notification.Application.Configuration;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Notification.Infrastructure.Notifications.Providers;
using Fgs.Notification.Infrastructure.Notifications.Providers.Email;
using Fgs.Notification.Infrastructure.Notifications.Providers.Push;
using Fgs.Notification.Infrastructure.Notifications.Providers.Sms;
using FluentAssertions;
using Moq;

namespace Fgs.Notification.Tests.Notifications;

public sealed class NotificationProviderFactoryTests
{
    [Fact]
    public void ResolveEmailProvider_UsesSendGrid_ByDefault()
    {
        var tenantConfig = new Mock<ITenantConfigurationResolver>();
        tenantConfig.Setup(t => t.GetProviderConfiguration(It.IsAny<long>()))
            .Returns(new TenantProviderConfiguration(EmailProviderKind.SendGrid, "Twilio", "Firebase"));

        var factory = new NotificationProviderFactory(
            tenantConfig.Object,
            new SendGridEmailProvider(Mock.Of<Application.Integrations.SendGrid.ISendGridIntegrationClient>()),
            new SmtpEmailProvider(Mock.Of<Microsoft.Extensions.Logging.ILogger<SmtpEmailProvider>>()),
            new TwilioSmsProvider(Mock.Of<Microsoft.Extensions.Logging.ILogger<TwilioSmsProvider>>()),
            new FirebasePushProvider(Mock.Of<Microsoft.Extensions.Logging.ILogger<FirebasePushProvider>>()));

        var provider = factory.ResolveEmailProvider(1001L);
        provider.ProviderName.Should().Be("SendGrid");
    }

    [Fact]
    public void ResolveEmailProvider_UsesSmtp_WhenTenantConfigured()
    {
        var tenantId = 2002L;
        var tenantConfig = new Mock<ITenantConfigurationResolver>();
        tenantConfig.Setup(t => t.GetProviderConfiguration(tenantId))
            .Returns(new TenantProviderConfiguration(EmailProviderKind.Smtp, "Twilio", "Firebase"));

        var factory = new NotificationProviderFactory(
            tenantConfig.Object,
            new SendGridEmailProvider(Mock.Of<Application.Integrations.SendGrid.ISendGridIntegrationClient>()),
            new SmtpEmailProvider(Mock.Of<Microsoft.Extensions.Logging.ILogger<SmtpEmailProvider>>()),
            new TwilioSmsProvider(Mock.Of<Microsoft.Extensions.Logging.ILogger<TwilioSmsProvider>>()),
            new FirebasePushProvider(Mock.Of<Microsoft.Extensions.Logging.ILogger<FirebasePushProvider>>()));

        factory.ResolveEmailProvider(tenantId).ProviderName.Should().Be("Smtp");
    }
}
