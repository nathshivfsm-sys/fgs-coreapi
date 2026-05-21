namespace Fgs.User.Domain.Entities;

public class FgsSetupPricingMatrixOther : FgsTenantCompanySetupEntityBase<Guid>
{
    public Guid FgsSetupPricingMatrixId { get; set; }

    public string CategoryCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public decimal? MarkupPercent { get; set; }

    public decimal? DiscountPercent { get; set; }
}
