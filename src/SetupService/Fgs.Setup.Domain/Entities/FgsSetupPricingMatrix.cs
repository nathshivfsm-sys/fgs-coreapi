namespace Fgs.Setup.Domain.Entities;

public class FgsSetupPricingMatrix : FgsTenantCompanySetupEntityBase<long>
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsLaborTierStructure { get; set; }

    public bool IsLaborRateBySkillLevel { get; set; }

    public DateOnly EffectiveFrom { get; set; }

    public DateOnly? EffectiveTo { get; set; }

    public bool IsMobileVisible { get; set; } = true;
}
