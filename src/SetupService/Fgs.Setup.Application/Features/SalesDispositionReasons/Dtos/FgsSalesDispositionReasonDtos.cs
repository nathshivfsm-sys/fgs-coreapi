namespace Fgs.Setup.Application.Features.SalesDispositionReasons.Dtos;

public sealed record FgsSalesDispositionReasonSummaryDto(
    long Id,
    long TenantId,
    long CompanyId,
    string DispositionReasonCode,
    string DispositionReasonName,
    string? Description,
    short DisplayOrder,
    bool IsSystem,
    bool AppliesToLead,
    bool AppliesToOpportunity,
    bool RequireComment,
    bool IsTerminal,
    bool AllowManualSelection,
    bool IsActive,
    DateTimeOffset CreatedOn,
    DateTimeOffset? UpdatedOn);

public sealed record FgsSalesDispositionReasonDetailDto(
    long Id,
    long TenantId,
    long CompanyId,
    string DispositionReasonCode,
    string DispositionReasonName,
    string? Description,
    short DisplayOrder,
    bool IsSystem,
    bool AppliesToLead,
    bool AppliesToOpportunity,
    bool RequireComment,
    bool IsTerminal,
    bool AllowManualSelection,
    bool IsActive,
    DateTimeOffset CreatedOn,
    string? CreatedBy,
    DateTimeOffset? UpdatedOn,
    string? UpdatedBy);

public sealed record FgsSalesDispositionReasonLookupDto(
    long Id,
    string DispositionReasonCode,
    string DispositionReasonName,
    short DisplayOrder);

public sealed record FgsSalesDispositionReasonCreateDto(
    string DispositionReasonCode,
    string DispositionReasonName,
    string? Description,
    short DisplayOrder,
    bool IsSystem,
    bool AppliesToLead,
    bool AppliesToOpportunity,
    bool RequireComment,
    bool IsTerminal,
    bool AllowManualSelection);

public sealed record FgsSalesDispositionReasonUpdateDto(
    string DispositionReasonCode,
    string DispositionReasonName,
    string? Description,
    short DisplayOrder,
    bool IsSystem,
    bool AppliesToLead,
    bool AppliesToOpportunity,
    bool RequireComment,
    bool IsTerminal,
    bool AllowManualSelection);

public sealed record FgsSalesDispositionReasonPatchDto(
    string? DispositionReasonCode,
    string? DispositionReasonName,
    string? Description,
    short? DisplayOrder,
    bool? IsSystem,
    bool? AppliesToLead,
    bool? AppliesToOpportunity,
    bool? RequireComment,
    bool? IsTerminal,
    bool? AllowManualSelection,
    bool? IsActive);

public sealed record FgsSalesDispositionReasonListFilters(
    string? DispositionReasonCode = null,
    string? DispositionReasonName = null);
