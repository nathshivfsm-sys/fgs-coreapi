namespace Fgs.Setup.Domain.Entities;

public class FgsSetupPricingMatrixLaborTier : FgsTenantCompanySetupEntityBase<long>
{
    public Guid FgsSetupPricingMatrixLaborId { get; set; }

    public int SequenceOrder { get; set; }

    public int DurationMinutes { get; set; }

    public decimal Rate { get; set; }
}
