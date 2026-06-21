namespace Fgs.Setup.Application.Features.TitlesOfCourtesy.Dtos;

public sealed record TitleOfCourtesySummaryDto(
    long Id,
    long TenantId,
    long CompanyId,
    string Code,
    string DisplayName,
    int? SortOrder,
    bool IsActive,
    DateTimeOffset CreatedOn,
    DateTimeOffset? UpdatedOn);

public sealed record TitleOfCourtesyDetailDto(
    long Id,
    long TenantId,
    long CompanyId,
    string Code,
    string DisplayName,
    int? SortOrder,
    bool IsActive,
    DateTimeOffset CreatedOn,
    string? CreatedBy,
    DateTimeOffset? UpdatedOn,
    string? UpdatedBy);

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
