namespace Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;

public sealed record LeadDisqualificationReasonSummaryDto(
    long Id,
    string ReasonCode,
    string ReasonName,
    string? Description,
    short? DisplayOrder,
    bool IsSystem,
    bool IsActive);

public sealed record LeadDisqualificationReasonDetailDto(
    long Id,
    string ReasonCode,
    string ReasonName,
    string? Description,
    short? DisplayOrder,
    bool IsSystem,
    bool IsActive);

public sealed record LeadDisqualificationReasonLookupDto(
    long Id,
    string ReasonCode,
    string ReasonName,
    short? DisplayOrder);

public sealed record LeadDisqualificationReasonCreateDto(
    string ReasonCode,
    string ReasonName,
    string? Description,
    short? DisplayOrder,
    bool IsSystem);

public sealed record LeadDisqualificationReasonUpdateDto(
    string ReasonCode,
    string ReasonName,
    string? Description,
    short? DisplayOrder,
    bool IsSystem);

public sealed record LeadDisqualificationReasonPatchDto(
    string? ReasonCode,
    string? ReasonName,
    string? Description,
    short? DisplayOrder,
    bool? IsSystem,
    bool? IsActive);

public sealed record LeadDisqualificationReasonListFilters(
    string? ReasonCode = null,
    string? ReasonName = null);
