namespace Fgs.Setup.Application.Features.SalesActivityOutcomes.Dtos;

public sealed record FgsSalesActivityOutcomeSummaryDto(
    long Id,
    long TenantId,
    long CompanyId,
    string OutcomeCode,
    string OutcomeName,
    string? Description,
    short DisplayOrder,
    bool IsSystem,
    bool AppliesToLead,
    bool AppliesToOpportunity,
    long? NextSalesPipelineStatusId,
    bool IsTerminal,
    bool RequireComment,
    bool AllowManualSelection,
    bool IsActive,
    DateTimeOffset CreatedOn,
    DateTimeOffset? UpdatedOn);

public sealed record FgsSalesActivityOutcomeDetailDto(
    long Id,
    long TenantId,
    long CompanyId,
    string OutcomeCode,
    string OutcomeName,
    string? Description,
    short DisplayOrder,
    bool IsSystem,
    bool AppliesToLead,
    bool AppliesToOpportunity,
    long? NextSalesPipelineStatusId,
    bool IsTerminal,
    bool RequireComment,
    bool AllowManualSelection,
    bool IsActive,
    DateTimeOffset CreatedOn,
    string? CreatedBy,
    DateTimeOffset? UpdatedOn,
    string? UpdatedBy);

public sealed record FgsSalesActivityOutcomeLookupDto(
    long Id,
    string OutcomeCode,
    string OutcomeName,
    short DisplayOrder);

public sealed record FgsSalesActivityOutcomeCreateDto(
    string OutcomeCode,
    string OutcomeName,
    string? Description,
    short DisplayOrder,
    bool IsSystem,
    bool AppliesToLead,
    bool AppliesToOpportunity,
    long? NextSalesPipelineStatusId,
    bool IsTerminal,
    bool RequireComment,
    bool AllowManualSelection);

public sealed record FgsSalesActivityOutcomeUpdateDto(
    string OutcomeCode,
    string OutcomeName,
    string? Description,
    short DisplayOrder,
    bool IsSystem,
    bool AppliesToLead,
    bool AppliesToOpportunity,
    long? NextSalesPipelineStatusId,
    bool IsTerminal,
    bool RequireComment,
    bool AllowManualSelection);

public sealed record FgsSalesActivityOutcomePatchDto(
    string? OutcomeCode,
    string? OutcomeName,
    string? Description,
    short? DisplayOrder,
    bool? IsSystem,
    bool? AppliesToLead,
    bool? AppliesToOpportunity,
    long? NextSalesPipelineStatusId,
    bool? IsTerminal,
    bool? RequireComment,
    bool? AllowManualSelection,
    bool? IsActive);

public sealed record FgsSalesActivityOutcomeListFilters(
    string? OutcomeCode = null,
    string? OutcomeName = null);
