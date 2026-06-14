namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSetupPricingMatrixOther</summary>
public sealed record FgsSetupPricingMatrixOtherSummaryDto(
    /// <summary>Reference to the pricing matrix.</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>PricingMatrixId</summary>
    long PricingMatrixId,
    /// <summary>Unique category code within the pricing matrix.</summary>
    string? CategoryCode,
    /// <summary>User-friendly category name.</summary>
    string? Name,
    /// <summary>Markup percentage applied to the base cost.</summary>
    decimal? MarkupPercent,
    /// <summary>Optional discount percentage applied after markup.</summary>
    decimal? DiscountPercent,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsSetupPricingMatrixOtherDetailDto(
    /// <summary>Reference to the pricing matrix.</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>PricingMatrixId</summary>
    long PricingMatrixId,
    /// <summary>Unique category code within the pricing matrix.</summary>
    string? CategoryCode,
    /// <summary>User-friendly category name.</summary>
    string? Name,
    /// <summary>Markup percentage applied to the base cost.</summary>
    decimal? MarkupPercent,
    /// <summary>Optional discount percentage applied after markup.</summary>
    decimal? DiscountPercent,
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

public sealed record FgsSetupPricingMatrixOtherCreateDto(
    /// <summary>PricingMatrixId</summary>
    long PricingMatrixId,
    /// <summary>Unique category code within the pricing matrix.</summary>
    string? CategoryCode,
    /// <summary>User-friendly category name.</summary>
    string? Name,
    /// <summary>Markup percentage applied to the base cost.</summary>
    decimal? MarkupPercent,
    /// <summary>Optional discount percentage applied after markup.</summary>
    decimal? DiscountPercent)
;

public sealed record FgsSetupPricingMatrixOtherUpdateDto(
    /// <summary>PricingMatrixId</summary>
    long PricingMatrixId,
    /// <summary>Unique category code within the pricing matrix.</summary>
    string? CategoryCode,
    /// <summary>User-friendly category name.</summary>
    string? Name,
    /// <summary>Markup percentage applied to the base cost.</summary>
    decimal? MarkupPercent,
    /// <summary>Optional discount percentage applied after markup.</summary>
    decimal? DiscountPercent)
;

public sealed record FgsSetupPricingMatrixOtherPatchDto(
    /// <summary>PricingMatrixId</summary>
    long? PricingMatrixId,
    /// <summary>Unique category code within the pricing matrix.</summary>
    string? CategoryCode,
    /// <summary>User-friendly category name.</summary>
    string? Name,
    /// <summary>Markup percentage applied to the base cost.</summary>
    decimal? MarkupPercent,
    /// <summary>Optional discount percentage applied after markup.</summary>
    decimal? DiscountPercent)
;

