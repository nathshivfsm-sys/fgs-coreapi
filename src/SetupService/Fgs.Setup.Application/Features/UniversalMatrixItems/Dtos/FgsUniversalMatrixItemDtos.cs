namespace Fgs.Setup.Application.Features.UniversalMatrixItems.Dtos;

public sealed record FgsUniversalMatrixItemSummaryDto(
    long Id,
    long UniversalPricingServiceId,
    string ItemName,
    string UnitType,
    decimal BasePrice,
    short DisplayOrder,
    bool IsActive);

public sealed record FgsUniversalMatrixItemDetailDto(
    long Id,
    long UniversalPricingServiceId,
    string ItemName,
    string UnitType,
    decimal BasePrice,
    short DisplayOrder,
    bool IsActive);

public sealed record FgsUniversalMatrixItemLookupDto(
    long Id,
    long UniversalPricingServiceId,
    string ItemName,
    short DisplayOrder);

public sealed record FgsUniversalMatrixItemCreateDto(
    long UniversalPricingServiceId,
    string ItemName,
    string UnitType,
    decimal BasePrice,
    short DisplayOrder);

public sealed record FgsUniversalMatrixItemUpdateDto(
    long UniversalPricingServiceId,
    string ItemName,
    string UnitType,
    decimal BasePrice,
    short DisplayOrder);

public sealed record FgsUniversalMatrixItemPatchDto(
    long? UniversalPricingServiceId,
    string? ItemName,
    string? UnitType,
    decimal? BasePrice,
    short? DisplayOrder,
    bool? IsActive);

public sealed record FgsUniversalMatrixItemListFilters(
    string? ItemName = null,
    long? UniversalPricingServiceId = null);
