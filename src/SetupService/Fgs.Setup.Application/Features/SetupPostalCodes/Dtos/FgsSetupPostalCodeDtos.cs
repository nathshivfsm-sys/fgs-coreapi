namespace Fgs.Setup.Application.Features.SetupPostalCodes.Dtos;

public sealed record FgsSetupPostalCodeSummaryDto(
    long Id,
    long TenantId,
    long CompanyId,
    string PostalCode,
    long? FgsSetupZoneId,
    long? FgsSetupTaxId,
    bool IsActive,
    DateTimeOffset CreatedOn,
    DateTimeOffset? UpdatedOn);

public sealed record FgsSetupPostalCodeDetailDto(
    long Id,
    long TenantId,
    long CompanyId,
    string PostalCode,
    long? FgsSetupZoneId,
    long? FgsSetupTaxId,
    bool IsActive,
    DateTimeOffset CreatedOn,
    string? CreatedBy,
    DateTimeOffset? UpdatedOn,
    string? UpdatedBy);

public sealed record FgsSetupPostalCodeLookupDto(
    long Id,
    string PostalCode);

public sealed record FgsSetupPostalCodeCreateDto(
    string PostalCode,
    long? FgsSetupZoneId,
    long? FgsSetupTaxId);

public sealed record FgsSetupPostalCodeUpdateDto(
    string PostalCode,
    long? FgsSetupZoneId,
    long? FgsSetupTaxId);

public sealed record FgsSetupPostalCodePatchDto(
    string? PostalCode,
    long? FgsSetupZoneId,
    long? FgsSetupTaxId,
    bool? IsActive);

public sealed record FgsSetupPostalCodeListFilters(
    string? PostalCode = null);
