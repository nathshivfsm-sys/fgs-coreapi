namespace Fgs.User.Domain.Entities;

public class FgsSetupPricingMatrixLaborTier : FgsTenantCompanySetupGuidEntityBase
{
    public Guid FgsSetupPricingMatrixLaborId { get; set; }

    public int SequenceOrder { get; set; }

    public int DurationMinutes { get; set; }

    public decimal Rate { get; set; }
}
