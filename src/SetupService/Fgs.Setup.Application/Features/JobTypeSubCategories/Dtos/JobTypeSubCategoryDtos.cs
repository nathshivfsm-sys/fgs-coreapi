namespace Fgs.Setup.Application.Features.JobTypeSubCategories.Dtos;

public sealed record JobTypeSubCategorySummaryDto(
    long Id,
    string SubCategoryCode,
    string Name,
    string? Description,
    short? DisplayOrder,
    bool IsActive);

public sealed record JobTypeSubCategoryDetailDto(
    long Id,
    string SubCategoryCode,
    string Name,
    string? Description,
    short? DisplayOrder);

public sealed record JobTypeSubCategoryLookupDto(
    long Id,
    string SubCategoryCode,
    string Name,
    short? DisplayOrder);

public sealed record JobTypeSubCategoryCreateDto(
    string SubCategoryCode,
    string Name,
    string? Description,
    short? DisplayOrder);

public sealed record JobTypeSubCategoryUpdateDto(
    string SubCategoryCode,
    string Name,
    string? Description,
    short? DisplayOrder);

public sealed record JobTypeSubCategoryPatchDto(
    string? SubCategoryCode,
    string? Name,
    string? Description,
    short? DisplayOrder,
    bool? IsActive);

public sealed record JobTypeSubCategoryListFilters(
    string? SubCategoryCode = null,
    string? Name = null);
