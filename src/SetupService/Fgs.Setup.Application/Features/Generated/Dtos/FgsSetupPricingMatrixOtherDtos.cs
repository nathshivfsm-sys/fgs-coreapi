namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSetupPricingMatrixOther</summary>
public sealed record FgsSetupPricingMatrixOtherSummaryDto(
    /// <summary>Id</summary>
    Guid Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>FgsSetupPricingMatrixId</summary>
    Guid FgsSetupPricingMatrixId,
    /// <summary>CategoryCode</summary>
    string? CategoryCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>MarkupPercent</summary>
    decimal? MarkupPercent,
    /// <summary>DiscountPercent</summary>
    decimal? DiscountPercent,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsSetupPricingMatrixOtherDetailDto(
    /// <summary>Id</summary>
    Guid Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>FgsSetupPricingMatrixId</summary>
    Guid FgsSetupPricingMatrixId,
    /// <summary>CategoryCode</summary>
    string? CategoryCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>MarkupPercent</summary>
    decimal? MarkupPercent,
    /// <summary>DiscountPercent</summary>
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
    /// <summary>FgsSetupPricingMatrixId</summary>
    Guid FgsSetupPricingMatrixId,
    /// <summary>CategoryCode</summary>
    string? CategoryCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>MarkupPercent</summary>
    decimal? MarkupPercent,
    /// <summary>DiscountPercent</summary>
    decimal? DiscountPercent)
;

public sealed record FgsSetupPricingMatrixOtherUpdateDto(
    /// <summary>FgsSetupPricingMatrixId</summary>
    Guid FgsSetupPricingMatrixId,
    /// <summary>CategoryCode</summary>
    string? CategoryCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>MarkupPercent</summary>
    decimal? MarkupPercent,
    /// <summary>DiscountPercent</summary>
    decimal? DiscountPercent)
;

public sealed record FgsSetupPricingMatrixOtherPatchDto(
    /// <summary>FgsSetupPricingMatrixId</summary>
    Guid? FgsSetupPricingMatrixId,
    /// <summary>CategoryCode</summary>
    string? CategoryCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>MarkupPercent</summary>
    decimal? MarkupPercent,
    /// <summary>DiscountPercent</summary>
    decimal? DiscountPercent)
;

