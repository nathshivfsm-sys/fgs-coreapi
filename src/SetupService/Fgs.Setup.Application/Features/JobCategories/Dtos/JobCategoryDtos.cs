namespace Fgs.Setup.Application.Features.JobCategories.Dtos;

public sealed record JobCategorySummaryDto(
    long Id,
    string CategoryCode,
    string Name,
    short? DisplayOrder,
    bool IsActive);

public sealed record JobCategoryDetailDto(
    long Id,
    string CategoryCode,
    string Name,
    short? DisplayOrder,
    bool IsActive);

public sealed record JobCategoryLookupDto(
    long Id,
    string CategoryCode,
    string Name,
    short? DisplayOrder);

public sealed record JobCategoryCreateDto(
    string CategoryCode,
    string Name,
    short? DisplayOrder);

public sealed record JobCategoryUpdateDto(
    string CategoryCode,
    string Name,
    short? DisplayOrder);

public sealed record JobCategoryPatchDto(
    string? CategoryCode,
    string? Name,
    short? DisplayOrder,
    bool? IsActive);

public sealed record JobCategoryListFilters(
    string? CategoryCode = null,
    string? Name = null);
