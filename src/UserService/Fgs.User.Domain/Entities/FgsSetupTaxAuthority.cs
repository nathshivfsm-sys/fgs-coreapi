namespace Fgs.User.Domain.Entities;

public class FgsSetupTaxAuthority : FgsTenantCompanySetupEntityBase
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? RegionCode { get; set; }

    public string? Description { get; set; }
}
