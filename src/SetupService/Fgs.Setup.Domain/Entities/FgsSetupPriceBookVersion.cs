namespace Fgs.Setup.Domain.Entities;

public class FgsSetupPriceBookVersion : FgsTenantCompanySetupEntityBase<long>
{
    public string Version { get; set; } = null!;

    public DateOnly EffectiveFrom { get; set; }

    public DateOnly? EffectiveTo { get; set; }
}
