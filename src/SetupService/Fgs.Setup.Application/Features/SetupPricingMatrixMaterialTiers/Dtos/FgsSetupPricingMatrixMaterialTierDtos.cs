namespace Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Dtos;

public sealed record FgsSetupPricingMatrixMaterialTierSummaryDto(
    long Id,
    long PricingMatrixId,
    decimal FromCost,
    decimal? ToCost,
    decimal AdjustmentValue,
    bool IsActive);

public sealed record FgsSetupPricingMatrixMaterialTierDetailDto(
    long Id,
    long PricingMatrixId,
    decimal FromCost,
    decimal? ToCost,
    decimal AdjustmentValue,
    bool IsActive);

public sealed record FgsSetupPricingMatrixMaterialTierLookupDto(
    long Id,
    long PricingMatrixId,
    decimal FromCost,
    decimal AdjustmentValue);

public sealed record FgsSetupPricingMatrixMaterialTierCreateDto(
    long PricingMatrixId,
    decimal FromCost,
    decimal? ToCost,
    decimal AdjustmentValue);

public sealed record FgsSetupPricingMatrixMaterialTierUpdateDto(
    long PricingMatrixId,
    decimal FromCost,
    decimal? ToCost,
    decimal AdjustmentValue);

public sealed record FgsSetupPricingMatrixMaterialTierPatchDto(
    long? PricingMatrixId,
    decimal? FromCost,
    decimal? ToCost,
    decimal? AdjustmentValue,
    bool? IsActive);

public sealed record FgsSetupPricingMatrixMaterialTierListFilters(
    long? PricingMatrixId = null);
