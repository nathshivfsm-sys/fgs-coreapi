using Fgs.Kernel.Entities;

namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Stores company-specific optional add-ons available within the Universal Pricing Matrix.
/// </summary>
public class FgsUniversalMatrixAddOn : FgsTenantCompanySetupEntityBase<long>
{
    public long UniversalPricingServiceId { get; set; }

    public string Name { get; set; } = null!;

    public string UnitType { get; set; } = null!;

    public decimal Price { get; set; }

    public short DisplayOrder { get; set; } = 1;
}
