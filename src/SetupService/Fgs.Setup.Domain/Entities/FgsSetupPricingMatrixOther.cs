namespace Fgs.Setup.Domain.Entities;

public class FgsSetupPricingMatrixOther : FgsTenantCompanySetupEntityBase<long>
{
    public long PricingMatrixId { get; set; }

    public string CategoryCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public decimal? AdjustmentValue { get; set; }

    public decimal? DiscountPercent { get; set; }
}
