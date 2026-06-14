namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSalesPipelineStatus</summary>
public sealed record FgsSalesPipelineStatusSummaryDto(
    /// <summary>Unique identifier for the sales pipeline status.</summary>
    long Id,
    /// <summary>Tenant identifier that owns the record.</summary>
    long TenantId,
    /// <summary>Company identifier that owns the record.</summary>
    long CompanyId,
    /// <summary>Immutable business code for the sales pipeline status.</summary>
    string? StatusCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? StatusName,
    /// <summary>Optional description explaining the purpose of the status.</summary>
    string? Description,
    /// <summary>Indicates whether the status can be used by Leads.</summary>
    bool AppliesToLead,
    /// <summary>Indicates whether the status can be used by Opportunities.</summary>
    bool AppliesToOpportunity,
    /// <summary>Indicates whether the status represents a terminal end state such as Won, Lost, or Disqualified.</summary>
    bool IsTerminal,
    /// <summary>Indicates whether users may manually select this status.</summary>
    bool AllowManualSelection,
    /// <summary>Controls the order in which statuses are displayed.</summary>
    short DisplayOrder,
    /// <summary>Indicates whether the status was seeded by the system. System records should have immutable business codes.</summary>
    bool IsSystem,
    /// <summary>Date and time the record was created.</summary>
    DateTimeOffset CreatedOn,
    /// <summary>Date and time the record was last updated.</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>Indicates whether the status is available for use.</summary>
    bool IsActive)
;

public sealed record FgsSalesPipelineStatusDetailDto(
    /// <summary>Unique identifier for the sales pipeline status.</summary>
    long Id,
    /// <summary>Tenant identifier that owns the record.</summary>
    long TenantId,
    /// <summary>Company identifier that owns the record.</summary>
    long CompanyId,
    /// <summary>Immutable business code for the sales pipeline status.</summary>
    string? StatusCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? StatusName,
    /// <summary>Optional description explaining the purpose of the status.</summary>
    string? Description,
    /// <summary>Indicates whether the status can be used by Leads.</summary>
    bool AppliesToLead,
    /// <summary>Indicates whether the status can be used by Opportunities.</summary>
    bool AppliesToOpportunity,
    /// <summary>Indicates whether the status represents a terminal end state such as Won, Lost, or Disqualified.</summary>
    bool IsTerminal,
    /// <summary>Indicates whether users may manually select this status.</summary>
    bool AllowManualSelection,
    /// <summary>Controls the order in which statuses are displayed.</summary>
    short DisplayOrder,
    /// <summary>Indicates whether the status was seeded by the system. System records should have immutable business codes.</summary>
    bool IsSystem,
    /// <summary>Date and time the record was created.</summary>
    DateTimeOffset CreatedOn,
    /// <summary>User who created the record.</summary>
    string? CreatedBy,
    /// <summary>Date and time the record was last updated.</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>User who last updated the record.</summary>
    string? UpdatedBy,
    /// <summary>Indicates whether the status is available for use.</summary>
    bool IsActive)
;

public sealed record FgsSalesPipelineStatusCreateDto(
    /// <summary>Immutable business code for the sales pipeline status.</summary>
    string? StatusCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? StatusName,
    /// <summary>Optional description explaining the purpose of the status.</summary>
    string? Description,
    /// <summary>Indicates whether the status can be used by Leads.</summary>
    bool AppliesToLead,
    /// <summary>Indicates whether the status can be used by Opportunities.</summary>
    bool AppliesToOpportunity,
    /// <summary>Indicates whether the status represents a terminal end state such as Won, Lost, or Disqualified.</summary>
    bool IsTerminal,
    /// <summary>Indicates whether users may manually select this status.</summary>
    bool AllowManualSelection,
    /// <summary>Controls the order in which statuses are displayed.</summary>
    short DisplayOrder,
    /// <summary>Indicates whether the status was seeded by the system. System records should have immutable business codes.</summary>
    bool IsSystem)
;

public sealed record FgsSalesPipelineStatusUpdateDto(
    /// <summary>Immutable business code for the sales pipeline status.</summary>
    string? StatusCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? StatusName,
    /// <summary>Optional description explaining the purpose of the status.</summary>
    string? Description,
    /// <summary>Indicates whether the status can be used by Leads.</summary>
    bool AppliesToLead,
    /// <summary>Indicates whether the status can be used by Opportunities.</summary>
    bool AppliesToOpportunity,
    /// <summary>Indicates whether the status represents a terminal end state such as Won, Lost, or Disqualified.</summary>
    bool IsTerminal,
    /// <summary>Indicates whether users may manually select this status.</summary>
    bool AllowManualSelection,
    /// <summary>Controls the order in which statuses are displayed.</summary>
    short DisplayOrder,
    /// <summary>Indicates whether the status was seeded by the system. System records should have immutable business codes.</summary>
    bool IsSystem)
;

public sealed record FgsSalesPipelineStatusPatchDto(
    /// <summary>Immutable business code for the sales pipeline status.</summary>
    string? StatusCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? StatusName,
    /// <summary>Optional description explaining the purpose of the status.</summary>
    string? Description,
    /// <summary>Indicates whether the status can be used by Leads.</summary>
    bool? AppliesToLead,
    /// <summary>Indicates whether the status can be used by Opportunities.</summary>
    bool? AppliesToOpportunity,
    /// <summary>Indicates whether the status represents a terminal end state such as Won, Lost, or Disqualified.</summary>
    bool? IsTerminal,
    /// <summary>Indicates whether users may manually select this status.</summary>
    bool? AllowManualSelection,
    /// <summary>Controls the order in which statuses are displayed.</summary>
    short? DisplayOrder,
    /// <summary>Indicates whether the status was seeded by the system. System records should have immutable business codes.</summary>
    bool? IsSystem)
;

