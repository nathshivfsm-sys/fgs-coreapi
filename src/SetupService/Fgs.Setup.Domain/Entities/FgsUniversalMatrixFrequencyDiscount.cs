using Fgs.Kernel.Entities;

namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Stores company-specific service frequency options and their discount percentages.
/// </summary>
public class FgsUniversalMatrixFrequencyDiscount : FgsTenantCompanySetupEntityBase<long>
{
    public long UniversalPricingServiceId { get; set; }

    public string Name { get; set; } = null!;

    public decimal DiscountPercent { get; set; }

    public short DisplayOrder { get; set; } = 1;
}
