using Fgs.Notification.Domain.Notifications;
using Fgs.Notification.Application.Configuration;
using Fgs.Notification.Infrastructure.Notifications.Providers;
using Fgs.Notification.Infrastructure.Notifications.Providers.Email;
using Fgs.Notification.Infrastructure.Notifications.Providers.Push;
using Fgs.Notification.Infrastructure.Notifications.Providers.Sms;
using Fgs.Notification.Infrastructure.Options;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

        var factory = CreateFactory(tenantConfig.Object);

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

        var factory = CreateFactory(tenantConfig.Object);

        factory.ResolveEmailProvider(tenantId).ProviderName.Should().Be("Smtp");
    }

    private static NotificationProviderFactory CreateFactory(ITenantConfigurationResolver tenantConfig) =>
        new(
            tenantConfig,
            new SendGridEmailProvider(Mock.Of<Application.Integrations.SendGrid.ISendGridIntegrationClient>()),
            new SmtpEmailProvider(
                CreateOptionsMonitor(new SmtpOptions()),
                Mock.Of<ILogger<SmtpEmailProvider>>()),
            new TwilioSmsProvider(
                Mock.Of<IHttpClientFactory>(),
                CreateOptionsMonitor(new TwilioOptions()),
                Mock.Of<ILogger<TwilioSmsProvider>>()),
            new FirebasePushProvider(
                CreateOptionsMonitor(new FirebaseOptions()),
                Mock.Of<ILogger<FirebasePushProvider>>()));

    private static IOptionsMonitor<T> CreateOptionsMonitor<T>(T value)
    {
        var monitor = new Mock<IOptionsMonitor<T>>();
        monitor.Setup(m => m.CurrentValue).Returns(value);
        return monitor.Object;
    }
}
