namespace Fgs.Setup.Application.Features.JobTypeTasks.Dtos;

public sealed record JobTypeTaskSummaryDto(
    long Id,
    long JobCategoryId,
    long TradeId,
    long? SkillLevelId,
    string Name,
    string TaskName,
    short Priority,
    decimal EstimatedHours,
    short? DisplayOrder,
    bool IsActive,
    string? CategoryName = null,
    string? TradeName = null,
    string? SkillName = null);

public sealed record JobTypeTaskDetailDto(
    long Id,
    long JobCategoryId,
    long TradeId,
    long? SkillLevelId,
    string Name,
    string TaskName,
    short Priority,
    decimal EstimatedHours,
    short? DisplayOrder,
    bool IsActive,
    string? CategoryName = null,
    string? TradeName = null,
    string? SkillName = null);

public sealed record JobTypeTaskLookupDto(
    long Id,
    string Name);

public sealed record JobTypeTaskCreateDto(
    long JobCategoryId,
    long TradeId,
    string Name,
    short Priority,
    decimal EstimatedHours,
    short? DisplayOrder = null,
    string? TaskName = null,
    long? SkillLevelId = null,
    bool? IsActive = null);

public sealed record JobTypeTaskUpdateDto(
    long JobCategoryId,
    long TradeId,
    string Name,
    short Priority,
    decimal EstimatedHours,
    short? DisplayOrder = null,
    string? TaskName = null,
    long? SkillLevelId = null);

public sealed record JobTypeTaskPatchDto(
    long? JobCategoryId,
    long? TradeId,
    string? Name,
    string? TaskName,
    short? Priority,
    decimal? EstimatedHours,
    short? DisplayOrder,
    bool? IsActive,
    long? SkillLevelId = null);

public sealed record JobTypeTaskListFilters(
    string? TaskName = null,
    string? Name = null,
    long? JobCategoryId = null,
    long? JobTypeId = null);
