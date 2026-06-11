namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSetupLaborRateType</summary>
public sealed record FgsSetupLaborRateTypeSummaryDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>SortOrder</summary>
    int SortOrder,
    /// <summary>IsSystem</summary>
    bool IsSystem,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsSetupLaborRateTypeDetailDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>SortOrder</summary>
    int SortOrder,
    /// <summary>IsSystem</summary>
    bool IsSystem,
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

public sealed record FgsSetupLaborRateTypeCreateDto(
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>SortOrder</summary>
    int SortOrder,
    /// <summary>IsSystem</summary>
    bool IsSystem)
;

public sealed record FgsSetupLaborRateTypeUpdateDto(
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>SortOrder</summary>
    int SortOrder,
    /// <summary>IsSystem</summary>
    bool IsSystem)
;

public sealed record FgsSetupLaborRateTypePatchDto(
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>SortOrder</summary>
    int? SortOrder,
    /// <summary>IsSystem</summary>
    bool? IsSystem)
;

