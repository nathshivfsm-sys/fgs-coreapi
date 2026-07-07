namespace Fgs.Setup.Application.Features.UniversalMatrixTiers.Dtos;

public sealed record FgsUniversalMatrixTierSummaryDto(
    long Id,
    long UniversalPricingServiceId,
    string Name,
    decimal Multiplier,
    short DisplayOrder,
    bool IsActive);

public sealed record FgsUniversalMatrixTierDetailDto(
    long Id,
    long UniversalPricingServiceId,
    string Name,
    decimal Multiplier,
    short DisplayOrder,
    bool IsActive);

public sealed record FgsUniversalMatrixTierLookupDto(
    long Id,
    long UniversalPricingServiceId,
    string Name,
    short DisplayOrder);

public sealed record FgsUniversalMatrixTierCreateDto(
    long UniversalPricingServiceId,
    string Name,
    decimal Multiplier,
    short DisplayOrder);

public sealed record FgsUniversalMatrixTierUpdateDto(
    long UniversalPricingServiceId,
    string Name,
    decimal Multiplier,
    short DisplayOrder);

public sealed record FgsUniversalMatrixTierPatchDto(
    long? UniversalPricingServiceId,
    string? Name,
    decimal? Multiplier,
    short? DisplayOrder,
    bool? IsActive);

public sealed record FgsUniversalMatrixTierListFilters(
    string? Name = null,
    long? UniversalPricingServiceId = null);
