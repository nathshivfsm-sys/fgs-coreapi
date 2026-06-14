namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsLeadDisqualificationReason</summary>
public sealed record FgsLeadDisqualificationReasonSummaryDto(
    /// <summary>Unique business code for the disqualification reason within a company.</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>ReasonCode</summary>
    string? ReasonCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? ReasonName,
    /// <summary>Optional description explaining the reason.</summary>
    string? Description,
    /// <summary>Controls the order in which reasons are displayed in dropdowns and lists.</summary>
    short DisplayOrder,
    /// <summary>Indicates whether the reason was seeded by the system or created by a user.</summary>
    bool IsSystem,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>Indicates whether the reason is available for selection.</summary>
    bool IsActive)
;

public sealed record FgsLeadDisqualificationReasonDetailDto(
    /// <summary>Unique business code for the disqualification reason within a company.</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>ReasonCode</summary>
    string? ReasonCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? ReasonName,
    /// <summary>Optional description explaining the reason.</summary>
    string? Description,
    /// <summary>Controls the order in which reasons are displayed in dropdowns and lists.</summary>
    short DisplayOrder,
    /// <summary>Indicates whether the reason was seeded by the system or created by a user.</summary>
    bool IsSystem,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>CreatedBy</summary>
    string? CreatedBy,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>UpdatedBy</summary>
    string? UpdatedBy,
    /// <summary>Indicates whether the reason is available for selection.</summary>
    bool IsActive)
;

public sealed record FgsLeadDisqualificationReasonCreateDto(
    /// <summary>ReasonCode</summary>
    string? ReasonCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? ReasonName,
    /// <summary>Optional description explaining the reason.</summary>
    string? Description,
    /// <summary>Controls the order in which reasons are displayed in dropdowns and lists.</summary>
    short DisplayOrder,
    /// <summary>Indicates whether the reason was seeded by the system or created by a user.</summary>
    bool IsSystem)
;

public sealed record FgsLeadDisqualificationReasonUpdateDto(
    /// <summary>ReasonCode</summary>
    string? ReasonCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? ReasonName,
    /// <summary>Optional description explaining the reason.</summary>
    string? Description,
    /// <summary>Controls the order in which reasons are displayed in dropdowns and lists.</summary>
    short DisplayOrder,
    /// <summary>Indicates whether the reason was seeded by the system or created by a user.</summary>
    bool IsSystem)
;

public sealed record FgsLeadDisqualificationReasonPatchDto(
    /// <summary>ReasonCode</summary>
    string? ReasonCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? ReasonName,
    /// <summary>Optional description explaining the reason.</summary>
    string? Description,
    /// <summary>Controls the order in which reasons are displayed in dropdowns and lists.</summary>
    short? DisplayOrder,
    /// <summary>Indicates whether the reason was seeded by the system or created by a user.</summary>
    bool? IsSystem)
;

