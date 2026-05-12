namespace Fgs.User.Domain.Entities;

public class FgsSetupGLBreak : FgsTenantCompanySetupEntityBase
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? BreakLabel { get; set; }

    public int BreakLevel { get; set; } = 1;

    public long? FgsSetupTechTradeId { get; set; }

    public string? LogoUrl { get; set; }
}
