using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;

namespace Fgs.Setup.Infrastructure.JobTypeCategories;

internal sealed class JobTypeCategorySummaryRow
{
    public long Id { get; set; }
    public string CategoryCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public JobTypeCategorySummaryDto ToDto() =>
        new(
            Id,
            CategoryCode,
            Name,
            Description,
            DisplayOrder,
            IsActive);
}

internal sealed class JobTypeCategoryDetailRow
{
    public long Id { get; set; }
    public string CategoryCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public JobTypeCategoryDetailDto ToDto() =>
        new(
            Id,
            CategoryCode,
            Name,
            Description,
            DisplayOrder,
            IsActive);
}

internal sealed class JobTypeCategoryLookupRow
{
    public long Id { get; set; }
    public string CategoryCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public short? DisplayOrder { get; set; }

    public JobTypeCategoryLookupDto ToDto() => new(Id,
            CategoryCode,
            Name,
            DisplayOrder);
}
