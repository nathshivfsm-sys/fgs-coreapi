namespace Fgs.User.Domain.Entities;

public class FgsSetupZone : FgsTenantCompanySetupEntityBase
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }
}
