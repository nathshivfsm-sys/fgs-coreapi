using Fgs.Setup.Application.Features.JobTypeTasks.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.JobTypeTasks;

internal sealed class JobTypeTaskSummaryRow
{
    public long Id { get; set; }
    public long JobTypeCategoryId { get; set; }
    public long TradeId { get; set; }
    public long? SkillLevelId { get; set; }
    public string Name { get; set; } = null!;
    public string TaskName { get; set; } = null!;
    public short Priority { get; set; }
    public decimal EstimatedHours { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public string? CategoryName { get; set; }
    public string? TradeName { get; set; }
    public string? SkillName { get; set; }

    public JobTypeTaskSummaryDto ToDto() =>
        new(
            Id,
            JobTypeCategoryId,
            TradeId,
            SkillLevelId,
            Name,
            TaskName,
            Priority,
            EstimatedHours,
            DisplayOrder,
            IsActive,
            CategoryName,
            TradeName,
            SkillName);
}

internal sealed class JobTypeTaskDetailRow
{
    public long Id { get; set; }
    public long JobTypeCategoryId { get; set; }
    public long TradeId { get; set; }
    public long? SkillLevelId { get; set; }
    public string Name { get; set; } = null!;
    public string TaskName { get; set; } = null!;
    public short Priority { get; set; }
    public decimal EstimatedHours { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public string? CategoryName { get; set; }
    public string? TradeName { get; set; }
    public string? SkillName { get; set; }

    public JobTypeTaskDetailDto ToDto() =>
        new(
            Id,
            JobTypeCategoryId,
            TradeId,
            SkillLevelId,
            Name,
            TaskName,
            Priority,
            EstimatedHours,
            DisplayOrder,
            IsActive,
            CategoryName,
            TradeName,
            SkillName);
}

internal sealed class JobTypeTaskLookupRow
{
    public long Id { get; set; }

    public JobTypeTaskLookupDto ToDto() => new(Id);
}
