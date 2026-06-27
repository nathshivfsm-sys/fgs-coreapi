namespace Fgs.Setup.Domain.Entities;

public class FgsSetupPricingMatrixLabor : FgsTenantCompanySetupEntityBase<long>
{
    public long PricingMatrixId { get; set; }

    public int LaborRateTypeId { get; set; }

    public long? TechSkillLevelId { get; set; }

    public decimal BaseRate { get; set; }

    public decimal? OvertimeMultiplier { get; set; }

    public decimal? DoubleTimeMultiplier { get; set; }

    public decimal? DiscountPercent { get; set; }
}
