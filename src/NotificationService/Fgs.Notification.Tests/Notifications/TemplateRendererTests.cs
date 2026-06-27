using Fgs.Notification.Application.Notifications.Templates;
using Fgs.Notification.Infrastructure.Notifications.Templates;
using FluentAssertions;

namespace Fgs.Notification.Tests.Notifications;

public sealed class TemplateRendererTests
{
    private readonly TemplateRenderer _renderer = new();

    [Fact]
    public void Render_ReplacesAllPlaceholders()
    {
        const string template = "Hello {{FirstName}}, welcome to {{CompanyName}}. Link: {{InviteLink}}";
        var tokens = new Dictionary<string, string>
        {
            ["FirstName"] = "Alex",
            ["CompanyName"] = "Acme",
            ["InviteLink"] = "https://example.com/invite"
        };

        var result = _renderer.Render(template, tokens);

        result.Should().Be("Hello Alex, welcome to Acme. Link: https://example.com/invite");
    }

    [Fact]
    public void Render_WhenTokenMissing_ThrowsWithMissingTokenNames()
    {
        const string template = "Hello {{FirstName}}, your link is {{InviteLink}}";
        var tokens = new Dictionary<string, string> { ["FirstName"] = "Alex" };

        var act = () => _renderer.Render(template, tokens);

        var ex = act.Should().Throw<TemplateRenderingException>().Which;
        ex.MissingTokens.Should().ContainSingle().Which.Should().Be("InviteLink");
    }

    [Fact]
    public void Render_WhenTokenValueEmpty_Throws()
    {
        const string template = "Hello {{FirstName}}";
        var tokens = new Dictionary<string, string> { ["FirstName"] = "" };

        var act = () => _renderer.Render(template, tokens);

        act.Should().Throw<TemplateRenderingException>();
    }

    [Fact]
    public void ExtractTokenNames_FindsDistinctPlaceholders()
    {
        const string content = "{{Name}} and {{Name}} plus {{InviteLink}}";

        var names = TemplateRenderer.ExtractTokenNames(content);

        names.Should().BeEquivalentTo(["Name", "InviteLink"]);
    }

    [Fact]
    public void Render_CompanyAdminInvitationSeedSubject_RendersSuccessfully()
    {
        var tokens = new Dictionary<string, string>
        {
            ["PlatformName"] = "FGS",
            ["Name"] = "Jordan",
            ["InviteLink"] = "https://example.com/invite",
            ["ExpirationHours"] = "72",
            ["CompanyName"] = "Acme Corp",
            ["SupportEmail"] = "support@fgs.example"
        };

        var subject = _renderer.Render(
            "Welcome to {{PlatformName}} – Activate Your Admin Account",
            tokens);

        subject.Should().Be("Welcome to FGS – Activate Your Admin Account");
    }
}
