namespace Fgs.User.Domain.Entities;

public class FgsSetupPriceSheetMaterial : FgsTenantCompanySetupEntityBase
{
    public long FgsSetupPriceSheetId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public decimal? DefaultMarkupPercent { get; set; }

    public decimal? DefaultDiscountPercent { get; set; }
}
