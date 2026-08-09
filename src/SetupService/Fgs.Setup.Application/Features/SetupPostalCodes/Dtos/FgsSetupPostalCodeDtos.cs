namespace Fgs.Setup.Application.Features.SetupPostalCodes.Dtos;

public sealed record FgsSetupPostalCodeSummaryDto(
    long Id,
    string PostalCode,
    string CountryCode,
    string StateProvinceCode,
    string City,
    decimal TripChargeAmount,
    long? FgsSetupZoneId,
    long? FgsSetupTaxId,
    bool IsActive);

public sealed record FgsSetupPostalCodeDetailDto(
    long Id,
    string PostalCode,
    string CountryCode,
    string StateProvinceCode,
    string City,
    decimal TripChargeAmount,
    long? FgsSetupZoneId,
    long? FgsSetupTaxId,
    bool IsActive);

public sealed record FgsSetupPostalCodeLookupDto(
    long Id,
    string PostalCode);

public sealed record FgsSetupPostalCodeCreateDto(
    string PostalCode,
    string CountryCode,
    string StateProvinceCode,
    string City,
    decimal TripChargeAmount,
    long? FgsSetupZoneId,
    long? FgsSetupTaxId);

public sealed record FgsSetupPostalCodeUpdateDto(
    string PostalCode,
    string CountryCode,
    string StateProvinceCode,
    string City,
    decimal TripChargeAmount,
    long? FgsSetupZoneId,
    long? FgsSetupTaxId);

public sealed record FgsSetupPostalCodePatchDto(
    string? PostalCode,
    string? CountryCode,
    string? StateProvinceCode,
    string? City,
    decimal? TripChargeAmount,
    long? FgsSetupZoneId,
    long? FgsSetupTaxId,
    bool? IsActive);

public sealed record FgsSetupPostalCodeListFilters(
    string? PostalCode = null);
