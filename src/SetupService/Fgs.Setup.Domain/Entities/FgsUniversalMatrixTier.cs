using Fgs.Kernel.Entities;

namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Stores company-specific pricing tiers and pricing multipliers for an enabled Universal Pricing Service.
/// </summary>
public class FgsUniversalMatrixTier : FgsTenantCompanySetupEntityBase<long>
{
    public long UniversalPricingServiceId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Multiplier { get; set; } = 1.0000m;

    public short DisplayOrder { get; set; } = 1;
}
