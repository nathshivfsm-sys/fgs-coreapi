namespace Fgs.Setup.Domain.Entities;

public class FgsSetupTechTrade : FgsTenantCompanySetupEntityBase<long>
{
    public string TradeCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? SortOrder { get; set; }
}
