namespace Fgs.User.Domain.Entities;

public class FgsSetupPriceSheetOther : FgsTenantCompanySetupEntityBase
{
    public long FgsSetupPriceSheetId { get; set; }

    public string CategoryCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public decimal? MarkupPercent { get; set; }

    public decimal? DiscountPercent { get; set; }
}
