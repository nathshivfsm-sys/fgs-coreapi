namespace Fgs.Platform.Application.Notifications.Templates;

public interface INotificationTemplateRenderer
{
    RenderedNotificationTemplate Render(
        string templateName,
        IReadOnlyDictionary<string, string> data);
}

public sealed record RenderedNotificationTemplate(
    string Subject,
    string HtmlBody,
    string PlainTextBody);
