using Fgs.Notification.Application.Notifications.Channels;
using Fgs.Notification.Infrastructure.Integrations.SendGrid;
using Fgs.Notification.Infrastructure.Options;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Fgs.Notification.Tests.Integrations;

public sealed class SendGridIntegrationClientTests
{
    [Fact]
    public async Task SendEmailAsync_ReturnsFailure_WhenApiKeyMissing()
    {
        var optionsMonitor = new Mock<IOptionsMonitor<SendGridOptions>>();
        optionsMonitor.Setup(m => m.CurrentValue).Returns(new SendGridOptions
        {
            ApiKey = "REPLACE_WITH_SENDGRID_API_KEY"
        });

        var client = new SendGridIntegrationClient(
            optionsMonitor.Object,
            Mock.Of<ILogger<SendGridIntegrationClient>>());

        var result = await client.SendEmailAsync(
            new EmailNotificationMessage(
                1L,
                "to@example.com",
                "To",
                "Subject",
                "<p>Body</p>",
                "Body",
                "corr"));

        result.Success.Should().BeFalse();
        result.Error.Should().Contain("API key");
    }
}
