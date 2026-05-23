using Fgs.Platform.Application.Notifications.Channels;
using Fgs.Platform.Infrastructure.Integrations.SendGrid;
using Fgs.Platform.Infrastructure.Options;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Fgs.Platform.Tests.Integrations;

public sealed class SendGridIntegrationClientTests
{
    [Fact]
    public async Task SendEmailAsync_ReturnsFailure_WhenApiKeyMissing()
    {
        var client = new SendGridIntegrationClient(
            Options.Create(new SendGridOptions { ApiKey = "REPLACE_WITH_SENDGRID_API_KEY" }),
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
