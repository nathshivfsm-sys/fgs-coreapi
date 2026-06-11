using Fgs.Kernel.Entities;

namespace Fgs.Crm.Domain.Entities;

public class CrmCustomer : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public string CustomerNumber { get; set; } = null!;

    public int LastServiceLocationSequence { get; set; }

    public string Name { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

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

    public long? DefaultPaymentTermId { get; set; }

    public long? DefaultMaterialPricingMatrixId { get; set; }

    public long? DefaultLaborPricingMatrixId { get; set; }

    public long? DefaultOtherPricingMatrixId { get; set; }

    public bool DefaultPORequired { get; set; }

    public bool TaxExempt { get; set; }

    public string? TaxExemptNumber { get; set; }

    public string? CustomerAccountNumber { get; set; }

    public string? ExternalEntityId { get; set; }

    public string? ExternalVersion { get; set; }

    public bool IsActive { get; set; } = true;
}
