using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.JobTypeCategories;

internal sealed class JobTypeCategorySummaryRow
{
    public long Id { get; set; }
    public long JobTypeId { get; set; }
    public long JobCategoryId { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public JobTypeCategorySummaryDto ToDto() =>
        new(
            Id,
            JobTypeId,
            JobCategoryId,
            DisplayOrder,
            IsActive);
}

internal sealed class JobTypeCategoryDetailRow
{
    public long Id { get; set; }
    public long JobTypeId { get; set; }
    public long JobCategoryId { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public JobTypeCategoryDetailDto ToDto() =>
        new(
            Id,
            JobTypeId,
            JobCategoryId,
            DisplayOrder,
            IsActive);
}

internal sealed class JobTypeCategoryLookupRow
{
    public long Id { get; set; }
    public long JobTypeId { get; set; }
    public long JobCategoryId { get; set; }
    public short? DisplayOrder { get; set; }

    public JobTypeCategoryLookupDto ToDto() => new(Id,
            JobTypeId,
            JobCategoryId,
            DisplayOrder);
}
