using Fgs.Setup.Application.Features.JobTypeSubCategories.Dtos;

namespace Fgs.Setup.Infrastructure.JobTypeSubCategories;

internal sealed class JobTypeSubCategorySummaryRow
{
    public long Id { get; set; }
    public string SubCategoryCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public JobTypeSubCategorySummaryDto ToDto() =>
        new(
            Id,
            SubCategoryCode,
            Name,
            Description,
            DisplayOrder,
            IsActive);
}

internal sealed class JobTypeSubCategoryDetailRow
{
    public long Id { get; set; }
    public string SubCategoryCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public JobTypeSubCategoryDetailDto ToDto() =>
        new(
            Id,
            SubCategoryCode,
            Name,
            Description,
            DisplayOrder,
            IsActive);
}

internal sealed class JobTypeSubCategoryLookupRow
{
    public long Id { get; set; }
    public string SubCategoryCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public short? DisplayOrder { get; set; }

    public JobTypeSubCategoryLookupDto ToDto() => new(Id,
            SubCategoryCode,
            Name,
            DisplayOrder);
}
