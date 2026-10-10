namespace Fgs.Setup.Application.Abstractions.CommunicationTemplates;

public interface IActiveCommunicationTemplateReadRepository
{
    Task<IReadOnlyList<ActiveCommunicationTemplateCandidate>> ListFgsMatchesAsync(
        long? tenantId,
        long? companyId,
        string templateType,
        string code,
        CancellationToken cancellationToken = default);

    Task<ActiveCommunicationTemplateCandidate?> FindLatestGloAsync(
        string templateType,
        string communicationChannel,
        string code,
        CancellationToken cancellationToken = default);
}

public sealed record ActiveCommunicationTemplateCandidate(
    long Id,
    long? TenantId,
    long? CompanyId,
    string TemplateType,
    string Code,
    string Name,
    string? Subject,
    string Body,
    bool IsMobileVisible,
    bool IsActive);
