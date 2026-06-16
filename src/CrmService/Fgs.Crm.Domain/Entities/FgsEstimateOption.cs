using Fgs.Kernel.Entities;

namespace Fgs.Crm.Domain.Entities;

public class FgsEstimateOption : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long EstimateId { get; set; }

    public short DisplayOrder { get; set; } = 1;

    public string OptionName { get; set; } = null!;

    public string? OptionDescription { get; set; }

    public bool IsRecommended { get; set; }

    public bool IsSelected { get; set; }

    public DateTimeOffset? SelectedOn { get; set; }

    public decimal SubtotalAmount { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal TotalAmount { get; set; }

    public string? InternalNotes { get; set; }
}
