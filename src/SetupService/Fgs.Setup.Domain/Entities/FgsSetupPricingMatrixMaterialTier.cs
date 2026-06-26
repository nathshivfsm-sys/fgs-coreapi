using Fgs.Setup.Domain.Enums;

namespace Fgs.Setup.Domain.Entities;

public class FgsSetupPricingMatrixMaterialTier : FgsTenantCompanySetupEntityBase<long>
{
    public long PricingMatrixId { get; set; }

    public decimal FromCost { get; set; }

    public decimal? ToCost { get; set; }

    public decimal AdjustmentValue { get; set; }

    public PriceAdjustmentType PriceAdjustmentTypeId { get; set; }
}
