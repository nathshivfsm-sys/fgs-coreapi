using Fgs.Notification.Domain.Entities;
using Fgs.Notification.Domain.Notifications;

namespace Fgs.Notification.Application.Notifications.Templates;

public interface ICommunicationTemplateService
{
    /// <summary>
    /// Resolves the active template via SetupService (company scope, tenant scope, global, then Glo fallback).
    /// </summary>
    Task<FgsSetupCommunicationTemplate> GetActiveTemplateAsync(
        long tenantId,
        long? companyId,
        NotificationChannel channel,
        string templateCode,
        CancellationToken cancellationToken = default);
}
