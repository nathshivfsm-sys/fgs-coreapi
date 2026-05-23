using Fgs.Platform.Domain.Entities;
using Fgs.Platform.Domain.Notifications;

namespace Fgs.Platform.Application.Notifications.Templates;

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
