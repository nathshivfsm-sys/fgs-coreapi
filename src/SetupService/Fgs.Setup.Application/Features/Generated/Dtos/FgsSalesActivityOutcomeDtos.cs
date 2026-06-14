namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSalesActivityOutcome</summary>
public sealed record FgsSalesActivityOutcomeSummaryDto(
    /// <summary>Unique identifier for the sales activity outcome.</summary>
    long Id,
    /// <summary>Tenant identifier that owns the record.</summary>
    long TenantId,
    /// <summary>Company identifier that owns the record.</summary>
    long CompanyId,
    /// <summary>Immutable business code for the sales activity outcome.</summary>
    string? OutcomeCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? OutcomeName,
    /// <summary>Optional description explaining the sales activity outcome.</summary>
    string? Description,
    /// <summary>Indicates whether the outcome can be used by Leads.</summary>
    bool AppliesToLead,
    /// <summary>Indicates whether the outcome can be used by Opportunities.</summary>
    bool AppliesToOpportunity,
    /// <summary>Suggested sales pipeline status that should be applied when this outcome is selected.</summary>
    long? NextSalesPipelineStatusId,
    /// <summary>Indicates whether selecting this outcome typically results in a terminal sales pipeline status.</summary>
    bool IsTerminal,
    /// <summary>Indicates whether users must provide additional comments when selecting this outcome.</summary>
    bool RequireComment,
    /// <summary>Indicates whether users may manually select this outcome.</summary>
    bool AllowManualSelection,
    /// <summary>Controls the order in which outcomes are displayed.</summary>
    short DisplayOrder,
    /// <summary>Indicates whether the outcome was seeded by the system. System records should have immutable business codes.</summary>
    bool IsSystem,
    /// <summary>Date and time the record was created.</summary>
    DateTimeOffset CreatedOn,
    /// <summary>Date and time the record was last updated.</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>Indicates whether the outcome is available for use.</summary>
    bool IsActive)
;

public sealed record FgsSalesActivityOutcomeDetailDto(
    /// <summary>Unique identifier for the sales activity outcome.</summary>
    long Id,
    /// <summary>Tenant identifier that owns the record.</summary>
    long TenantId,
    /// <summary>Company identifier that owns the record.</summary>
    long CompanyId,
    /// <summary>Immutable business code for the sales activity outcome.</summary>
    string? OutcomeCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? OutcomeName,
    /// <summary>Optional description explaining the sales activity outcome.</summary>
    string? Description,
    /// <summary>Indicates whether the outcome can be used by Leads.</summary>
    bool AppliesToLead,
    /// <summary>Indicates whether the outcome can be used by Opportunities.</summary>
    bool AppliesToOpportunity,
    /// <summary>Suggested sales pipeline status that should be applied when this outcome is selected.</summary>
    long? NextSalesPipelineStatusId,
    /// <summary>Indicates whether selecting this outcome typically results in a terminal sales pipeline status.</summary>
    bool IsTerminal,
    /// <summary>Indicates whether users must provide additional comments when selecting this outcome.</summary>
    bool RequireComment,
    /// <summary>Indicates whether users may manually select this outcome.</summary>
    bool AllowManualSelection,
    /// <summary>Controls the order in which outcomes are displayed.</summary>
    short DisplayOrder,
    /// <summary>Indicates whether the outcome was seeded by the system. System records should have immutable business codes.</summary>
    bool IsSystem,
    /// <summary>Date and time the record was created.</summary>
    DateTimeOffset CreatedOn,
    /// <summary>User who created the record.</summary>
    string? CreatedBy,
    /// <summary>Date and time the record was last updated.</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>User who last updated the record.</summary>
    string? UpdatedBy,
    /// <summary>Indicates whether the outcome is available for use.</summary>
    bool IsActive)
;

public sealed record FgsSalesActivityOutcomeCreateDto(
    /// <summary>Immutable business code for the sales activity outcome.</summary>
    string? OutcomeCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? OutcomeName,
    /// <summary>Optional description explaining the sales activity outcome.</summary>
    string? Description,
    /// <summary>Indicates whether the outcome can be used by Leads.</summary>
    bool AppliesToLead,
    /// <summary>Indicates whether the outcome can be used by Opportunities.</summary>
    bool AppliesToOpportunity,
    /// <summary>Suggested sales pipeline status that should be applied when this outcome is selected.</summary>
    long? NextSalesPipelineStatusId,
    /// <summary>Indicates whether selecting this outcome typically results in a terminal sales pipeline status.</summary>
    bool IsTerminal,
    /// <summary>Indicates whether users must provide additional comments when selecting this outcome.</summary>
    bool RequireComment,
    /// <summary>Indicates whether users may manually select this outcome.</summary>
    bool AllowManualSelection,
    /// <summary>Controls the order in which outcomes are displayed.</summary>
    short DisplayOrder,
    /// <summary>Indicates whether the outcome was seeded by the system. System records should have immutable business codes.</summary>
    bool IsSystem)
;

public sealed record FgsSalesActivityOutcomeUpdateDto(
    /// <summary>Immutable business code for the sales activity outcome.</summary>
    string? OutcomeCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? OutcomeName,
    /// <summary>Optional description explaining the sales activity outcome.</summary>
    string? Description,
    /// <summary>Indicates whether the outcome can be used by Leads.</summary>
    bool AppliesToLead,
    /// <summary>Indicates whether the outcome can be used by Opportunities.</summary>
    bool AppliesToOpportunity,
    /// <summary>Suggested sales pipeline status that should be applied when this outcome is selected.</summary>
    long? NextSalesPipelineStatusId,
    /// <summary>Indicates whether selecting this outcome typically results in a terminal sales pipeline status.</summary>
    bool IsTerminal,
    /// <summary>Indicates whether users must provide additional comments when selecting this outcome.</summary>
    bool RequireComment,
    /// <summary>Indicates whether users may manually select this outcome.</summary>
    bool AllowManualSelection,
    /// <summary>Controls the order in which outcomes are displayed.</summary>
    short DisplayOrder,
    /// <summary>Indicates whether the outcome was seeded by the system. System records should have immutable business codes.</summary>
    bool IsSystem)
;

public sealed record FgsSalesActivityOutcomePatchDto(
    /// <summary>Immutable business code for the sales activity outcome.</summary>
    string? OutcomeCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? OutcomeName,
    /// <summary>Optional description explaining the sales activity outcome.</summary>
    string? Description,
    /// <summary>Indicates whether the outcome can be used by Leads.</summary>
    bool? AppliesToLead,
    /// <summary>Indicates whether the outcome can be used by Opportunities.</summary>
    bool? AppliesToOpportunity,
    /// <summary>Suggested sales pipeline status that should be applied when this outcome is selected.</summary>
    long? NextSalesPipelineStatusId,
    /// <summary>Indicates whether selecting this outcome typically results in a terminal sales pipeline status.</summary>
    bool? IsTerminal,
    /// <summary>Indicates whether users must provide additional comments when selecting this outcome.</summary>
    bool? RequireComment,
    /// <summary>Indicates whether users may manually select this outcome.</summary>
    bool? AllowManualSelection,
    /// <summary>Controls the order in which outcomes are displayed.</summary>
    short? DisplayOrder,
    /// <summary>Indicates whether the outcome was seeded by the system. System records should have immutable business codes.</summary>
    bool? IsSystem)
;

