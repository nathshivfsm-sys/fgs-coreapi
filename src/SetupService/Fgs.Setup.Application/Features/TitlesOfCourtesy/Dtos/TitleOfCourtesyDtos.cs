namespace Fgs.Setup.Application.Features.TitlesOfCourtesy.Dtos;

public sealed record TitleOfCourtesySummaryDto(
    long Id,
    string Code,
    string DisplayName,
    int? SortOrder,
    bool IsActive);

public sealed record TitleOfCourtesyDetailDto(
    long Id,
    string Code,
    string DisplayName,
    int? SortOrder,
    bool IsActive);

public sealed record TitleOfCourtesyLookupDto(
    long Id,
    string Code,
    string DisplayName,
    int? SortOrder);

public sealed record TitleOfCourtesyCreateDto(
    string Code,
    string DisplayName,
    int? SortOrder);

public sealed record TitleOfCourtesyUpdateDto(
    string Code,
    string DisplayName,
    int? SortOrder);

public sealed record TitleOfCourtesyPatchDto(
    string? Code,
    string? DisplayName,
    int? SortOrder,
    bool? IsActive);

public sealed record TitleOfCourtesyListFilters(
    string? Code = null,
    string? DisplayName = null);
