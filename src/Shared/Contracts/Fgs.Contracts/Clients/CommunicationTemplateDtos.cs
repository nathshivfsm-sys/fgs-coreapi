namespace Fgs.Contracts.Clients;

public sealed record CommunicationTemplateDto(
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
