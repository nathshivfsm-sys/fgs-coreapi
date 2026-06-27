using Fgs.Kernel.Entities;

namespace Fgs.Crm.Domain.Entities;

public class FgsEstimateTemplateOption : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long EstimateTemplateId { get; set; }

    public long EstimateFlavorId { get; set; }

    public short DisplayOrder { get; set; } = 1;

    public string OptionName { get; set; } = null!;

    public string? OptionDescription { get; set; }

    public bool ShowOnProposal { get; set; } = true;

    public bool ShowPriceOnProposal { get; set; } = true;

    public bool IsSelectedByDefault { get; set; }

    public bool AllowQuantityChange { get; set; } = true;

    public bool AllowPriceChange { get; set; } = true;
}
