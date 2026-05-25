namespace Fgs.User.Domain.Entities;

/// <summary>
/// Tenant- and company-scoped labor rate type catalog seeded from GloSetupLaborRateType.
/// </summary>
public class FgsSetupLaborRateType : FgsTenantCompanySetupEntityBase<long>
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int SortOrder { get; set; }

    public bool IsSystem { get; set; }
}
