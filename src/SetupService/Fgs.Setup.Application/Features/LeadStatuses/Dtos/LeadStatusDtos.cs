namespace Fgs.Setup.Application.Features.LeadStatuses.Dtos;

public sealed record LeadStatusSummaryDto(
    long Id,
    string StatusCode,
    string StatusName,
    string? Description,
    short? DisplayOrder,
    bool IsSystem,
    bool IsActive);

public sealed record LeadStatusDetailDto(
    long Id,
    string StatusCode,
    string StatusName,
    string? Description,
    short? DisplayOrder,
    bool IsSystem,
    bool IsActive);

public sealed record LeadStatusLookupDto(
    long Id,
    string StatusCode,
    string StatusName,
    short? DisplayOrder);

public sealed record LeadStatusCreateDto(
    string StatusCode,
    string StatusName,
    string? Description,
    short? DisplayOrder,
    bool IsSystem);

public sealed record LeadStatusUpdateDto(
    string StatusCode,
    string StatusName,
    string? Description,
    short? DisplayOrder,
    bool IsSystem);

public sealed record LeadStatusPatchDto(
    string? StatusCode,
    string? StatusName,
    string? Description,
    short? DisplayOrder,
    bool? IsSystem,
    bool? IsActive);

public sealed record LeadStatusListFilters(
    string? StatusCode = null,
    string? StatusName = null);
