namespace Fgs.Setup.Application.Features.JobTypeCategories.Dtos;

public sealed record JobTypeCategorySummaryDto(
    long Id,
    long JobTypeId,
    long JobTypeTaskId,
    short? DisplayOrder,
    bool IsActive,
    string? Name = null,
    string? TaskName = null,
    string? TradeName = null,
    string? SkillName = null,
    decimal? EstimatedHours = null,
    short? Priority = null);

public sealed record JobTypeCategoryDetailDto(
    long Id,
    long JobTypeId,
    long JobTypeTaskId,
    short? DisplayOrder,
    bool IsActive,
    string? Name = null,
    string? TaskName = null,
    string? TradeName = null,
    string? SkillName = null,
    decimal? EstimatedHours = null,
    short? Priority = null);

public sealed record JobTypeCategoryLookupDto(
    long Id,
    long JobTypeId,
    long JobTypeTaskId,
    short? DisplayOrder,
    string? Name = null,
    string? TaskName = null,
    string? TradeName = null,
    string? SkillName = null,
    decimal? EstimatedHours = null,
    short? Priority = null);

public sealed record JobTypeCategoryCreateDto(
    long JobTypeId,
    long JobTypeTaskId,
    short? DisplayOrder);

public sealed record JobTypeCategoryUpdateDto(
    long JobTypeId,
    long JobTypeTaskId,
    short? DisplayOrder);

public sealed record JobTypeCategoryPatchDto(
    long? JobTypeId,
    long? JobTypeTaskId,
    short? DisplayOrder,
    bool? IsActive);

public sealed record JobTypeCategoryListFilters(
    long? JobTypeId = null,
    long? JobTypeTaskId = null);
