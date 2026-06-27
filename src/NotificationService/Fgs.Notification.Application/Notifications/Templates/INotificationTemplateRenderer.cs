using Fgs.Notification.Domain.Notifications;

namespace Fgs.Notification.Application.Notifications.Templates;

public interface INotificationTemplateRenderer
{
    Task<RenderedNotificationTemplate> RenderAsync(
        long tenantId,
        long? companyId,
        NotificationChannel channel,
        string templateCode,
        IReadOnlyDictionary<string, string> tokens,
        CancellationToken cancellationToken = default);
}

public sealed record RenderedNotificationTemplate(
    string Subject,
    string HtmlBody,
    string PlainTextBody);
