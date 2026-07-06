using Fgs.Setup.Application.Features.BillingCategories.Dtos;

namespace Fgs.Setup.Infrastructure.BillingCategories;

internal sealed class BillingCategorySummaryRow
{
    public long Id { get; set; }
    public string BillingCategoryType { get; set; } = null!;
    public string BillingCategoryName { get; set; } = null!;
    public string? Description { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsSystemDefined { get; set; }
    public bool ShowToFieldTech { get; set; }
    public bool AllowToPick { get; set; }
    public bool IsActive { get; set; }

    public BillingCategorySummaryDto ToDto() =>
        new(
            Id,
            BillingCategoryType,
            BillingCategoryName,
            Description,
            DisplayOrder,
            IsSystemDefined,
            ShowToFieldTech,
            AllowToPick,
            IsActive);
}

internal sealed class BillingCategoryDetailRow
{
    public long Id { get; set; }
    public string BillingCategoryType { get; set; } = null!;
    public string BillingCategoryName { get; set; } = null!;
    public string? Description { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsSystemDefined { get; set; }
    public bool ShowToFieldTech { get; set; }
    public bool AllowToPick { get; set; }
    public bool IsActive { get; set; }

    public BillingCategoryDetailDto ToDto() =>
        new(
            Id,
            BillingCategoryType,
            BillingCategoryName,
            Description,
            DisplayOrder);
}

internal sealed class BillingCategoryLookupRow
{
    public long Id { get; set; }
    public string BillingCategoryType { get; set; } = null!;
    public string BillingCategoryName { get; set; } = null!;
    public short? DisplayOrder { get; set; }

    public BillingCategoryLookupDto ToDto() => new(Id,
            BillingCategoryType,
            BillingCategoryName,
            DisplayOrder);
}
