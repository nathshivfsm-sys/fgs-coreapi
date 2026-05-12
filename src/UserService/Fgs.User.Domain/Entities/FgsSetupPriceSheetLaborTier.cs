namespace Fgs.User.Domain.Entities;

public class FgsSetupPriceSheetLaborTier : FgsTenantCompanySetupEntityBase
{
    public long FgsSetupPriceSheetLaborId { get; set; }

    public int SequenceOrder { get; set; }

    public int DurationMinutes { get; set; }

    public decimal Rate { get; set; }
}
