namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsLeadSource</summary>
public sealed record FgsLeadSourceSummaryDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>SourceCode</summary>
    string? SourceCode,
    /// <summary>SourceName</summary>
    string? SourceName,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsLeadSourceDetailDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>SourceCode</summary>
    string? SourceCode,
    /// <summary>SourceName</summary>
    string? SourceName,
    /// <summary>Description</summary>
    string? Description,
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

public sealed record FgsLeadSourceCreateDto(
    /// <summary>SourceCode</summary>
    string? SourceCode,
    /// <summary>SourceName</summary>
    string? SourceName,
    /// <summary>Description</summary>
    string? Description)
;

public sealed record FgsLeadSourceUpdateDto(
    /// <summary>SourceCode</summary>
    string? SourceCode,
    /// <summary>SourceName</summary>
    string? SourceName,
    /// <summary>Description</summary>
    string? Description)
;

public sealed record FgsLeadSourcePatchDto(
    /// <summary>SourceCode</summary>
    string? SourceCode,
    /// <summary>SourceName</summary>
    string? SourceName,
    /// <summary>Description</summary>
    string? Description)
;

