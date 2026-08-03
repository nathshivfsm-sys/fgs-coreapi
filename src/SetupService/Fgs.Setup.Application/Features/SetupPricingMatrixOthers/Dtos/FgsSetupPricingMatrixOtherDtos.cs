namespace Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Dtos;

public sealed record FgsSetupPricingMatrixOtherSummaryDto(
    long Id,
    long PricingMatrixId,
    string CategoryCode,
    string Name,
    decimal? AdjustmentValue,
    decimal? DiscountPercent,
    bool IsActive);

public sealed record FgsSetupPricingMatrixOtherDetailDto(
    long Id,
    long PricingMatrixId,
    string CategoryCode,
    string Name,
    decimal? AdjustmentValue,
    decimal? DiscountPercent,
    bool IsActive);

public sealed record FgsSetupPricingMatrixOtherLookupDto(
    long Id,
    long PricingMatrixId,
    string CategoryCode,
    string Name);

public sealed record FgsSetupPricingMatrixOtherCreateDto(
    long PricingMatrixId,
    string CategoryCode,
    string Name,
    decimal? AdjustmentValue,
    decimal? DiscountPercent);

public sealed record FgsSetupPricingMatrixOtherUpdateDto(
    long PricingMatrixId,
    string CategoryCode,
    string Name,
    decimal? AdjustmentValue,
    decimal? DiscountPercent);

public sealed record FgsSetupPricingMatrixOtherPatchDto(
    long? PricingMatrixId,
    string? CategoryCode,
    string? Name,
    decimal? AdjustmentValue,
    decimal? DiscountPercent,
    bool? IsActive);

public sealed record FgsSetupPricingMatrixOtherListFilters(
    long? PricingMatrixId = null,
    string? CategoryCode = null);
