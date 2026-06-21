namespace Fgs.Setup.Application.Features.CommunicationTemplates.Dtos;

public sealed record FgsSetupCommunicationTemplateSummaryDto(
    long Id,
    long? TenantId,
    long? CompanyId,
    string CommunicationChannel,
    string TemplateType,
    string Code,
    string Name,
    string? Subject,
    string Body,
    bool IsMobileVisible,
    bool IsActive,
    DateTimeOffset CreatedOn,
    DateTimeOffset? UpdatedOn);

public sealed record FgsSetupCommunicationTemplateDetailDto(
    long Id,
    long? TenantId,
    long? CompanyId,
    string CommunicationChannel,
    string TemplateType,
    string Code,
    string Name,
    string? Subject,
    string Body,
    bool IsMobileVisible,
    bool IsActive,
    DateTimeOffset CreatedOn,
    string? CreatedBy,
    DateTimeOffset? UpdatedOn,
    string? UpdatedBy);

public sealed record FgsSetupCommunicationTemplateLookupDto(
    long Id,
    string CommunicationChannel,
    string TemplateType,
    string Code,
    string Name);

public sealed record FgsSetupCommunicationTemplateCreateDto(
    long? TenantId,
    long? CompanyId,
    string CommunicationChannel,
    string TemplateType,
    string Code,
    string Name,
    string? Subject,
    string Body,
    bool IsMobileVisible);

public sealed record FgsSetupCommunicationTemplateUpdateDto(
    long? TenantId,
    long? CompanyId,
    string CommunicationChannel,
    string TemplateType,
    string Code,
    string Name,
    string? Subject,
    string Body,
    bool IsMobileVisible);

public sealed record FgsSetupCommunicationTemplatePatchDto(
    long? TenantId,
    long? CompanyId,
    string? CommunicationChannel,
    string? TemplateType,
    string? Code,
    string? Name,
    string? Subject,
    string? Body,
    bool? IsMobileVisible,
    bool? IsActive);

public sealed record FgsSetupCommunicationTemplateListFilters(
    string? CommunicationChannel = null,
    string? TemplateType = null,
    string? Code = null,
    string? Name = null);
