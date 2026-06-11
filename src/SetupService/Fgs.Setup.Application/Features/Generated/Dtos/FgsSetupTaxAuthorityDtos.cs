namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSetupTaxAuthority</summary>
public sealed record FgsSetupTaxAuthoritySummaryDto(
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
    /// <summary>RegionCode</summary>
    string? RegionCode,
    /// <summary>IsExternalSystemRecord</summary>
    bool IsExternalSystemRecord,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsSetupTaxAuthorityDetailDto(
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
    /// <summary>RegionCode</summary>
    string? RegionCode,
    /// <summary>IsExternalSystemRecord</summary>
    bool IsExternalSystemRecord,
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

public sealed record FgsSetupTaxAuthorityCreateDto(
    /// <summary>Code</summary>
    string? Code,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>RegionCode</summary>
    string? RegionCode,
    /// <summary>IsExternalSystemRecord</summary>
    bool IsExternalSystemRecord,
    /// <summary>Description</summary>
    string? Description)
;

public sealed record FgsSetupTaxAuthorityUpdateDto(
    /// <summary>Code</summary>
    string? Code,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>RegionCode</summary>
    string? RegionCode,
    /// <summary>IsExternalSystemRecord</summary>
    bool IsExternalSystemRecord,
    /// <summary>Description</summary>
    string? Description)
;

public sealed record FgsSetupTaxAuthorityPatchDto(
    /// <summary>Code</summary>
    string? Code,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>RegionCode</summary>
    string? RegionCode,
    /// <summary>IsExternalSystemRecord</summary>
    bool? IsExternalSystemRecord,
    /// <summary>Description</summary>
    string? Description)
;

