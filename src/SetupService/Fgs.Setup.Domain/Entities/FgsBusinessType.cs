namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Tenant- and company-scoped business type catalog seeded from GloBusinessType.
/// </summary>
public class FgsBusinessType : FgsTenantCompanySetupEntityBase<long>
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public short DisplayOrder { get; set; } = 1;
}
