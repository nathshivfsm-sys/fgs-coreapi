namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSetupCommunicationTemplate</summary>
public sealed record FgsSetupCommunicationTemplateSummaryDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long? TenantId,
    /// <summary>CompanyId</summary>
    long? CompanyId,
    /// <summary>CommunicationChannel</summary>
    string? CommunicationChannel,
    /// <summary>TemplateType</summary>
    string? TemplateType,
    /// <summary>Code</summary>
    string? Code,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Subject</summary>
    string? Subject,
    /// <summary>Body</summary>
    string? Body,
    /// <summary>IsMobileVisible</summary>
    bool IsMobileVisible,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsSetupCommunicationTemplateDetailDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long? TenantId,
    /// <summary>CompanyId</summary>
    long? CompanyId,
    /// <summary>CommunicationChannel</summary>
    string? CommunicationChannel,
    /// <summary>TemplateType</summary>
    string? TemplateType,
    /// <summary>Code</summary>
    string? Code,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Subject</summary>
    string? Subject,
    /// <summary>Body</summary>
    string? Body,
    /// <summary>IsMobileVisible</summary>
    bool IsMobileVisible,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>CreatedBy</summary>
    string? CreatedBy,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>UpdatedBy</summary>
    string? UpdatedBy,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsSetupCommunicationTemplateCreateDto(
    /// <summary>CommunicationChannel</summary>
    string? CommunicationChannel,
    /// <summary>TemplateType</summary>
    string? TemplateType,
    /// <summary>Code</summary>
    string? Code,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Subject</summary>
    string? Subject,
    /// <summary>Body</summary>
    string? Body,
    /// <summary>IsMobileVisible</summary>
    bool IsMobileVisible)
;

public sealed record FgsSetupCommunicationTemplateUpdateDto(
    /// <summary>CommunicationChannel</summary>
    string? CommunicationChannel,
    /// <summary>TemplateType</summary>
    string? TemplateType,
    /// <summary>Code</summary>
    string? Code,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Subject</summary>
    string? Subject,
    /// <summary>Body</summary>
    string? Body,
    /// <summary>IsMobileVisible</summary>
    bool IsMobileVisible)
;

public sealed record FgsSetupCommunicationTemplatePatchDto(
    /// <summary>CommunicationChannel</summary>
    string? CommunicationChannel,
    /// <summary>TemplateType</summary>
    string? TemplateType,
    /// <summary>Code</summary>
    string? Code,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Subject</summary>
    string? Subject,
    /// <summary>Body</summary>
    string? Body,
    /// <summary>IsMobileVisible</summary>
    bool? IsMobileVisible)
;

