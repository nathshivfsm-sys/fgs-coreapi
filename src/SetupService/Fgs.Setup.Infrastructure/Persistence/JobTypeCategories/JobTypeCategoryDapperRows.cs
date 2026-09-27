using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.JobTypeCategories;

internal sealed class JobTypeCategorySummaryRow
{
    public long Id { get; set; }
    public long JobTypeId { get; set; }
    public long JobTypeTaskId { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public JobTypeCategorySummaryDto ToDto() =>
        new(
            Id,
            JobTypeId,
            JobTypeTaskId,
            DisplayOrder,
            IsActive);
}

internal sealed class JobTypeCategoryDetailRow
{
    public long Id { get; set; }
    public long JobTypeId { get; set; }
    public long JobTypeTaskId { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public JobTypeCategoryDetailDto ToDto() =>
        new(
            Id,
            JobTypeId,
            JobTypeTaskId,
            DisplayOrder,
            IsActive);
}

internal sealed class JobTypeCategoryLookupRow
{
    public long Id { get; set; }
    public long JobTypeId { get; set; }
    public long JobTypeTaskId { get; set; }
    public short? DisplayOrder { get; set; }

    public JobTypeCategoryLookupDto ToDto() => new(Id,
            JobTypeId,
            JobTypeTaskId,
            DisplayOrder);
}
