namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Tenant- and company-scoped job type category catalog seeded from GloJobTypeCategory.
/// </summary>
public class FgsJobTypeCategory : FgsTenantCompanySetupEntityBase<long>
{
    public string CategoryCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public short DisplayOrder { get; set; } = 1;
}
