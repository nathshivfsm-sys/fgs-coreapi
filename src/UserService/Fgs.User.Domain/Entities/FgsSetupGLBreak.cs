namespace Fgs.User.Domain.Entities;

public class FgsSetupGLBreak : FgsTenantCompanySetupEntityBase
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int BreakLevel { get; set; } = 1;

    public long? FgsSetupTechTradeId { get; set; }

    public Guid? LogoLocationId { get; set; }

    public string? Description { get; set; }
}
