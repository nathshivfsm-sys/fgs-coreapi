using Fgs.Kernel.Entities;

namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Stores company-specific Universal Pricing Matrix items and base prices.
/// </summary>
public class FgsUniversalMatrixItem : FgsTenantCompanySetupEntityBase<long>
{
    public long UniversalPricingServiceId { get; set; }

    public string ItemName { get; set; } = null!;

    public string UnitType { get; set; } = null!;

    public decimal BasePrice { get; set; }

    public short DisplayOrder { get; set; } = 1;
}
