namespace Fgs.User.Domain.Entities;

public class FgsSetupPriceSheet : FgsTenantCompanySetupEntityBase
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    /// <summary>
    /// When true, labor pricing uses tier rows (<see cref="FgsSetupPriceSheetLaborTier"/>).
    /// </summary>
    public bool IsLaborTierStructure { get; set; }

    /// <summary>
    /// When true, labor rates vary by technician skill level.
    /// </summary>
    public bool IsLaborRateBySkillLevel { get; set; }

    public DateOnly EffectiveFrom { get; set; }

    public DateOnly? EffectiveTo { get; set; }

    public bool IsMobileVisible { get; set; } = true;
}
