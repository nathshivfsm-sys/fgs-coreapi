namespace Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Dtos;

public sealed record FgsUniversalMatrixFrequencyDiscountSummaryDto(
    long Id,
    long UniversalPricingServiceId,
    string Name,
    decimal DiscountPercent,
    short DisplayOrder,
    bool IsActive);

public sealed record FgsUniversalMatrixFrequencyDiscountDetailDto(
    long Id,
    long UniversalPricingServiceId,
    string Name,
    decimal DiscountPercent,
    short DisplayOrder,
    bool IsActive);

public sealed record FgsUniversalMatrixFrequencyDiscountLookupDto(
    long Id,
    long UniversalPricingServiceId,
    string Name,
    short DisplayOrder);

public sealed record FgsUniversalMatrixFrequencyDiscountCreateDto(
    long UniversalPricingServiceId,
    string Name,
    decimal DiscountPercent,
    short DisplayOrder);

public sealed record FgsUniversalMatrixFrequencyDiscountUpdateDto(
    long UniversalPricingServiceId,
    string Name,
    decimal DiscountPercent,
    short DisplayOrder);

public sealed record FgsUniversalMatrixFrequencyDiscountPatchDto(
    long? UniversalPricingServiceId,
    string? Name,
    decimal? DiscountPercent,
    short? DisplayOrder,
    bool? IsActive);

public sealed record FgsUniversalMatrixFrequencyDiscountListFilters(
    string? Name = null,
    long? UniversalPricingServiceId = null);
