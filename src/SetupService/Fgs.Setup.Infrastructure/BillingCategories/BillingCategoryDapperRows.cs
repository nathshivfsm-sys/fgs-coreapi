using Fgs.Setup.Application.Features.BillingCategories.Dtos;

namespace Fgs.Setup.Infrastructure.BillingCategories;

internal sealed class BillingCategorySummaryRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string BillingCategoryType { get; set; }
    public string BillingCategoryName { get; set; }
    public string? Description { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsSystemDefined { get; set; }
    public bool ShowToFieldTech { get; set; }
    public bool AllowToPick { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public BillingCategorySummaryDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            BillingCategoryType,
            BillingCategoryName,
            Description,
            DisplayOrder,
            IsSystemDefined,
            ShowToFieldTech,
            AllowToPick,
            IsActive,
            CreatedOn,
            UpdatedOn);
}

internal sealed class BillingCategoryDetailRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string BillingCategoryType { get; set; }
    public string BillingCategoryName { get; set; }
    public string? Description { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsSystemDefined { get; set; }
    public bool ShowToFieldTech { get; set; }
    public bool AllowToPick { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public BillingCategoryDetailDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            BillingCategoryType,
            BillingCategoryName,
            Description,
            DisplayOrder,
            IsSystemDefined,
            ShowToFieldTech,
            AllowToPick,
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class BillingCategoryLookupRow
{
    public long Id { get; set; }
    public string BillingCategoryType { get; set; }
    public string BillingCategoryName { get; set; }
    public short? DisplayOrder { get; set; }

    public BillingCategoryLookupDto ToDto() => new(Id,
            BillingCategoryType,
            BillingCategoryName,
            DisplayOrder);
}
