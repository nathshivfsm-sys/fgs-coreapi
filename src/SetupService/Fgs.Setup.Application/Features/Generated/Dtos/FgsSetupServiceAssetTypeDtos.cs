namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSetupServiceAssetType</summary>
public sealed record FgsSetupServiceAssetTypeSummaryDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>Code</summary>
    string? Code,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsSetupServiceAssetTypeDetailDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>Code</summary>
    string? Code,
    /// <summary>Name</summary>
    string? Name,
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

public sealed record FgsSetupServiceAssetTypeCreateDto(
    /// <summary>Code</summary>
    string? Code,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description)
;

public sealed record FgsSetupServiceAssetTypeUpdateDto(
    /// <summary>Code</summary>
    string? Code,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description)
;

public sealed record FgsSetupServiceAssetTypePatchDto(
    /// <summary>Code</summary>
    string? Code,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description)
;

