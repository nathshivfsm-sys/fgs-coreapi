namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSetupPostalCode</summary>
public sealed record FgsSetupPostalCodeSummaryDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>PostalCode</summary>
    string? PostalCode,
    /// <summary>FgsSetupZoneId</summary>
    long? FgsSetupZoneId,
    /// <summary>FgsSetupTaxId</summary>
    long? FgsSetupTaxId,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsSetupPostalCodeDetailDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>PostalCode</summary>
    string? PostalCode,
    /// <summary>FgsSetupZoneId</summary>
    long? FgsSetupZoneId,
    /// <summary>FgsSetupTaxId</summary>
    long? FgsSetupTaxId,
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

public sealed record FgsSetupPostalCodeCreateDto(
    /// <summary>PostalCode</summary>
    string? PostalCode,
    /// <summary>FgsSetupZoneId</summary>
    long? FgsSetupZoneId,
    /// <summary>FgsSetupTaxId</summary>
    long? FgsSetupTaxId)
;

public sealed record FgsSetupPostalCodeUpdateDto(
    /// <summary>PostalCode</summary>
    string? PostalCode,
    /// <summary>FgsSetupZoneId</summary>
    long? FgsSetupZoneId,
    /// <summary>FgsSetupTaxId</summary>
    long? FgsSetupTaxId)
;

public sealed record FgsSetupPostalCodePatchDto(
    /// <summary>PostalCode</summary>
    string? PostalCode,
    /// <summary>FgsSetupZoneId</summary>
    long? FgsSetupZoneId,
    /// <summary>FgsSetupTaxId</summary>
    long? FgsSetupTaxId)
;

