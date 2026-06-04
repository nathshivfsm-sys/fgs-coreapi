namespace Fgs.Setup.Domain.Entities;

public class FgsSetupPricingMatrixLabor : FgsTenantCompanySetupEntityBase<Guid>
{
    public Guid FgsSetupPricingMatrixId { get; set; }

    public int FgsSetupLaborRateTypeId { get; set; }

    public long? FgsSetupTechSkillLevelId { get; set; }

    public decimal BaseRate { get; set; }

    public decimal? OvertimeMultiplier { get; set; }

    public decimal? DoubleTimeMultiplier { get; set; }

    public decimal? DiscountPercent { get; set; }
}
