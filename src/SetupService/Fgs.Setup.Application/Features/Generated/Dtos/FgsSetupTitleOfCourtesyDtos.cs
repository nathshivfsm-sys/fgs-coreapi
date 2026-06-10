namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSetupTitleOfCourtesy</summary>
public sealed record FgsSetupTitleOfCourtesySummaryDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>Code</summary>
    string? Code,
    /// <summary>DisplayName</summary>
    string? DisplayName,
    /// <summary>SortOrder</summary>
    int? SortOrder,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsSetupTitleOfCourtesyDetailDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>Code</summary>
    string? Code,
    /// <summary>DisplayName</summary>
    string? DisplayName,
    /// <summary>SortOrder</summary>
    int? SortOrder,
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

public sealed record FgsSetupTitleOfCourtesyCreateDto(
    /// <summary>Code</summary>
    string? Code,
    /// <summary>DisplayName</summary>
    string? DisplayName,
    /// <summary>SortOrder</summary>
    int? SortOrder)
;

public sealed record FgsSetupTitleOfCourtesyUpdateDto(
    /// <summary>Code</summary>
    string? Code,
    /// <summary>DisplayName</summary>
    string? DisplayName,
    /// <summary>SortOrder</summary>
    int? SortOrder)
;

public sealed record FgsSetupTitleOfCourtesyPatchDto(
    /// <summary>Code</summary>
    string? Code,
    /// <summary>DisplayName</summary>
    string? DisplayName,
    /// <summary>SortOrder</summary>
    int? SortOrder)
;

