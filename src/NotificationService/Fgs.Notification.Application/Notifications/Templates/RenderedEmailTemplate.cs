namespace Fgs.Notification.Application.Notifications.Templates;

public sealed record RenderedEmailTemplate(string Subject, string HtmlBody, string PlainTextBody);
