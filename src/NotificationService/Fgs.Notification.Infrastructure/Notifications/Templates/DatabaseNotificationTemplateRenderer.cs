using Fgs.Notification.Application.Notifications.Templates;
using Fgs.Notification.Domain.Entities;
using Fgs.Notification.Domain.Notifications;

namespace Fgs.Notification.Infrastructure.Notifications.Templates;

public sealed class DatabaseNotificationTemplateRenderer(
    ICommunicationTemplateService templateService,
    ITemplateRenderer templateRenderer) : INotificationTemplateRenderer
{
    public async Task<RenderedNotificationTemplate> RenderAsync(
        long tenantId,
        long? companyId,
        NotificationChannel channel,
        string templateCode,
        IReadOnlyDictionary<string, string> tokens,
        CancellationToken cancellationToken = default)
    {
        var template = await templateService.GetActiveTemplateAsync(
            tenantId,
            companyId,
            channel,
            templateCode,
            cancellationToken);

        return channel switch
        {
            NotificationChannel.Email => RenderEmail(template, tokens),
            NotificationChannel.Sms => RenderSms(template, tokens),
            NotificationChannel.Push => RenderPush(template, tokens),
            _ => throw new NotSupportedException($"Channel '{channel}' is not supported for template rendering.")
        };
    }

    private RenderedNotificationTemplate RenderEmail(
        FgsSetupCommunicationTemplate template,
        IReadOnlyDictionary<string, string> tokens)
    {
        var subject = templateRenderer.Render(template.Subject ?? string.Empty, tokens);
        var plainBody = templateRenderer.Render(template.Body, tokens);
        var htmlBody = NotificationEmailBodyFormatter.ToHtmlBody(plainBody);
        return new RenderedNotificationTemplate(subject, htmlBody, plainBody);
    }

    private RenderedNotificationTemplate RenderSms(
        FgsSetupCommunicationTemplate template,
        IReadOnlyDictionary<string, string> tokens)
    {
        var textBody = templateRenderer.Render(template.Body, tokens);
        return new RenderedNotificationTemplate(string.Empty, string.Empty, textBody);
    }

    private RenderedNotificationTemplate RenderPush(
        FgsSetupCommunicationTemplate template,
        IReadOnlyDictionary<string, string> tokens)
    {
        var title = template.Subject is not null
            ? templateRenderer.Render(template.Subject, tokens)
            : template.Name;

        var body = templateRenderer.Render(template.Body, tokens);
        return new RenderedNotificationTemplate(title, string.Empty, body);
    }
}
