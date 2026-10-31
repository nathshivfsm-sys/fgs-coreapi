namespace Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;

public sealed record FgsSetupPricingMatrixSummaryDto(
    long Id,
    string Code,
    string Name,
    bool IsDefault,
    bool IsLaborTierStructure,
    bool IsLaborRateBySkillLevel,
    short PriceAdjustmentTypeId,
    short Category,
    DateOnly EffectiveFrom,
    DateOnly? EffectiveTo,
    bool IsMobileVisible,
    bool IsUsedInPriceBook,
    bool IsActive);

public sealed record FgsSetupPricingMatrixDetailDto(
    long Id,
    string Code,
    string Name,
    bool IsDefault,
    bool IsLaborTierStructure,
    bool IsLaborRateBySkillLevel,
    short PriceAdjustmentTypeId,
    short Category,
    DateOnly EffectiveFrom,
    DateOnly? EffectiveTo,
    bool IsMobileVisible,
    bool IsUsedInPriceBook,
    bool IsActive);

public sealed record FgsSetupPricingMatrixLookupDto(
    long Id,
    string Code,
    string Name,
    bool IsDefault,
    short Category,
    bool IsUsedInPriceBook);

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
    bool? IsUsedInPriceBook);

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
    bool IsUsedInPriceBook);

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
    bool? IsUsedInPriceBook,
    bool? IsActive);

public sealed record FgsSetupPricingMatrixListFilters(
    string? Code = null,
    bool? IsDefault = null,
    short? Category = null,
    bool? IsUsedInPriceBook = null);

/// <summary>Header flags used by child validators.</summary>
public sealed record FgsSetupPricingMatrixFlagsDto(
    long Id,
    bool IsLaborTierStructure,
    bool IsLaborRateBySkillLevel,
    short PriceAdjustmentTypeId,
    bool IsActive);
