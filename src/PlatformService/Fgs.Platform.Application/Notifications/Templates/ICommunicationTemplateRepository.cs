using Fgs.Platform.Domain.Entities;

namespace Fgs.Platform.Application.Notifications.Templates;

public interface ICommunicationTemplateRepository
{
    Task<FgsSetupCommunicationTemplate?> GetActiveTemplateAsync(
        Guid? tenantId,
        Guid? companyId,
        string templateType,
        string code,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);

    Task AddAsync(FgsSetupCommunicationTemplate template, CancellationToken cancellationToken = default);
}
