using Fgs.Kernel.Entities;

namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Stores company-specific one-time fees used by the Universal Pricing Matrix.
/// </summary>
public class FgsUniversalMatrixOneTimeFee : FgsTenantCompanySetupEntityBase<long>
{
    public long UniversalPricingServiceId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Amount { get; set; }

    public short DisplayOrder { get; set; } = 1;
}
