namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSetupPricingMatrixLabor</summary>
public sealed record FgsSetupPricingMatrixLaborSummaryDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>PricingMatrixId</summary>
    long PricingMatrixId,
    /// <summary>LaborRateTypeId</summary>
    int LaborRateTypeId,
    /// <summary>TechSkillLevelId</summary>
    long? TechSkillLevelId,
    /// <summary>BaseRate</summary>
    decimal BaseRate,
    /// <summary>OvertimeMultiplier</summary>
    decimal? OvertimeMultiplier,
    /// <summary>DoubleTimeMultiplier</summary>
    decimal? DoubleTimeMultiplier,
    /// <summary>DiscountPercent</summary>
    decimal? DiscountPercent,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsSetupPricingMatrixLaborDetailDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>PricingMatrixId</summary>
    long PricingMatrixId,
    /// <summary>LaborRateTypeId</summary>
    int LaborRateTypeId,
    /// <summary>TechSkillLevelId</summary>
    long? TechSkillLevelId,
    /// <summary>BaseRate</summary>
    decimal BaseRate,
    /// <summary>OvertimeMultiplier</summary>
    decimal? OvertimeMultiplier,
    /// <summary>DoubleTimeMultiplier</summary>
    decimal? DoubleTimeMultiplier,
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

public sealed record FgsSetupPricingMatrixLaborCreateDto(
    /// <summary>PricingMatrixId</summary>
    long PricingMatrixId,
    /// <summary>LaborRateTypeId</summary>
    int LaborRateTypeId,
    /// <summary>TechSkillLevelId</summary>
    long? TechSkillLevelId,
    /// <summary>BaseRate</summary>
    decimal BaseRate,
    /// <summary>OvertimeMultiplier</summary>
    decimal? OvertimeMultiplier,
    /// <summary>DoubleTimeMultiplier</summary>
    decimal? DoubleTimeMultiplier,
    /// <summary>DiscountPercent</summary>
    decimal? DiscountPercent)
;

public sealed record FgsSetupPricingMatrixLaborUpdateDto(
    /// <summary>PricingMatrixId</summary>
    long PricingMatrixId,
    /// <summary>LaborRateTypeId</summary>
    int LaborRateTypeId,
    /// <summary>TechSkillLevelId</summary>
    long? TechSkillLevelId,
    /// <summary>BaseRate</summary>
    decimal BaseRate,
    /// <summary>OvertimeMultiplier</summary>
    decimal? OvertimeMultiplier,
    /// <summary>DoubleTimeMultiplier</summary>
    decimal? DoubleTimeMultiplier,
    /// <summary>DiscountPercent</summary>
    decimal? DiscountPercent)
;

public sealed record FgsSetupPricingMatrixLaborPatchDto(
    /// <summary>PricingMatrixId</summary>
    long? PricingMatrixId,
    /// <summary>LaborRateTypeId</summary>
    int? LaborRateTypeId,
    /// <summary>TechSkillLevelId</summary>
    long? TechSkillLevelId,
    /// <summary>BaseRate</summary>
    decimal? BaseRate,
    /// <summary>OvertimeMultiplier</summary>
    decimal? OvertimeMultiplier,
    /// <summary>DoubleTimeMultiplier</summary>
    decimal? DoubleTimeMultiplier,
    /// <summary>DiscountPercent</summary>
    decimal? DiscountPercent)
;

