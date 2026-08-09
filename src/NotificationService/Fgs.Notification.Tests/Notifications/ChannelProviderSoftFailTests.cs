using Fgs.Notification.Application.Notifications.Channels;
using Fgs.Notification.Infrastructure.Notifications.Providers.Email;
using Fgs.Notification.Infrastructure.Notifications.Providers.Push;
using Fgs.Notification.Infrastructure.Notifications.Providers.Sms;
using Fgs.Notification.Infrastructure.Options;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Fgs.Notification.Tests.Notifications;

public sealed class ChannelProviderSoftFailTests
{
    [Fact]
    public async Task Smtp_SoftFails_WhenHostMissing()
    {
        var provider = new SmtpEmailProvider(
            CreateMonitor(new SmtpOptions()),
            Mock.Of<ILogger<SmtpEmailProvider>>());

        var result = await provider.SendAsync(
            new EmailNotificationMessage(1, "a@b.com", null, "s", "<p>x</p>", null, "c1"));

        result.Success.Should().BeFalse();
        result.Error.Should().Contain("not configured");
    }

    [Fact]
    public async Task Twilio_SoftFails_WhenCredentialsMissing()
    {
        var provider = new TwilioSmsProvider(
            Mock.Of<IHttpClientFactory>(),
            CreateMonitor(new TwilioOptions()),
            Mock.Of<ILogger<TwilioSmsProvider>>());

        var result = await provider.SendAsync(
            new SmsNotificationMessage(1, "+15551212", "hi", "c1"));

        result.Success.Should().BeFalse();
        result.Error.Should().Contain("not configured");
    }

    [Fact]
    public async Task Firebase_SoftFails_WhenCredentialsMissing()
    {
        var provider = new FirebasePushProvider(
            CreateMonitor(new FirebaseOptions()),
            Mock.Of<ILogger<FirebasePushProvider>>());

        var result = await provider.SendAsync(
            new PushNotificationMessage(1, "token", "t", "b", null, "c1"));

        result.Success.Should().BeFalse();
        result.Error.Should().Contain("not configured");
    }

    private static IOptionsMonitor<T> CreateMonitor<T>(T value)
    {
        var monitor = new Mock<IOptionsMonitor<T>>();
        monitor.Setup(m => m.CurrentValue).Returns(value);
        return monitor.Object;
    }
}
