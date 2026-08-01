namespace Fgs.Setup.Application.Features.UniversalPricingServices.Dtos;

public sealed record FgsUniversalPricingServiceSummaryDto(
    long Id,
    string UniversalPricingServiceCode,
    short DisplayOrder,
    bool IsActive);

public sealed record FgsUniversalMatrixTierDetailDto(
    long Id,
    string Name,
    decimal Multiplier,
    short DisplayOrder,
    bool IsActive);

public sealed record FgsUniversalMatrixSizeTierDetailDto(
    long Id,
    string Name,
    decimal Multiplier,
    short DisplayOrder,
    bool IsActive);

public sealed record FgsUniversalMatrixItemDetailDto(
    long Id,
    string ItemName,
    string UnitType,
    decimal BasePrice,
    short DisplayOrder,
    bool IsActive);

public sealed record FgsUniversalMatrixFrequencyDiscountDetailDto(
    long Id,
    string Name,
    decimal DiscountPercent,
    short DisplayOrder,
    bool IsActive);

public sealed record FgsUniversalMatrixOneTimeFeeDetailDto(
    long Id,
    string Name,
    decimal Amount,
    short DisplayOrder,
    bool IsActive);

public sealed record FgsUniversalMatrixAddOnDetailDto(
    long Id,
    string Name,
    string UnitType,
    decimal Price,
    short DisplayOrder,
    bool IsActive);

public sealed record FgsUniversalPricingServiceDetailDto(
    long Id,
    string UniversalPricingServiceCode,
    short DisplayOrder,
    bool IsActive,
    IReadOnlyList<FgsUniversalMatrixTierDetailDto> Tiers,
    IReadOnlyList<FgsUniversalMatrixSizeTierDetailDto> SizeTiers,
    IReadOnlyList<FgsUniversalMatrixItemDetailDto> Items,
    IReadOnlyList<FgsUniversalMatrixFrequencyDiscountDetailDto> FrequencyDiscounts,
    IReadOnlyList<FgsUniversalMatrixOneTimeFeeDetailDto> OneTimeFees,
    IReadOnlyList<FgsUniversalMatrixAddOnDetailDto> AddOns);

public sealed record FgsUniversalPricingServiceLookupDto(
    long Id,
    string UniversalPricingServiceCode,
    short DisplayOrder);

public sealed record FgsUniversalMatrixTierItemDto(
    long? Id,
    string Name,
    decimal Multiplier,
    short DisplayOrder);

public sealed record FgsUniversalMatrixSizeTierItemDto(
    long? Id,
    string Name,
    decimal Multiplier,
    short DisplayOrder);

public sealed record FgsUniversalMatrixItemItemDto(
    long? Id,
    string ItemName,
    string UnitType,
    decimal BasePrice,
    short DisplayOrder);

public sealed record FgsUniversalMatrixFrequencyDiscountItemDto(
    long? Id,
    string Name,
    decimal DiscountPercent,
    short DisplayOrder);

public sealed record FgsUniversalMatrixOneTimeFeeItemDto(
    long? Id,
    string Name,
    decimal Amount,
    short DisplayOrder);

public sealed record FgsUniversalMatrixAddOnItemDto(
    long? Id,
    string Name,
    string UnitType,
    decimal Price,
    short DisplayOrder);

public sealed record FgsUniversalPricingServiceCreateDto(
    string UniversalPricingServiceCode,
    short DisplayOrder,
    IReadOnlyList<FgsUniversalMatrixTierItemDto>? Tiers = null,
    IReadOnlyList<FgsUniversalMatrixSizeTierItemDto>? SizeTiers = null,
    IReadOnlyList<FgsUniversalMatrixItemItemDto>? Items = null,
    IReadOnlyList<FgsUniversalMatrixFrequencyDiscountItemDto>? FrequencyDiscounts = null,
    IReadOnlyList<FgsUniversalMatrixOneTimeFeeItemDto>? OneTimeFees = null,
    IReadOnlyList<FgsUniversalMatrixAddOnItemDto>? AddOns = null);

public sealed record FgsUniversalPricingServiceUpdateDto(
    string UniversalPricingServiceCode,
    short DisplayOrder,
    IReadOnlyList<FgsUniversalMatrixTierItemDto>? Tiers = null,
    IReadOnlyList<FgsUniversalMatrixSizeTierItemDto>? SizeTiers = null,
    IReadOnlyList<FgsUniversalMatrixItemItemDto>? Items = null,
    IReadOnlyList<FgsUniversalMatrixFrequencyDiscountItemDto>? FrequencyDiscounts = null,
    IReadOnlyList<FgsUniversalMatrixOneTimeFeeItemDto>? OneTimeFees = null,
    IReadOnlyList<FgsUniversalMatrixAddOnItemDto>? AddOns = null);

public sealed record FgsUniversalPricingServicePatchDto(
    string? UniversalPricingServiceCode,
    short? DisplayOrder,
    bool? IsActive);

public sealed record FgsUniversalPricingServiceListFilters(
    string? UniversalPricingServiceCode = null);
