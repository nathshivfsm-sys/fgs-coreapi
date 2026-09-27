using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.JobTypeCategories;

internal sealed class JobTypeCategorySummaryRow
{
    public long Id { get; set; }
    public long JobTypeId { get; set; }
    public long JobTypeTaskId { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public string? Name { get; set; }
    public string? TaskName { get; set; }
    public string? TradeName { get; set; }
    public string? SkillName { get; set; }
    public decimal? EstimatedHours { get; set; }
    public short? Priority { get; set; }

    public JobTypeCategorySummaryDto ToDto() =>
        new(
            Id,
            JobTypeId,
            JobTypeTaskId,
            DisplayOrder,
            IsActive,
            Name,
            TaskName,
            TradeName,
            SkillName,
            EstimatedHours,
            Priority);
}

internal sealed class JobTypeCategoryDetailRow
{
    public long Id { get; set; }
    public long JobTypeId { get; set; }
    public long JobTypeTaskId { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public string? Name { get; set; }
    public string? TaskName { get; set; }
    public string? TradeName { get; set; }
    public string? SkillName { get; set; }
    public decimal? EstimatedHours { get; set; }
    public short? Priority { get; set; }

    public JobTypeCategoryDetailDto ToDto() =>
        new(
            Id,
            JobTypeId,
            JobTypeTaskId,
            DisplayOrder,
            IsActive,
            Name,
            TaskName,
            TradeName,
            SkillName,
            EstimatedHours,
            Priority);
}

internal sealed class JobTypeCategoryLookupRow
{
    public long Id { get; set; }
    public long JobTypeId { get; set; }
    public long JobTypeTaskId { get; set; }
    public short? DisplayOrder { get; set; }
    public string? Name { get; set; }
    public string? TaskName { get; set; }
    public string? TradeName { get; set; }
    public string? SkillName { get; set; }
    public decimal? EstimatedHours { get; set; }
    public short? Priority { get; set; }

    public JobTypeCategoryLookupDto ToDto() =>
        new(
            Id,
            JobTypeId,
            JobTypeTaskId,
            DisplayOrder,
            Name,
            TaskName,
            TradeName,
            SkillName,
            EstimatedHours,
            Priority);
}
