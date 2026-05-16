using Fgs.Platform.Application.Configuration;
using Fgs.Platform.Domain.Notifications;
using Fgs.Platform.Infrastructure.Notifications.Providers;
using Fgs.Platform.Infrastructure.Notifications.Providers.Email;
using Fgs.Platform.Infrastructure.Notifications.Providers.Push;
using Fgs.Platform.Infrastructure.Notifications.Providers.Sms;
using FluentAssertions;
using Moq;

namespace Fgs.Platform.Tests.Notifications;

public sealed class NotificationProviderFactoryTests
{
    [Fact]
    public void ResolveEmailProvider_UsesSendGrid_ByDefault()
    {
        var tenantConfig = new Mock<ITenantConfigurationResolver>();
        tenantConfig.Setup(t => t.GetProviderConfiguration(It.IsAny<Guid>()))
            .Returns(new TenantProviderConfiguration(EmailProviderKind.SendGrid, "Twilio", "Firebase"));

        var factory = new NotificationProviderFactory(
            tenantConfig.Object,
            new SendGridEmailProvider(Mock.Of<Application.Integrations.SendGrid.ISendGridIntegrationClient>()),
            new SmtpEmailProvider(Mock.Of<Microsoft.Extensions.Logging.ILogger<SmtpEmailProvider>>()),
            new TwilioSmsProvider(Mock.Of<Microsoft.Extensions.Logging.ILogger<TwilioSmsProvider>>()),
            new FirebasePushProvider(Mock.Of<Microsoft.Extensions.Logging.ILogger<FirebasePushProvider>>()));

        var provider = factory.ResolveEmailProvider(Guid.NewGuid());
        provider.ProviderName.Should().Be("SendGrid");
    }

    [Fact]
    public void ResolveEmailProvider_UsesSmtp_WhenTenantConfigured()
    {
        var tenantId = Guid.NewGuid();
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
