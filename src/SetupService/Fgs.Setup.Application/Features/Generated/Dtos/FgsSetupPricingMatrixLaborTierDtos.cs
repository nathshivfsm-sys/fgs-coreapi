namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSetupPricingMatrixLaborTier</summary>
public sealed record FgsSetupPricingMatrixLaborTierSummaryDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>FgsSetupPricingMatrixLaborId</summary>
    Guid FgsSetupPricingMatrixLaborId,
    /// <summary>SequenceOrder</summary>
    int SequenceOrder,
    /// <summary>DurationMinutes</summary>
    int DurationMinutes,
    /// <summary>Rate</summary>
    decimal Rate,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsSetupPricingMatrixLaborTierDetailDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>FgsSetupPricingMatrixLaborId</summary>
    Guid FgsSetupPricingMatrixLaborId,
    /// <summary>SequenceOrder</summary>
    int SequenceOrder,
    /// <summary>DurationMinutes</summary>
    int DurationMinutes,
    /// <summary>Rate</summary>
    decimal Rate,
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

public sealed record FgsSetupPricingMatrixLaborTierCreateDto(
    /// <summary>FgsSetupPricingMatrixLaborId</summary>
    Guid FgsSetupPricingMatrixLaborId,
    /// <summary>SequenceOrder</summary>
    int SequenceOrder,
    /// <summary>DurationMinutes</summary>
    int DurationMinutes,
    /// <summary>Rate</summary>
    decimal Rate)
;

public sealed record FgsSetupPricingMatrixLaborTierUpdateDto(
    /// <summary>FgsSetupPricingMatrixLaborId</summary>
    Guid FgsSetupPricingMatrixLaborId,
    /// <summary>SequenceOrder</summary>
    int SequenceOrder,
    /// <summary>DurationMinutes</summary>
    int DurationMinutes,
    /// <summary>Rate</summary>
    decimal Rate)
;

public sealed record FgsSetupPricingMatrixLaborTierPatchDto(
    /// <summary>FgsSetupPricingMatrixLaborId</summary>
    Guid? FgsSetupPricingMatrixLaborId,
    /// <summary>SequenceOrder</summary>
    int? SequenceOrder,
    /// <summary>DurationMinutes</summary>
    int? DurationMinutes,
    /// <summary>Rate</summary>
    decimal? Rate)
;

