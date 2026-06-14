namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSetupPricingMatrix</summary>
public sealed record FgsSetupPricingMatrixSummaryDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>Code</summary>
    string? Code,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>IsLaborTierStructure</summary>
    bool IsLaborTierStructure,
    /// <summary>IsLaborRateBySkillLevel</summary>
    bool IsLaborRateBySkillLevel,
    /// <summary>EffectiveFrom</summary>
    DateOnly EffectiveFrom,
    /// <summary>EffectiveTo</summary>
    DateOnly? EffectiveTo,
    /// <summary>IsMobileVisible</summary>
    bool IsMobileVisible,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsSetupPricingMatrixDetailDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>Code</summary>
    string? Code,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>IsLaborTierStructure</summary>
    bool IsLaborTierStructure,
    /// <summary>IsLaborRateBySkillLevel</summary>
    bool IsLaborRateBySkillLevel,
    /// <summary>EffectiveFrom</summary>
    DateOnly EffectiveFrom,
    /// <summary>EffectiveTo</summary>
    DateOnly? EffectiveTo,
    /// <summary>IsMobileVisible</summary>
    bool IsMobileVisible,
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

public sealed record FgsSetupPricingMatrixCreateDto(
    /// <summary>Code</summary>
    string? Code,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>IsLaborTierStructure</summary>
    bool IsLaborTierStructure,
    /// <summary>IsLaborRateBySkillLevel</summary>
    bool IsLaborRateBySkillLevel,
    /// <summary>EffectiveFrom</summary>
    DateOnly EffectiveFrom,
    /// <summary>EffectiveTo</summary>
    DateOnly? EffectiveTo,
    /// <summary>IsMobileVisible</summary>
    bool IsMobileVisible)
;

public sealed record FgsSetupPricingMatrixUpdateDto(
    /// <summary>Code</summary>
    string? Code,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>IsLaborTierStructure</summary>
    bool IsLaborTierStructure,
    /// <summary>IsLaborRateBySkillLevel</summary>
    bool IsLaborRateBySkillLevel,
    /// <summary>EffectiveFrom</summary>
    DateOnly EffectiveFrom,
    /// <summary>EffectiveTo</summary>
    DateOnly? EffectiveTo,
    /// <summary>IsMobileVisible</summary>
    bool IsMobileVisible)
;

public sealed record FgsSetupPricingMatrixPatchDto(
    /// <summary>Code</summary>
    string? Code,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>IsLaborTierStructure</summary>
    bool? IsLaborTierStructure,
    /// <summary>IsLaborRateBySkillLevel</summary>
    bool? IsLaborRateBySkillLevel,
    /// <summary>EffectiveFrom</summary>
    DateOnly? EffectiveFrom,
    /// <summary>EffectiveTo</summary>
    DateOnly? EffectiveTo,
    /// <summary>IsMobileVisible</summary>
    bool? IsMobileVisible)
;

