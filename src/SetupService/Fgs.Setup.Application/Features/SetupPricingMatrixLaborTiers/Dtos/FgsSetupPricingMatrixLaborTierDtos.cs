namespace Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Dtos;

public sealed record FgsSetupPricingMatrixLaborTierSummaryDto(
    long Id,
    long PricingMatrixLaborId,
    short SequenceOrder,
    int DurationMinutes,
    decimal Rate,
    long? TechSkillLevelId,
    bool IsActive);

public sealed record FgsSetupPricingMatrixLaborTierDetailDto(
    long Id,
    long PricingMatrixLaborId,
    short SequenceOrder,
    int DurationMinutes,
    decimal Rate,
    long? TechSkillLevelId,
    bool IsActive);

public sealed record FgsSetupPricingMatrixLaborTierLookupDto(
    long Id,
    long PricingMatrixLaborId,
    short SequenceOrder);

public sealed record FgsSetupPricingMatrixLaborTierCreateDto(
    long PricingMatrixLaborId,
    short SequenceOrder,
    int DurationMinutes,
    decimal Rate,
    long? TechSkillLevelId);

public sealed record FgsSetupPricingMatrixLaborTierUpdateDto(
    long PricingMatrixLaborId,
    short SequenceOrder,
    int DurationMinutes,
    decimal Rate,
    long? TechSkillLevelId);

public sealed record FgsSetupPricingMatrixLaborTierPatchDto(
    long? PricingMatrixLaborId,
    short? SequenceOrder,
    int? DurationMinutes,
    decimal? Rate,
    long? TechSkillLevelId,
    bool? IsActive);

public sealed record FgsSetupPricingMatrixLaborTierListFilters(
    long? PricingMatrixLaborId = null);
