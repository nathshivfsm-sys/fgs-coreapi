using Fgs.Platform.Domain.Notifications;
using Fgs.Platform.Application.Notifications.Templates;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Platform.Infrastructure.Database.Seed;
using Fgs.Platform.Infrastructure.Notifications.Templates;
using Fgs.Platform.Tests;
using Moq;

namespace Fgs.Platform.Tests.Notifications;

public sealed class DatabaseNotificationTemplateRendererTests
{
    [Fact]
    public async Task RenderAsync_Email_ProducesSubjectHtmlAndPlainText()
    {
        await using var context = TestDbContextFactory.Create();
        var seed = CommunicationTemplateSeedData.CompanyAdminInvitationEmail();
        context.CommunicationTemplates.Add(seed);
        await context.SaveChangesAsync();

        var templateService = new CommunicationTemplateService(new CommunicationTemplateRepository(context));
        var renderer = new DatabaseNotificationTemplateRenderer(
            templateService,
            new TemplateRenderer());

        var tokens = new Dictionary<string, string>
        {
            ["Name"] = "Jordan",
            ["PlatformName"] = "FGS",
            ["InviteLink"] = "https://example.com/invite",
            ["ExpirationHours"] = "72",
            ["CompanyName"] = "Acme Corp",
            ["SupportEmail"] = "support@fgs.example"
        };

        var result = await renderer.RenderAsync(
            1L,
            companyId: null,
            NotificationChannel.Email,
            CommunicationTemplateCodes.CompanyAdminInvitation,
            tokens);

        result.Subject.Should().Be("Welcome to FGS – Activate Your Admin Account");
        result.PlainTextBody.Should().Contain("Hello Jordan,");
        result.PlainTextBody.Should().Contain("https://example.com/invite");
        result.HtmlBody.Should().Contain("<p>Hello Jordan,</p>");
        result.HtmlBody.Should().NotContain("<br/></p><p>");
        result.PlainTextBody.Should().NotMatchRegex(@"\n{3,}");
    }

    [Fact]
    public async Task RenderAsync_Sms_UsesBodyOnly()
    {
        var templateService = new Mock<ICommunicationTemplateService>();
        templateService
            .Setup(s => s.GetActiveTemplateAsync(
                It.IsAny<long>(),
                It.IsAny<long?>(),
                NotificationChannel.Sms,
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Domain.Entities.FgsSetupCommunicationTemplate
            {
                Id = 99,
                TemplateType = CommunicationTemplateTypes.Sms,
                Code = "TEST_SMS",
                Name = "Test SMS",
                Body = "Code: {{Code}}",
                IsActive = true,
                CreatedOn = DateTimeOffset.UtcNow
            });

        var renderer = new DatabaseNotificationTemplateRenderer(
            templateService.Object,
            new TemplateRenderer());

        var result = await renderer.RenderAsync(
            1L,
            null,
            NotificationChannel.Sms,
            "TEST_SMS",
            new Dictionary<string, string> { ["Code"] = "123456" });

        result.PlainTextBody.Should().Be("Code: 123456");
        result.Subject.Should().BeEmpty();
        result.HtmlBody.Should().BeEmpty();
    }
}
