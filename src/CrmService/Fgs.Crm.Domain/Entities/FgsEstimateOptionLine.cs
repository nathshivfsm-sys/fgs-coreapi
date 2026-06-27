using Fgs.Kernel.Entities;

namespace Fgs.Crm.Domain.Entities;

public class FgsEstimateOptionLine : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long EstimateOptionId { get; set; }

    public long? ParentLineId { get; set; }

    public long? TemplateId { get; set; }

    public long? TemplateLineId { get; set; }

    public int DisplayOrder { get; set; } = 1;

    public long BillingCategoryId { get; set; }

    public string? ItemCode { get; set; }

    public long? RateOfDayId { get; set; }

    public string Description { get; set; } = null!;

    public bool ShowOnProposal { get; set; } = true;

    public bool ShowPriceOnProposal { get; set; } = true;

    public bool ShowToFieldTechnician { get; set; } = true;

    public string? Source { get; set; }

    public decimal Quantity { get; set; } = 1;

    public decimal UnitCost { get; set; }

    public decimal ExtendedCost { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal ExtendedPrice { get; set; }
}
