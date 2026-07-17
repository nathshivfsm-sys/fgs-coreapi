namespace Fgs.Setup.Domain.Entities;

public class FgsSetupPricingMatrixLaborTier : FgsTenantCompanySetupEntityBase<long>
{
    public long PricingMatrixLaborId { get; set; }

    public long? TechSkillLevelId { get; set; }

    public short SequenceOrder { get; set; }

    public int DurationMinutes { get; set; }

    public decimal Rate { get; set; }
}
