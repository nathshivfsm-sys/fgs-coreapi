namespace Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Dtos;

public sealed record FgsUniversalMatrixOneTimeFeeSummaryDto(
    long Id,
    long UniversalPricingServiceId,
    string Name,
    decimal Amount,
    short DisplayOrder,
    bool IsActive);

public sealed record FgsUniversalMatrixOneTimeFeeDetailDto(
    long Id,
    long UniversalPricingServiceId,
    string Name,
    decimal Amount,
    short DisplayOrder,
    bool IsActive);

public sealed record FgsUniversalMatrixOneTimeFeeLookupDto(
    long Id,
    long UniversalPricingServiceId,
    string Name,
    short DisplayOrder);

public sealed record FgsUniversalMatrixOneTimeFeeCreateDto(
    long UniversalPricingServiceId,
    string Name,
    decimal Amount,
    short DisplayOrder);

public sealed record FgsUniversalMatrixOneTimeFeeUpdateDto(
    long UniversalPricingServiceId,
    string Name,
    decimal Amount,
    short DisplayOrder);

public sealed record FgsUniversalMatrixOneTimeFeePatchDto(
    long? UniversalPricingServiceId,
    string? Name,
    decimal? Amount,
    short? DisplayOrder,
    bool? IsActive);

public sealed record FgsUniversalMatrixOneTimeFeeListFilters(
    long? UniversalPricingServiceId = null,
    string? Name = null);
