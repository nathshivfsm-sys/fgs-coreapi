namespace Fgs.Setup.Application.Features.SalesPipelineStatuses.Dtos;

public sealed record FgsSalesPipelineStatusSummaryDto(
    long Id,
    long TenantId,
    long CompanyId,
    string StatusCode,
    string StatusName,
    string? Description,
    short DisplayOrder,
    bool IsSystem,
    bool AppliesToLead,
    bool AppliesToOpportunity,
    bool IsTerminal,
    bool AllowManualSelection,
    bool IsActive,
    DateTimeOffset CreatedOn,
    DateTimeOffset? UpdatedOn);

public sealed record FgsSalesPipelineStatusDetailDto(
    long Id,
    long TenantId,
    long CompanyId,
    string StatusCode,
    string StatusName,
    string? Description,
    short DisplayOrder,
    bool IsSystem,
    bool AppliesToLead,
    bool AppliesToOpportunity,
    bool IsTerminal,
    bool AllowManualSelection,
    bool IsActive,
    DateTimeOffset CreatedOn,
    string? CreatedBy,
    DateTimeOffset? UpdatedOn,
    string? UpdatedBy);

public sealed record FgsSalesPipelineStatusLookupDto(
    long Id,
    string StatusCode,
    string StatusName,
    short DisplayOrder);

public sealed record FgsSalesPipelineStatusCreateDto(
    string StatusCode,
    string StatusName,
    string? Description,
    short DisplayOrder,
    bool IsSystem,
    bool AppliesToLead,
    bool AppliesToOpportunity,
    bool IsTerminal,
    bool AllowManualSelection);

public sealed record FgsSalesPipelineStatusUpdateDto(
    string StatusCode,
    string StatusName,
    string? Description,
    short DisplayOrder,
    bool IsSystem,
    bool AppliesToLead,
    bool AppliesToOpportunity,
    bool IsTerminal,
    bool AllowManualSelection);

public sealed record FgsSalesPipelineStatusPatchDto(
    string? StatusCode,
    string? StatusName,
    string? Description,
    short? DisplayOrder,
    bool? IsSystem,
    bool? AppliesToLead,
    bool? AppliesToOpportunity,
    bool? IsTerminal,
    bool? AllowManualSelection,
    bool? IsActive);

public sealed record FgsSalesPipelineStatusListFilters(
    string? StatusCode = null,
    string? StatusName = null);
