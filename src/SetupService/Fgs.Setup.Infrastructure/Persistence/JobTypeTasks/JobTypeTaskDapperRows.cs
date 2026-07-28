using Fgs.Setup.Application.Features.JobTypeTasks.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.JobTypeTasks;

internal sealed class JobTypeTaskSummaryRow
{
    public long Id { get; set; }
    public long JobTypeCategoryId { get; set; }
    public long TradeId { get; set; }
    public string TaskName { get; set; } = null!;
    public short Priority { get; set; }
    public decimal EstimatedHours { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public JobTypeTaskSummaryDto ToDto() =>
        new(
            Id,
            JobTypeCategoryId,
            TradeId,
            TaskName,
            Priority,
            EstimatedHours,
            DisplayOrder,
            IsActive);
}

internal sealed class JobTypeTaskDetailRow
{
    public long Id { get; set; }
    public long JobTypeCategoryId { get; set; }
    public long TradeId { get; set; }
    public string TaskName { get; set; } = null!;
    public short Priority { get; set; }
    public decimal EstimatedHours { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public JobTypeTaskDetailDto ToDto() =>
        new(
            Id,
            JobTypeCategoryId,
            TradeId,
            TaskName,
            Priority,
            EstimatedHours,
            DisplayOrder,
            IsActive);
}

internal sealed class JobTypeTaskLookupRow
{
    public long Id { get; set; }

    public JobTypeTaskLookupDto ToDto() => new(Id);
}
