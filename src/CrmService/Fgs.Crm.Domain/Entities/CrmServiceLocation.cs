using Fgs.Kernel.Entities;

namespace Fgs.Crm.Domain.Entities;

public class CrmServiceLocation : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long CustomerId { get; set; }

    public int LocationSequence { get; set; }

    public string LocationNumber { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public short ServiceLocationTypeId { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? AddressLine3 { get; set; }

    public string? AddressLine4 { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? County { get; set; }

    public string? Country { get; set; }

    public string? PostalCode { get; set; }

    public string? FormattedAddress { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public string? PlaceId { get; set; }

    public long? DefaultPaymentMethodId { get; set; }

    public long? DefaultMaterialPricingMatrixId { get; set; }

    public long? DefaultLaborPricingMatrixId { get; set; }

    public long? DefaultOtherPricingMatrixId { get; set; }

    public long? InvoiceEmailTemplateId { get; set; }

    public long? EstimateEmailTemplateId { get; set; }

    public long? InvoiceSmsTemplateId { get; set; }

    public long? EstimateSmsTemplateId { get; set; }

    public bool TaxExempt { get; set; }

    public bool EmailAllowed { get; set; } = true;

    public bool SmsAllowed { get; set; } = true;

    public bool IsActive { get; set; } = true;
}
