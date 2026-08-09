namespace Fgs.Setup.Application.Features.JobTypeTasks.Dtos;

public sealed record JobTypeTaskSummaryDto(
    long Id,
    long JobTypeCategoryId,
    long TradeId,
    string TaskName,
    short Priority,
    decimal EstimatedHours,
    short? DisplayOrder,
    bool IsActive);

public sealed record JobTypeTaskDetailDto(
    long Id,
    long JobTypeCategoryId,
    long TradeId,
    string TaskName,
    short Priority,
    decimal EstimatedHours,
    short? DisplayOrder,
    bool IsActive);

public sealed record JobTypeTaskLookupDto(
    long Id);

public sealed record JobTypeTaskCreateDto(
    long JobTypeCategoryId,
    long TradeId,
    string TaskName,
    short Priority,
    decimal EstimatedHours,
    short? DisplayOrder);

public sealed record JobTypeTaskUpdateDto(
    long JobTypeCategoryId,
    long TradeId,
    string TaskName,
    short Priority,
    decimal EstimatedHours,
    short? DisplayOrder);

public sealed record JobTypeTaskPatchDto(
    long? JobTypeCategoryId,
    long? TradeId,
    string? TaskName,
    short? Priority,
    decimal? EstimatedHours,
    short? DisplayOrder,
    bool? IsActive);

public sealed record JobTypeTaskListFilters(
    string? TaskName = null);
