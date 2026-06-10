namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSetupServiceAssetManufacturer</summary>
public sealed record FgsSetupServiceAssetManufacturerSummaryDto(
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

public sealed record FgsSetupServiceAssetManufacturerDetailDto(
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

public sealed record FgsSetupServiceAssetManufacturerCreateDto(
    /// <summary>Code</summary>
    string? Code,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description)
;

public sealed record FgsSetupServiceAssetManufacturerUpdateDto(
    /// <summary>Code</summary>
    string? Code,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description)
;

public sealed record FgsSetupServiceAssetManufacturerPatchDto(
    /// <summary>Code</summary>
    string? Code,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description)
;

