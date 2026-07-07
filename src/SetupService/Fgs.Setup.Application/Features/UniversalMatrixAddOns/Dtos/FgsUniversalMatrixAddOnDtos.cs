namespace Fgs.Setup.Application.Features.UniversalMatrixAddOns.Dtos;

public sealed record FgsUniversalMatrixAddOnSummaryDto(
    long Id,
    long UniversalPricingServiceId,
    string Name,
    string UnitType,
    decimal Price,
    short DisplayOrder,
    bool IsActive);

public sealed record FgsUniversalMatrixAddOnDetailDto(
    long Id,
    long UniversalPricingServiceId,
    string Name,
    string UnitType,
    decimal Price,
    short DisplayOrder,
    bool IsActive);

public sealed record FgsUniversalMatrixAddOnLookupDto(
    long Id,
    long UniversalPricingServiceId,
    string Name,
    short DisplayOrder);

public sealed record FgsUniversalMatrixAddOnCreateDto(
    long UniversalPricingServiceId,
    string Name,
    string UnitType,
    decimal Price,
    short DisplayOrder);

public sealed record FgsUniversalMatrixAddOnUpdateDto(
    long UniversalPricingServiceId,
    string Name,
    string UnitType,
    decimal Price,
    short DisplayOrder);

public sealed record FgsUniversalMatrixAddOnPatchDto(
    long? UniversalPricingServiceId,
    string? Name,
    string? UnitType,
    decimal? Price,
    short? DisplayOrder,
    bool? IsActive);

public sealed record FgsUniversalMatrixAddOnListFilters(
    string? Name = null,
    long? UniversalPricingServiceId = null);
