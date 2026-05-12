namespace Fgs.User.Domain.Entities;

public class FgsSetupPriceSheetLabor : FgsTenantCompanySetupEntityBase
{
    public long FgsSetupPriceSheetId { get; set; }

    public long? FgsSetupTechSkillLevelId { get; set; }

    public string RateType { get; set; } = null!;

    public decimal BaseRate { get; set; }

    public decimal? OvertimeMultiplier { get; set; }

    public decimal? DoubleTimeMultiplier { get; set; }

    public decimal? DiscountPercent { get; set; }
}
