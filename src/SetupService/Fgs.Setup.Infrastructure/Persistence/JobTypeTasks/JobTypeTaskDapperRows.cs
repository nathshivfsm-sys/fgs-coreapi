using Fgs.Setup.Application.Features.JobTypeTasks.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.JobTypeTasks;

internal sealed class JobTypeTaskSummaryRow
{
    public long Id { get; set; }
    public long JobCategoryId { get; set; }
    public long TradeId { get; set; }
    public long? SkillLevelId { get; set; }
    public string Name { get; set; } = null!;
    public string TaskName { get; set; } = null!;
    public short Priority { get; set; }
    public decimal EstimatedHours { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public string? CategoryName { get; set; }

    public JobTypeTaskSummaryDto ToDto() =>
        new(
            Id,
            JobCategoryId,
            TradeId,
            SkillLevelId,
            Name,
            TaskName,
            Priority,
            EstimatedHours,
            DisplayOrder,
            IsActive,
            CategoryName);
}

internal sealed class JobTypeTaskDetailRow
{
    public long Id { get; set; }
    public long JobCategoryId { get; set; }
    public long TradeId { get; set; }
    public long? SkillLevelId { get; set; }
    public string Name { get; set; } = null!;
    public string TaskName { get; set; } = null!;
    public short Priority { get; set; }
    public decimal EstimatedHours { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public string? CategoryName { get; set; }

    public JobTypeTaskDetailDto ToDto() =>
        new(
            Id,
            JobCategoryId,
            TradeId,
            SkillLevelId,
            Name,
            TaskName,
            Priority,
            EstimatedHours,
            DisplayOrder,
            IsActive,
            CategoryName);
}

internal sealed class JobTypeTaskLookupRow
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;

    public JobTypeTaskLookupDto ToDto() => new(Id, Name);
}
