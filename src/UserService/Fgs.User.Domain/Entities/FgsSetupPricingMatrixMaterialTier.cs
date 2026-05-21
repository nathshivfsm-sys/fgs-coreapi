namespace Fgs.User.Domain.Entities;

public class FgsSetupPricingMatrixMaterialTier : FgsTenantCompanySetupEntityBase<long>
{
    public Guid FgsSetupPricingMatrixId { get; set; }

    public decimal FromCost { get; set; }

    public decimal? ToCost { get; set; }

    public decimal MarkupPercent { get; set; }

    public decimal? DiscountPercent { get; set; }
}
