namespace Fgs.Setup.Application.Features.Sources.Dtos;

public sealed record SourcesummaryDto(
    long Id,
    string SourceCode,
    string SourceName,
    string? Description,
    bool IsActive);

public sealed record SourceDetailDto(
    long Id,
    string SourceCode,
    string SourceName,
    string? Description,
    bool IsActive);

public sealed record SourceLookupDto(
    long Id,
    string SourceCode,
    string SourceName);

public sealed record SourceCreateDto(
    string SourceCode,
    string SourceName,
    string? Description);

public sealed record SourceUpdateDto(
    string SourceCode,
    string SourceName,
    string? Description);

public sealed record SourcePatchDto(
    string? SourceCode,
    string? SourceName,
    string? Description,
    bool? IsActive);

public sealed record SourceListFilters(
    string? SourceCode = null,
    string? SourceName = null);
