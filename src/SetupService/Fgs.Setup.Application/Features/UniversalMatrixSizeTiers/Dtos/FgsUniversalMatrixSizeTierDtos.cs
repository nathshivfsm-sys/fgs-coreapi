namespace Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Dtos;

public sealed record FgsUniversalMatrixSizeTierSummaryDto(
    long Id,
    long UniversalPricingServiceId,
    string Name,
    decimal Multiplier,
    short DisplayOrder,
    bool IsActive);

public sealed record FgsUniversalMatrixSizeTierDetailDto(
    long Id,
    long UniversalPricingServiceId,
    string Name,
    decimal Multiplier,
    short DisplayOrder,
    bool IsActive);

public sealed record FgsUniversalMatrixSizeTierLookupDto(
    long Id,
    long UniversalPricingServiceId,
    string Name,
    short DisplayOrder);

public sealed record FgsUniversalMatrixSizeTierCreateDto(
    long UniversalPricingServiceId,
    string Name,
    decimal Multiplier,
    short DisplayOrder);

public sealed record FgsUniversalMatrixSizeTierUpdateDto(
    long UniversalPricingServiceId,
    string Name,
    decimal Multiplier,
    short DisplayOrder);

public sealed record FgsUniversalMatrixSizeTierPatchDto(
    long? UniversalPricingServiceId,
    string? Name,
    decimal? Multiplier,
    short? DisplayOrder,
    bool? IsActive);

public sealed record FgsUniversalMatrixSizeTierListFilters(
    string? Name = null,
    long? UniversalPricingServiceId = null);
