namespace Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Dtos;

public sealed record FgsSetupPricingMatrixLaborSummaryDto(
    long Id,
    long PricingMatrixId,
    int LaborRateTypeId,
    long? TechSkillLevelId,
    decimal BaseRate,
    decimal? OvertimeMultiplier,
    decimal? DoubleTimeMultiplier,
    decimal? DiscountPercent,
    bool IsActive);

public sealed record FgsSetupPricingMatrixLaborDetailDto(
    long Id,
    long PricingMatrixId,
    int LaborRateTypeId,
    long? TechSkillLevelId,
    decimal BaseRate,
    decimal? OvertimeMultiplier,
    decimal? DoubleTimeMultiplier,
    decimal? DiscountPercent,
    bool IsActive);

public sealed record FgsSetupPricingMatrixLaborLookupDto(
    long Id,
    long PricingMatrixId,
    int LaborRateTypeId);

public sealed record FgsSetupPricingMatrixLaborCreateDto(
    long PricingMatrixId,
    int LaborRateTypeId,
    long? TechSkillLevelId,
    decimal BaseRate,
    decimal? OvertimeMultiplier,
    decimal? DoubleTimeMultiplier,
    decimal? DiscountPercent);

public sealed record FgsSetupPricingMatrixLaborUpdateDto(
    long PricingMatrixId,
    int LaborRateTypeId,
    long? TechSkillLevelId,
    decimal BaseRate,
    decimal? OvertimeMultiplier,
    decimal? DoubleTimeMultiplier,
    decimal? DiscountPercent);

public sealed record FgsSetupPricingMatrixLaborPatchDto(
    long? PricingMatrixId,
    int? LaborRateTypeId,
    long? TechSkillLevelId,
    decimal? BaseRate,
    decimal? OvertimeMultiplier,
    decimal? DoubleTimeMultiplier,
    decimal? DiscountPercent,
    bool? IsActive);

public sealed record FgsSetupPricingMatrixLaborListFilters(
    long? PricingMatrixId = null);
