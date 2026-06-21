namespace Fgs.Setup.Application.Features.JobTypeCategories.Dtos;

public sealed record JobTypeCategorySummaryDto(
    long Id,
    long TenantId,
    long CompanyId,
    string CategoryCode,
    string Name,
    string? Description,
    short? DisplayOrder,
    bool IsActive,
    DateTimeOffset CreatedOn,
    DateTimeOffset? UpdatedOn);

public sealed record JobTypeCategoryDetailDto(
    long Id,
    long TenantId,
    long CompanyId,
    string CategoryCode,
    string Name,
    string? Description,
    short? DisplayOrder,
    bool IsActive,
    DateTimeOffset CreatedOn,
    string? CreatedBy,
    DateTimeOffset? UpdatedOn,
    string? UpdatedBy);

public sealed record JobTypeCategoryLookupDto(
    long Id,
    string CategoryCode,
    string Name,
    short? DisplayOrder);

public sealed record JobTypeCategoryCreateDto(
    string CategoryCode,
    string Name,
    string? Description,
    short? DisplayOrder);

public sealed record JobTypeCategoryUpdateDto(
    string CategoryCode,
    string Name,
    string? Description,
    short? DisplayOrder);

public sealed record JobTypeCategoryPatchDto(
    string? CategoryCode,
    string? Name,
    string? Description,
    short? DisplayOrder,
    bool? IsActive);

public sealed record JobTypeCategoryListFilters(
    string? CategoryCode = null,
    string? Name = null);
