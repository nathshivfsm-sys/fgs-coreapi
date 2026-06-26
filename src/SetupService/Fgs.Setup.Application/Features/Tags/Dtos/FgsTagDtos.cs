namespace Fgs.Setup.Application.Features.Tags.Dtos;

public sealed record FgsTagSummaryDto(
    long Id,
    string? TagCode,
    string Name,
    string? Description,
    string? BackgroundColor,
    string? TextColor,
    long? IconFileId,
    int UsageCount,
    bool IsActive);

public sealed record FgsTagDetailDto(
    long Id,
    string? TagCode,
    string Name,
    string? Description,
    string? BackgroundColor,
    string? TextColor,
    long? IconFileId,
    int UsageCount,
    bool IsActive);

public sealed record FgsTagLookupDto(
    long Id,
    string? TagCode,
    string Name);

public sealed record FgsTagCreateDto(
    string? TagCode,
    string Name,
    string? Description,
    string? BackgroundColor,
    string? TextColor,
    long? IconFileId);

public sealed record FgsTagUpdateDto(
    string? TagCode,
    string Name,
    string? Description,
    string? BackgroundColor,
    string? TextColor,
    long? IconFileId);

public sealed record FgsTagPatchDto(
    string? TagCode,
    string? Name,
    string? Description,
    string? BackgroundColor,
    string? TextColor,
    long? IconFileId,
    bool? IsActive);

public sealed record FgsTagListFilters(
    string? TagCode = null,
    string? Name = null);
