namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSetupTimeSlot</summary>
public sealed record FgsSetupTimeSlotSummaryDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>FgsSetupZoneId</summary>
    long? FgsSetupZoneId,
    /// <summary>Code</summary>
    string? Code,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>IsMobileVisible</summary>
    bool IsMobileVisible,
    /// <summary>IsCustomerPortalVisible</summary>
    bool IsCustomerPortalVisible,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsSetupTimeSlotDetailDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>FgsSetupZoneId</summary>
    long? FgsSetupZoneId,
    /// <summary>Code</summary>
    string? Code,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>IsMobileVisible</summary>
    bool IsMobileVisible,
    /// <summary>IsCustomerPortalVisible</summary>
    bool IsCustomerPortalVisible,
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

public sealed record FgsSetupTimeSlotCreateDto(
    /// <summary>FgsSetupZoneId</summary>
    long? FgsSetupZoneId,
    /// <summary>Code</summary>
    string? Code,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>IsMobileVisible</summary>
    bool IsMobileVisible,
    /// <summary>IsCustomerPortalVisible</summary>
    bool IsCustomerPortalVisible)
;

public sealed record FgsSetupTimeSlotUpdateDto(
    /// <summary>FgsSetupZoneId</summary>
    long? FgsSetupZoneId,
    /// <summary>Code</summary>
    string? Code,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>IsMobileVisible</summary>
    bool IsMobileVisible,
    /// <summary>IsCustomerPortalVisible</summary>
    bool IsCustomerPortalVisible)
;

public sealed record FgsSetupTimeSlotPatchDto(
    /// <summary>FgsSetupZoneId</summary>
    long? FgsSetupZoneId,
    /// <summary>Code</summary>
    string? Code,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>IsMobileVisible</summary>
    bool? IsMobileVisible,
    /// <summary>IsCustomerPortalVisible</summary>
    bool? IsCustomerPortalVisible)
;

