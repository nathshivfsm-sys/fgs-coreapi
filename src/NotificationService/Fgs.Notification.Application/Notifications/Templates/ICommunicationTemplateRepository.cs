using Fgs.Notification.Domain.Entities;

namespace Fgs.Notification.Application.Notifications.Templates;

public interface ICommunicationTemplateRepository
{
    Task<FgsSetupCommunicationTemplate?> GetActiveTemplateAsync(
        long? tenantId,
        long? companyId,
        string templateType,
        string code,
        CancellationToken cancellationToken = default);
}
