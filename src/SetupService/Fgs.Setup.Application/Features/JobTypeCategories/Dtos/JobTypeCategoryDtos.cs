namespace Fgs.Setup.Application.Features.JobTypeCategories.Dtos;

public sealed record JobTypeCategorySummaryDto(
    long Id,
    long JobTypeId,
    long JobCategoryId,
    short? DisplayOrder,
    bool IsActive);

public sealed record JobTypeCategoryDetailDto(
    long Id,
    long JobTypeId,
    long JobCategoryId,
    short? DisplayOrder,
    bool IsActive);

public sealed record JobTypeCategoryLookupDto(
    long Id,
    long JobTypeId,
    long JobCategoryId,
    short? DisplayOrder);

public sealed record JobTypeCategoryCreateDto(
    long JobTypeId,
    long JobCategoryId,
    short? DisplayOrder);

public sealed record JobTypeCategoryUpdateDto(
    long JobTypeId,
    long JobCategoryId,
    short? DisplayOrder);

public sealed record JobTypeCategoryPatchDto(
    long? JobTypeId,
    long? JobCategoryId,
    short? DisplayOrder,
    bool? IsActive);

public sealed record JobTypeCategoryListFilters(
    long? JobTypeId = null,
    long? JobCategoryId = null);
