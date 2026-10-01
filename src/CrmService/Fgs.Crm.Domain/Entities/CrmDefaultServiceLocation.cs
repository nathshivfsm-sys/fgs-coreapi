using Fgs.Kernel.Entities;

namespace Fgs.Crm.Domain.Entities;

/// <summary>
/// Default values used when creating new customer service locations.
/// </summary>
public class CrmDefaultServiceLocation : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long? DefaultLaborPricingMatrixId { get; set; }

    public long? DefaultMaterialPricingMatrixId { get; set; }

    public long? DefaultOtherPricingMatrixId { get; set; }

    public long? DefaultPaymentMethodId { get; set; }

    public bool EmailAllowed { get; set; }

    public long? EstimateEmailTemplateId { get; set; }

    public long? EstimateSmsTemplateId { get; set; }

    public long? InvoiceEmailTemplateId { get; set; }

    public long? InvoiceSmsTemplateId { get; set; }

    public bool SmsAllowed { get; set; }

    public bool TaxExempt { get; set; }
}
