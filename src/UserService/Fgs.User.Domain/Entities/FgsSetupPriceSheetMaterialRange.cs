namespace Fgs.User.Domain.Entities;

public class FgsSetupPriceSheetMaterialRange : FgsTenantCompanySetupEntityBase
{
    public long FgsSetupPriceSheetMaterialId { get; set; }

    public decimal CostFrom { get; set; }

    public decimal CostTo { get; set; }

    public decimal MarkupPercent { get; set; }
}
