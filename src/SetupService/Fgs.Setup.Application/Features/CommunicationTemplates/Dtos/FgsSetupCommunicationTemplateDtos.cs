namespace Fgs.Setup.Application.Features.CommunicationTemplates.Dtos;

public sealed record FgsSetupCommunicationTemplateSummaryDto(
    long Id,
    string CommunicationChannel,
    string TemplateType,
    string Code,
    string Name,
    string? Subject,
    string Body,
    bool IsMobileVisible,
    bool IsActive);

public sealed record FgsSetupCommunicationTemplateDetailDto(
    long Id,
    string CommunicationChannel,
    string TemplateType,
    string Code,
    string Name,
    string? Subject,
    string Body,
    bool IsMobileVisible,
    bool IsActive);

public sealed record FgsSetupCommunicationTemplateLookupDto(
    long Id,
    string CommunicationChannel,
    string TemplateType,
    string Code,
    string Name);

public sealed record FgsSetupCommunicationTemplateCreateDto(
    string CommunicationChannel,
    string TemplateType,
    string Code,
    string Name,
    string? Subject,
    string Body,
    bool IsMobileVisible);

public sealed record FgsSetupCommunicationTemplateUpdateDto(
    string CommunicationChannel,
    string TemplateType,
    string Code,
    string Name,
    string? Subject,
    string Body,
    bool IsMobileVisible);

public sealed record FgsSetupCommunicationTemplatePatchDto(
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
