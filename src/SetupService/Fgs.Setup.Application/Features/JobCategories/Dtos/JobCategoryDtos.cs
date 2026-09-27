namespace Fgs.Setup.Application.Features.JobCategories.Dtos;

public sealed record JobCategorySummaryDto(
    long Id,
    string CategoryCode,
    string Name,
    string BackgroundColor,
    string TextColor,
    short? DisplayOrder,
    bool IsActive);

public sealed record JobCategoryDetailDto(
    long Id,
    string CategoryCode,
    string Name,
    string BackgroundColor,
    string TextColor,
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
    short? DisplayOrder,
    string? BackgroundColor = null,
    string? TextColor = null,
    bool IsActive = true);

public sealed record JobCategoryUpdateDto(
    string CategoryCode,
    string Name,
    short? DisplayOrder,
    string? BackgroundColor = null,
    string? TextColor = null);

public sealed record JobCategoryPatchDto(
    string? CategoryCode,
    string? Name,
    short? DisplayOrder,
    bool? IsActive,
    string? BackgroundColor = null,
    string? TextColor = null);

public sealed record JobCategoryListFilters(
    string? CategoryCode = null,
    string? Name = null);
