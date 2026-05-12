namespace Fgs.User.Domain.Entities;

public class FgsSetupTechTrade : FgsTenantCompanySetupEntityBase
{
    public string TradeCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? SortOrder { get; set; }
}
