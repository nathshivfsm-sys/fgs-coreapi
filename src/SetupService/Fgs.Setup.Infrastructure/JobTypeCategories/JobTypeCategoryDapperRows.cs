using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;

namespace Fgs.Setup.Infrastructure.JobTypeCategories;

internal sealed class JobTypeCategorySummaryRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string CategoryCode { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public JobTypeCategorySummaryDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            CategoryCode,
            Name,
            Description,
            DisplayOrder,
            IsActive,
            CreatedOn,
            UpdatedOn);
}

internal sealed class JobTypeCategoryDetailRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string CategoryCode { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public JobTypeCategoryDetailDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            CategoryCode,
            Name,
            Description,
            DisplayOrder,
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class JobTypeCategoryLookupRow
{
    public long Id { get; set; }
    public string CategoryCode { get; set; }
    public string Name { get; set; }
    public short? DisplayOrder { get; set; }

    public JobTypeCategoryLookupDto ToDto() => new(Id,
            CategoryCode,
            Name,
            DisplayOrder);
}
