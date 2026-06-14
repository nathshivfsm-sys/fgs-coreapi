namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSalesDispositionReason</summary>
public sealed record FgsSalesDispositionReasonSummaryDto(
    /// <summary>Unique identifier for the sales disposition reason.</summary>
    long Id,
    /// <summary>Tenant identifier that owns the record.</summary>
    long TenantId,
    /// <summary>Company identifier that owns the record.</summary>
    long CompanyId,
    /// <summary>Immutable business code for the disposition reason.</summary>
    string? DispositionReasonCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? DispositionReasonName,
    /// <summary>Optional description explaining the disposition reason.</summary>
    string? Description,
    /// <summary>Indicates whether the reason can be used when a Lead is Disqualified.</summary>
    bool AppliesToLead,
    /// <summary>Indicates whether the reason can be used when an Opportunity is Lost.</summary>
    bool AppliesToOpportunity,
    /// <summary>Indicates whether users must provide additional comments when selecting this disposition reason.</summary>
    bool RequireComment,
    /// <summary>Indicates whether selecting this disposition reason should result in a terminal pipeline status.</summary>
    bool IsTerminal,
    /// <summary>Indicates whether users may manually select this disposition reason.</summary>
    bool AllowManualSelection,
    /// <summary>Controls the order in which disposition reasons are displayed.</summary>
    short DisplayOrder,
    /// <summary>Indicates whether the disposition reason was seeded by the system. System records should have immutable business codes.</summary>
    bool IsSystem,
    /// <summary>Date and time the record was created.</summary>
    DateTimeOffset CreatedOn,
    /// <summary>Date and time the record was last updated.</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>Indicates whether the disposition reason is available for use.</summary>
    bool IsActive)
;

public sealed record FgsSalesDispositionReasonDetailDto(
    /// <summary>Unique identifier for the sales disposition reason.</summary>
    long Id,
    /// <summary>Tenant identifier that owns the record.</summary>
    long TenantId,
    /// <summary>Company identifier that owns the record.</summary>
    long CompanyId,
    /// <summary>Immutable business code for the disposition reason.</summary>
    string? DispositionReasonCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? DispositionReasonName,
    /// <summary>Optional description explaining the disposition reason.</summary>
    string? Description,
    /// <summary>Indicates whether the reason can be used when a Lead is Disqualified.</summary>
    bool AppliesToLead,
    /// <summary>Indicates whether the reason can be used when an Opportunity is Lost.</summary>
    bool AppliesToOpportunity,
    /// <summary>Indicates whether users must provide additional comments when selecting this disposition reason.</summary>
    bool RequireComment,
    /// <summary>Indicates whether selecting this disposition reason should result in a terminal pipeline status.</summary>
    bool IsTerminal,
    /// <summary>Indicates whether users may manually select this disposition reason.</summary>
    bool AllowManualSelection,
    /// <summary>Controls the order in which disposition reasons are displayed.</summary>
    short DisplayOrder,
    /// <summary>Indicates whether the disposition reason was seeded by the system. System records should have immutable business codes.</summary>
    bool IsSystem,
    /// <summary>Date and time the record was created.</summary>
    DateTimeOffset CreatedOn,
    /// <summary>User who created the record.</summary>
    string? CreatedBy,
    /// <summary>Date and time the record was last updated.</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>User who last updated the record.</summary>
    string? UpdatedBy,
    /// <summary>Indicates whether the disposition reason is available for use.</summary>
    bool IsActive)
;

public sealed record FgsSalesDispositionReasonCreateDto(
    /// <summary>Immutable business code for the disposition reason.</summary>
    string? DispositionReasonCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? DispositionReasonName,
    /// <summary>Optional description explaining the disposition reason.</summary>
    string? Description,
    /// <summary>Indicates whether the reason can be used when a Lead is Disqualified.</summary>
    bool AppliesToLead,
    /// <summary>Indicates whether the reason can be used when an Opportunity is Lost.</summary>
    bool AppliesToOpportunity,
    /// <summary>Indicates whether users must provide additional comments when selecting this disposition reason.</summary>
    bool RequireComment,
    /// <summary>Indicates whether selecting this disposition reason should result in a terminal pipeline status.</summary>
    bool IsTerminal,
    /// <summary>Indicates whether users may manually select this disposition reason.</summary>
    bool AllowManualSelection,
    /// <summary>Controls the order in which disposition reasons are displayed.</summary>
    short DisplayOrder,
    /// <summary>Indicates whether the disposition reason was seeded by the system. System records should have immutable business codes.</summary>
    bool IsSystem)
;

public sealed record FgsSalesDispositionReasonUpdateDto(
    /// <summary>Immutable business code for the disposition reason.</summary>
    string? DispositionReasonCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? DispositionReasonName,
    /// <summary>Optional description explaining the disposition reason.</summary>
    string? Description,
    /// <summary>Indicates whether the reason can be used when a Lead is Disqualified.</summary>
    bool AppliesToLead,
    /// <summary>Indicates whether the reason can be used when an Opportunity is Lost.</summary>
    bool AppliesToOpportunity,
    /// <summary>Indicates whether users must provide additional comments when selecting this disposition reason.</summary>
    bool RequireComment,
    /// <summary>Indicates whether selecting this disposition reason should result in a terminal pipeline status.</summary>
    bool IsTerminal,
    /// <summary>Indicates whether users may manually select this disposition reason.</summary>
    bool AllowManualSelection,
    /// <summary>Controls the order in which disposition reasons are displayed.</summary>
    short DisplayOrder,
    /// <summary>Indicates whether the disposition reason was seeded by the system. System records should have immutable business codes.</summary>
    bool IsSystem)
;

public sealed record FgsSalesDispositionReasonPatchDto(
    /// <summary>Immutable business code for the disposition reason.</summary>
    string? DispositionReasonCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? DispositionReasonName,
    /// <summary>Optional description explaining the disposition reason.</summary>
    string? Description,
    /// <summary>Indicates whether the reason can be used when a Lead is Disqualified.</summary>
    bool? AppliesToLead,
    /// <summary>Indicates whether the reason can be used when an Opportunity is Lost.</summary>
    bool? AppliesToOpportunity,
    /// <summary>Indicates whether users must provide additional comments when selecting this disposition reason.</summary>
    bool? RequireComment,
    /// <summary>Indicates whether selecting this disposition reason should result in a terminal pipeline status.</summary>
    bool? IsTerminal,
    /// <summary>Indicates whether users may manually select this disposition reason.</summary>
    bool? AllowManualSelection,
    /// <summary>Controls the order in which disposition reasons are displayed.</summary>
    short? DisplayOrder,
    /// <summary>Indicates whether the disposition reason was seeded by the system. System records should have immutable business codes.</summary>
    bool? IsSystem)
;

