namespace Fgs.User.Domain.Entities;

public class FgsSetupPriceSheet : FgsTenantCompanySetupEntityBase
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public DateOnly EffectiveFrom { get; set; }

    public DateOnly? EffectiveTo { get; set; }

    public bool IsMobileVisible { get; set; } = true;
}
