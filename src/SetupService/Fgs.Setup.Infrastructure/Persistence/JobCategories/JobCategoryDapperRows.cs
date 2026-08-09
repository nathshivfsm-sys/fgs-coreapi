using Fgs.Setup.Application.Features.JobCategories.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.JobCategories;

internal sealed class JobCategorySummaryRow
{
    public long Id { get; set; }
    public string CategoryCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public JobCategorySummaryDto ToDto() =>
        new(
            Id,
            CategoryCode,
            Name,
            DisplayOrder,
            IsActive);
}

internal sealed class JobCategoryDetailRow
{
    public long Id { get; set; }
    public string CategoryCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public JobCategoryDetailDto ToDto() =>
        new(
            Id,
            CategoryCode,
            Name,
            DisplayOrder,
            IsActive);
}

internal sealed class JobCategoryLookupRow
{
    public long Id { get; set; }
    public string CategoryCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public short? DisplayOrder { get; set; }

    public JobCategoryLookupDto ToDto() => new(Id,
            CategoryCode,
            Name,
            DisplayOrder);
}
