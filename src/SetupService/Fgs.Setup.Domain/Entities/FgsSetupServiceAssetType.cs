namespace Fgs.Setup.Domain.Entities;

public class FgsSetupServiceAssetType : FgsTenantCompanySetupEntityBase<long>
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }
}
