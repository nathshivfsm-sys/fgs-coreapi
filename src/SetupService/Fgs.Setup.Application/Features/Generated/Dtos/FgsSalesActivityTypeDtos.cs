namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSalesActivityType</summary>
public sealed record FgsSalesActivityTypeSummaryDto(
    /// <summary>Unique identifier for the sales activity type.</summary>
    long Id,
    /// <summary>Tenant identifier that owns the record.</summary>
    long TenantId,
    /// <summary>Company identifier that owns the record.</summary>
    long CompanyId,
    /// <summary>Immutable business code for the sales activity type.</summary>
    string? ActivityTypeCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? ActivityTypeName,
    /// <summary>Optional description explaining the sales activity type.</summary>
    string? Description,
    /// <summary>Indicates whether the activity type can be used by Leads.</summary>
    bool AppliesToLead,
    /// <summary>Indicates whether the activity type can be used by Opportunities.</summary>
    bool AppliesToOpportunity,
    /// <summary>Indicates whether users may manually select this activity type.</summary>
    bool AllowManualSelection,
    /// <summary>Controls the order in which activity types are displayed.</summary>
    short DisplayOrder,
    /// <summary>Indicates whether the activity type was seeded by the system. System records should have immutable business codes.</summary>
    bool IsSystem,
    /// <summary>Date and time the record was created.</summary>
    DateTimeOffset CreatedOn,
    /// <summary>Date and time the record was last updated.</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>Indicates whether the activity type is available for use.</summary>
    bool IsActive)
;

public sealed record FgsSalesActivityTypeDetailDto(
    /// <summary>Unique identifier for the sales activity type.</summary>
    long Id,
    /// <summary>Tenant identifier that owns the record.</summary>
    long TenantId,
    /// <summary>Company identifier that owns the record.</summary>
    long CompanyId,
    /// <summary>Immutable business code for the sales activity type.</summary>
    string? ActivityTypeCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? ActivityTypeName,
    /// <summary>Optional description explaining the sales activity type.</summary>
    string? Description,
    /// <summary>Indicates whether the activity type can be used by Leads.</summary>
    bool AppliesToLead,
    /// <summary>Indicates whether the activity type can be used by Opportunities.</summary>
    bool AppliesToOpportunity,
    /// <summary>Indicates whether users may manually select this activity type.</summary>
    bool AllowManualSelection,
    /// <summary>Controls the order in which activity types are displayed.</summary>
    short DisplayOrder,
    /// <summary>Indicates whether the activity type was seeded by the system. System records should have immutable business codes.</summary>
    bool IsSystem,
    /// <summary>Date and time the record was created.</summary>
    DateTimeOffset CreatedOn,
    /// <summary>User who created the record.</summary>
    string? CreatedBy,
    /// <summary>Date and time the record was last updated.</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>User who last updated the record.</summary>
    string? UpdatedBy,
    /// <summary>Indicates whether the activity type is available for use.</summary>
    bool IsActive)
;

public sealed record FgsSalesActivityTypeCreateDto(
    /// <summary>Immutable business code for the sales activity type.</summary>
    string? ActivityTypeCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? ActivityTypeName,
    /// <summary>Optional description explaining the sales activity type.</summary>
    string? Description,
    /// <summary>Indicates whether the activity type can be used by Leads.</summary>
    bool AppliesToLead,
    /// <summary>Indicates whether the activity type can be used by Opportunities.</summary>
    bool AppliesToOpportunity,
    /// <summary>Indicates whether users may manually select this activity type.</summary>
    bool AllowManualSelection,
    /// <summary>Controls the order in which activity types are displayed.</summary>
    short DisplayOrder,
    /// <summary>Indicates whether the activity type was seeded by the system. System records should have immutable business codes.</summary>
    bool IsSystem)
;

public sealed record FgsSalesActivityTypeUpdateDto(
    /// <summary>Immutable business code for the sales activity type.</summary>
    string? ActivityTypeCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? ActivityTypeName,
    /// <summary>Optional description explaining the sales activity type.</summary>
    string? Description,
    /// <summary>Indicates whether the activity type can be used by Leads.</summary>
    bool AppliesToLead,
    /// <summary>Indicates whether the activity type can be used by Opportunities.</summary>
    bool AppliesToOpportunity,
    /// <summary>Indicates whether users may manually select this activity type.</summary>
    bool AllowManualSelection,
    /// <summary>Controls the order in which activity types are displayed.</summary>
    short DisplayOrder,
    /// <summary>Indicates whether the activity type was seeded by the system. System records should have immutable business codes.</summary>
    bool IsSystem)
;

public sealed record FgsSalesActivityTypePatchDto(
    /// <summary>Immutable business code for the sales activity type.</summary>
    string? ActivityTypeCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? ActivityTypeName,
    /// <summary>Optional description explaining the sales activity type.</summary>
    string? Description,
    /// <summary>Indicates whether the activity type can be used by Leads.</summary>
    bool? AppliesToLead,
    /// <summary>Indicates whether the activity type can be used by Opportunities.</summary>
    bool? AppliesToOpportunity,
    /// <summary>Indicates whether users may manually select this activity type.</summary>
    bool? AllowManualSelection,
    /// <summary>Controls the order in which activity types are displayed.</summary>
    short? DisplayOrder,
    /// <summary>Indicates whether the activity type was seeded by the system. System records should have immutable business codes.</summary>
    bool? IsSystem)
;

