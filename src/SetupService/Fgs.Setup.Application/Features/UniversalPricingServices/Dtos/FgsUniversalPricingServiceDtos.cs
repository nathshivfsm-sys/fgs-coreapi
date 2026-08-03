namespace Fgs.Setup.Application.Features.UniversalPricingServices.Dtos;

public sealed record FgsUniversalPricingServiceSummaryDto(
    long Id,
    string UniversalPricingServiceCode,
    short DisplayOrder,
    bool IsActive);

public sealed record FgsUniversalPricingServiceDetailDto(
    long Id,
    string UniversalPricingServiceCode,
    short DisplayOrder,
    bool IsActive);

public sealed record FgsUniversalPricingServiceLookupDto(
    long Id,
    string UniversalPricingServiceCode,
    short DisplayOrder);

public sealed record FgsUniversalPricingServiceCreateDto(
    string UniversalPricingServiceCode,
    short DisplayOrder);

public sealed record FgsUniversalPricingServiceUpdateDto(
    string UniversalPricingServiceCode,
    short DisplayOrder);

public sealed record FgsUniversalPricingServicePatchDto(
    string? UniversalPricingServiceCode,
    short? DisplayOrder,
    bool? IsActive);

public sealed record FgsUniversalPricingServiceListFilters(
    string? UniversalPricingServiceCode = null);
