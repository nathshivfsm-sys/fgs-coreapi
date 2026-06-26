namespace Fgs.Setup.Application.Features.LeadSources.Dtos;

public sealed record LeadSourceSummaryDto(
    long Id,
    string SourceCode,
    string SourceName,
    string? Description,
    bool IsActive);

public sealed record LeadSourceDetailDto(
    long Id,
    string SourceCode,
    string SourceName,
    string? Description,
    bool IsActive);

public sealed record LeadSourceLookupDto(
    long Id,
    string SourceCode,
    string SourceName);

public sealed record LeadSourceCreateDto(
    string SourceCode,
    string SourceName,
    string? Description);

public sealed record LeadSourceUpdateDto(
    string SourceCode,
    string SourceName,
    string? Description);

public sealed record LeadSourcePatchDto(
    string? SourceCode,
    string? SourceName,
    string? Description,
    bool? IsActive);

public sealed record LeadSourceListFilters(
    string? SourceCode = null,
    string? SourceName = null);
