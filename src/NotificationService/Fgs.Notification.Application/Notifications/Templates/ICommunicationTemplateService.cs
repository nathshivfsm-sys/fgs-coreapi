using Fgs.Notification.Domain.Entities;
using Fgs.Notification.Domain.Notifications;

namespace Fgs.Notification.Application.Notifications.Templates;

public interface ICommunicationTemplateService
{
    /// <summary>
    /// Resolves the active template using tenant/company scope, then tenant-only, then global defaults.
    /// </summary>
    Task<FgsSetupCommunicationTemplate> GetActiveTemplateAsync(
        long tenantId,
        long? companyId,
        NotificationChannel channel,
        string templateCode,
        CancellationToken cancellationToken = default);
}
