namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Reusable service agreement pricing component and its default pricing for a tenant-company.
/// </summary>
public class FgsSetupServiceAgreementPricingComponent : FgsTenantCompanySetupEntityBase<long>
{
    public string PricingComponentCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string PricingComponentTypeCode { get; set; } = null!;

    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public short DisplayOrder { get; set; } = 1;
}
