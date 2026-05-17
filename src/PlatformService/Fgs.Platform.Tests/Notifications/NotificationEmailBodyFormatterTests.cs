using Fgs.Platform.Infrastructure.Notifications.Templates;
using FluentAssertions;

namespace Fgs.Platform.Tests.Notifications;

public sealed class NotificationEmailBodyFormatterTests
{
    [Fact]
    public void ToHtmlBody_GroupsParagraphsWithoutExtraBreaksBetweenBlocks()
    {
        const string plainText = """
            Hello Alex,

            Welcome to FGS.

            Your company account has been created.
            """;

        var html = NotificationEmailBodyFormatter.ToHtmlBody(plainText);

        html.Should().Be(
            "<p>Hello Alex,</p>" +
            "<p>Welcome to FGS.</p>" +
            "<p>Your company account has been created.</p>");
    }

    [Fact]
    public void ToHtmlBody_UsesLineBreaksWithinSameParagraph()
    {
        const string plainText = """
            During setup, you will be asked to:
            • Create or sign in to your account
            • Verify your email address
            """;

        var html = NotificationEmailBodyFormatter.ToHtmlBody(plainText);

        html.Should().Be(
            "<p>During setup, you will be asked to:<br/>" +
            "• Create or sign in to your account<br/>" +
            "• Verify your email address</p>");
    }

    [Fact]
    public void ToHtmlBody_CollapsesMultipleBlankLinesIntoSingleParagraphGap()
    {
        const string plainText = "Line one\n\n\n\nLine two";

        var html = NotificationEmailBodyFormatter.ToHtmlBody(plainText);

        html.Should().Be("<p>Line one</p><p>Line two</p>");
    }
}
