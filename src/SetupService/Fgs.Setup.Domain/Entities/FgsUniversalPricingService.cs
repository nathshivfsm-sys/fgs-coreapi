using Fgs.Kernel.Entities;

namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Defines Universal Pricing Services enabled and configured for a tenant company.
/// </summary>
public class FgsUniversalPricingService : FgsTenantCompanySetupEntityBase<long>
{
    public string UniversalPricingServiceCode { get; set; } = null!;

    public short DisplayOrder { get; set; } = 1;
}
