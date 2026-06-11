using Fgs.Kernel.Entities;

namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Snapshot of a pricing component included in a service agreement template.
/// </summary>
public class FgsSetupServiceAgreementTemplatePricingComponent : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long ServiceAgreementTemplateId { get; set; }

    public string PricingComponentCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public decimal Amount { get; set; }

    public short DisplayOrder { get; set; } = 1;
}
