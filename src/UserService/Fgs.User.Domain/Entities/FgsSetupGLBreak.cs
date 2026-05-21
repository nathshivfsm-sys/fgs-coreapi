namespace Fgs.User.Domain.Entities;

public class FgsSetupGLBreak : FgsTenantCompanySetupEntityBase<long>
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? BreakLabel { get; set; }

    public int BreakLevel { get; set; } = 1;

    public string? LogoUrl { get; set; }

    public ICollection<FgsSetupGLBreakTechTrade> TechTrades { get; set; } = new List<FgsSetupGLBreakTechTrade>();
}
