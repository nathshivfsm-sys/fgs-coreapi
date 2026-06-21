using Fgs.Setup.Application.Features.JobTypeSubCategories.Dtos;

namespace Fgs.Setup.Infrastructure.JobTypeSubCategories;

internal sealed class JobTypeSubCategorySummaryRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string SubCategoryCode { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public JobTypeSubCategorySummaryDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            SubCategoryCode,
            Name,
            Description,
            DisplayOrder,
            IsActive,
            CreatedOn,
            UpdatedOn);
}

internal sealed class JobTypeSubCategoryDetailRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string SubCategoryCode { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public JobTypeSubCategoryDetailDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            SubCategoryCode,
            Name,
            Description,
            DisplayOrder,
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class JobTypeSubCategoryLookupRow
{
    public long Id { get; set; }
    public string SubCategoryCode { get; set; }
    public string Name { get; set; }
    public short? DisplayOrder { get; set; }

    public JobTypeSubCategoryLookupDto ToDto() => new(Id,
            SubCategoryCode,
            Name,
            DisplayOrder);
}
