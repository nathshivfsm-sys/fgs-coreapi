using Fgs.Platform.Domain.Notifications;

namespace Fgs.Platform.Application.Notifications.Templates;

public interface INotificationTemplateRenderer
{
    Task<RenderedNotificationTemplate> RenderAsync(
        Guid tenantId,
        Guid? companyId,
        NotificationChannel channel,
        string templateCode,
        IReadOnlyDictionary<string, string> tokens,
        CancellationToken cancellationToken = default);
}

public sealed record RenderedNotificationTemplate(
    string Subject,
    string HtmlBody,
    string PlainTextBody);
