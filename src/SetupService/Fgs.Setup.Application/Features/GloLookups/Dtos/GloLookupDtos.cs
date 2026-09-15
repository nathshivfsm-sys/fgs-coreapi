namespace Fgs.Setup.Application.Features.GloLookups.Dtos;

public sealed record GloCountryLookupDto(
    string CountryCode,
    string CountryName,
    string? CurrencyCode);

public sealed record GloStateProvinceLookupDto(
    int Id,
    string CountryCode,
    string StateProvinceCode,
    string StateProvinceName);

public sealed record GloTimeZoneLookupDto(
    short Id,
    string TimeZoneCode,
    string Name,
    bool IsActive);

public sealed record GloLanguageLookupDto(
    string LanguageCode,
    string LanguageName,
    string CultureCode);

public sealed record GloLocationTypeLookupDto(
    int Id,
    string Code,
    string Name);

public sealed record GloBusinessTypeLookupDto(
    int Id,
    string Code,
    string Name);

public sealed record GloSetupDescriptionTypeLookupDto(
    short Id,
    string Code,
    string Name);

public sealed record GloMasterEntityTypeLookupDto(
    int Id,
    string Code);

public sealed record GloUnitOfMeasureLookupDto(
    int Id,
    string UnitCode,
    string Name,
    string Abbreviation,
    string UnitType);

public sealed record GloInventoryTransactionSourceTypeLookupDto(
    int Id,
    string Code,
    string Name);

public sealed record GloInventoryTransactionTypeLookupDto(
    int Id,
    int InventoryTransactionSourceTypeId,
    string Code,
    string Name);

public sealed record GloAccountingIntegrationTypeLookupDto(
    int Id,
    string Code,
    string Name);

public sealed record GloAppointmentAssignmentEventTypeLookupDto(
    short EventTypeId,
    string Code,
    string Name);

public sealed record GloVehicleMaintenanceTypeLookupDto(
    int Id,
    string MaintenanceTypeCode,
    string Name);

public sealed record GloSetupTenantStatusLookupDto(
    short Id,
    string Name);
