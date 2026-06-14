namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSetupPricingMatrixMaterialTier</summary>
public sealed record FgsSetupPricingMatrixMaterialTierSummaryDto(
    /// <summary>Reference to the pricing matrix that contains this tier.</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>PricingMatrixId</summary>
    long PricingMatrixId,
    /// <summary>Inclusive minimum material cost for this pricing tier.</summary>
    decimal FromCost,
    /// <summary>Inclusive maximum material cost for this pricing tier. NULL indicates no upper limit.</summary>
    decimal? ToCost,
    /// <summary>Value used by the selected pricing adjustment type. Examples: 25 = 25% markup, 150 = fixed dollar markup, 1.75 = multiplier.</summary>
    decimal AdjustmentValue,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsSetupPricingMatrixMaterialTierDetailDto(
    /// <summary>Reference to the pricing matrix that contains this tier.</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>PricingMatrixId</summary>
    long PricingMatrixId,
    /// <summary>Inclusive minimum material cost for this pricing tier.</summary>
    decimal FromCost,
    /// <summary>Inclusive maximum material cost for this pricing tier. NULL indicates no upper limit.</summary>
    decimal? ToCost,
    /// <summary>Value used by the selected pricing adjustment type. Examples: 25 = 25% markup, 150 = fixed dollar markup, 1.75 = multiplier.</summary>
    decimal AdjustmentValue,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>CreatedBy</summary>
    string? CreatedBy,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>UpdatedBy</summary>
    string? UpdatedBy,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsSetupPricingMatrixMaterialTierCreateDto(
    /// <summary>PricingMatrixId</summary>
    long PricingMatrixId,
    /// <summary>Inclusive minimum material cost for this pricing tier.</summary>
    decimal FromCost,
    /// <summary>Inclusive maximum material cost for this pricing tier. NULL indicates no upper limit.</summary>
    decimal? ToCost,
    /// <summary>Value used by the selected pricing adjustment type. Examples: 25 = 25% markup, 150 = fixed dollar markup, 1.75 = multiplier.</summary>
    decimal AdjustmentValue)
;

public sealed record FgsSetupPricingMatrixMaterialTierUpdateDto(
    /// <summary>PricingMatrixId</summary>
    long PricingMatrixId,
    /// <summary>Inclusive minimum material cost for this pricing tier.</summary>
    decimal FromCost,
    /// <summary>Inclusive maximum material cost for this pricing tier. NULL indicates no upper limit.</summary>
    decimal? ToCost,
    /// <summary>Value used by the selected pricing adjustment type. Examples: 25 = 25% markup, 150 = fixed dollar markup, 1.75 = multiplier.</summary>
    decimal AdjustmentValue)
;

public sealed record FgsSetupPricingMatrixMaterialTierPatchDto(
    /// <summary>PricingMatrixId</summary>
    long? PricingMatrixId,
    /// <summary>Inclusive minimum material cost for this pricing tier.</summary>
    decimal? FromCost,
    /// <summary>Inclusive maximum material cost for this pricing tier. NULL indicates no upper limit.</summary>
    decimal? ToCost,
    /// <summary>Value used by the selected pricing adjustment type. Examples: 25 = 25% markup, 150 = fixed dollar markup, 1.75 = multiplier.</summary>
    decimal? AdjustmentValue)
;

