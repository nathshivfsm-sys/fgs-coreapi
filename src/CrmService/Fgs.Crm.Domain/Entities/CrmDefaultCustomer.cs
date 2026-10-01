using Fgs.Kernel.Entities;

namespace Fgs.Crm.Domain.Entities;

/// <summary>
/// Default values used when creating new customers.
/// </summary>
public class CrmDefaultCustomer : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long? DefaultPaymentTermId { get; set; }

    public long? DefaultMaterialPricingMatrixId { get; set; }

    public long? DefaultLaborPricingMatrixId { get; set; }

    public long? DefaultOtherPricingMatrixId { get; set; }

    public bool DefaultPORequired { get; set; }

    public bool TaxExempt { get; set; }
}
