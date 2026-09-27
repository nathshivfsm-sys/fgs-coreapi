using Fgs.Setup.Application.Features.JobTypes.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.JobTypes;

internal sealed class JobTypeSummaryRow
{
    public long Id { get; set; }
    public string JobTypeCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public short UsedFor { get; set; }
    public string? BusinessUnit { get; set; }
    public bool ShowToFieldTech { get; set; }
    public bool ShowOnCustomerPortal { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public JobTypeSummaryDto ToDto() =>
        new(
            Id,
            JobTypeCode,
            Name,
            UsedFor,
            BusinessUnit,
            ShowToFieldTech,
            ShowOnCustomerPortal,
            DisplayOrder,
            IsActive);
}

internal sealed class JobTypeDetailRow
{
    public long Id { get; set; }
    public string JobTypeCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public short UsedFor { get; set; }
    public string? BusinessUnit { get; set; }
    public bool ShowToFieldTech { get; set; }
    public bool ShowOnCustomerPortal { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public JobTypeDetailDto ToDto(IReadOnlyList<JobTypeSubCategoryDto>? subCategories = null) =>
        new(
            Id,
            JobTypeCode,
            Name,
            UsedFor,
            BusinessUnit,
            ShowToFieldTech,
            ShowOnCustomerPortal,
            DisplayOrder,
            IsActive,
            subCategories ?? []);
}

internal sealed class JobTypeSubCategoryRow
{
    public long Id { get; set; }
    public long JobTypeTaskId { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public string? Name { get; set; }
    public string? TaskName { get; set; }
    public string? TradeName { get; set; }
    public string? SkillName { get; set; }
    public decimal? EstimatedHours { get; set; }
    public short? Priority { get; set; }

    public JobTypeSubCategoryDto ToDto() =>
        new(Id, JobTypeTaskId, DisplayOrder, IsActive, Name, TaskName, TradeName, SkillName, EstimatedHours, Priority);
}

internal sealed class JobTypeCountsRow
{
    public int ActiveCount { get; set; }
    public int InactiveCount { get; set; }

    public JobTypeCountsDto ToDto() => new(ActiveCount, InactiveCount);
}

internal sealed class JobTypeLookupRow
{
    public long Id { get; set; }
    public string JobTypeCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public short? DisplayOrder { get; set; }

    public JobTypeLookupDto ToDto() => new(Id,
            JobTypeCode,
            Name,
            DisplayOrder);
}
