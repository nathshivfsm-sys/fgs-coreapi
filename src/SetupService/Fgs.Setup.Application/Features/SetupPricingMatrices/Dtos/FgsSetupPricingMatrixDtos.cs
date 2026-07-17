namespace Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;

public sealed record FgsSetupPricingMatrixSummaryDto(
    long Id,
    string Code,
    string Name,
    bool IsDefault,
    bool IsLaborTierStructure,
    bool IsLaborRateBySkillLevel,
    short PriceAdjustmentTypeId,
    DateOnly EffectiveFrom,
    DateOnly? EffectiveTo,
    bool IsMobileVisible,
    bool IsActive);

public sealed record FgsSetupPricingMatrixLaborTierDetailDto(
    long Id,
    short SequenceOrder,
    int DurationMinutes,
    decimal Rate,
    long? TechSkillLevelId,
    bool IsActive);

public sealed record FgsSetupPricingMatrixLaborLineDetailDto(
    long Id,
    int LaborRateTypeId,
    long? TechSkillLevelId,
    decimal BaseRate,
    decimal? OvertimeMultiplier,
    decimal? DoubleTimeMultiplier,
    decimal? DiscountPercent,
    bool IsActive,
    IReadOnlyList<FgsSetupPricingMatrixLaborTierDetailDto> Tiers);

public sealed record FgsSetupPricingMatrixMaterialTierDetailDto(
    long Id,
    decimal FromCost,
    decimal? ToCost,
    decimal AdjustmentValue,
    bool IsActive);

public sealed record FgsSetupPricingMatrixOtherItemDetailDto(
    long Id,
    string CategoryCode,
    string Name,
    decimal? AdjustmentValue,
    decimal? DiscountPercent,
    bool IsActive);

public sealed record FgsSetupPricingMatrixDetailDto(
    long Id,
    string Code,
    string Name,
    bool IsDefault,
    bool IsLaborTierStructure,
    bool IsLaborRateBySkillLevel,
    short PriceAdjustmentTypeId,
    DateOnly EffectiveFrom,
    DateOnly? EffectiveTo,
    bool IsMobileVisible,
    bool IsActive,
    IReadOnlyList<FgsSetupPricingMatrixLaborLineDetailDto> LaborLines,
    IReadOnlyList<FgsSetupPricingMatrixMaterialTierDetailDto> MaterialTiers,
    IReadOnlyList<FgsSetupPricingMatrixOtherItemDetailDto> OtherItems);

public sealed record FgsSetupPricingMatrixLookupDto(
    long Id,
    string Code,
    string Name,
    bool IsDefault);

public sealed record FgsSetupPricingMatrixLaborTierItemDto(
    long? Id,
    short SequenceOrder,
    int DurationMinutes,
    decimal Rate,
    long? TechSkillLevelId);

public sealed record FgsSetupPricingMatrixLaborLineDto(
    long? Id,
    int LaborRateTypeId,
    long? TechSkillLevelId,
    decimal? BaseRate,
    decimal? OvertimeMultiplier,
    decimal? DoubleTimeMultiplier,
    decimal? DiscountPercent,
    IReadOnlyList<FgsSetupPricingMatrixLaborTierItemDto>? Tiers);

public sealed record FgsSetupPricingMatrixMaterialTierDto(
    long? Id,
    decimal FromCost,
    decimal? ToCost,
    decimal AdjustmentValue);

public sealed record FgsSetupPricingMatrixOtherItemDto(
    long? Id,
    string CategoryCode,
    string Name,
    decimal? AdjustmentValue,
    decimal? DiscountPercent);

public sealed record FgsSetupPricingMatrixWriteDto(
    string Name,
    string Description,
    bool IsDefault,
    bool IsLaborTierStructure,
    bool IsLaborRateBySkillLevel,
    short? PriceAdjustmentTypeId,
    DateOnly? EffectiveFrom,
    DateOnly? EffectiveTo,
    bool? IsMobileVisible,
    IReadOnlyList<FgsSetupPricingMatrixLaborLineDto>? LaborLines,
    IReadOnlyList<FgsSetupPricingMatrixMaterialTierDto>? MaterialTiers,
    IReadOnlyList<FgsSetupPricingMatrixOtherItemDto>? OtherItems);

public sealed record FgsSetupPricingMatrixCreateDto(
    string Name,
    string Description,
    bool IsDefault,
    bool IsLaborTierStructure,
    bool IsLaborRateBySkillLevel,
    short? PriceAdjustmentTypeId,
    DateOnly? EffectiveFrom,
    DateOnly? EffectiveTo,
    bool? IsMobileVisible,
    IReadOnlyList<FgsSetupPricingMatrixLaborLineDto>? LaborLines,
    IReadOnlyList<FgsSetupPricingMatrixMaterialTierDto>? MaterialTiers,
    IReadOnlyList<FgsSetupPricingMatrixOtherItemDto>? OtherItems);

public sealed record FgsSetupPricingMatrixUpdateDto(
    string Name,
    string Description,
    bool IsDefault,
    bool IsLaborTierStructure,
    bool IsLaborRateBySkillLevel,
    short? PriceAdjustmentTypeId,
    DateOnly? EffectiveFrom,
    DateOnly? EffectiveTo,
    bool? IsMobileVisible,
    IReadOnlyList<FgsSetupPricingMatrixLaborLineDto>? LaborLines,
    IReadOnlyList<FgsSetupPricingMatrixMaterialTierDto>? MaterialTiers,
    IReadOnlyList<FgsSetupPricingMatrixOtherItemDto>? OtherItems);

public sealed record FgsSetupPricingMatrixPatchDto(
    string? Name,
    string? Description,
    bool? IsDefault,
    bool? IsLaborTierStructure,
    bool? IsLaborRateBySkillLevel,
    short? PriceAdjustmentTypeId,
    DateOnly? EffectiveFrom,
    DateOnly? EffectiveTo,
    bool? IsMobileVisible,
    bool? IsActive);

public sealed record FgsSetupPricingMatrixListFilters(
    string? Code = null,
    bool? IsDefault = null);
