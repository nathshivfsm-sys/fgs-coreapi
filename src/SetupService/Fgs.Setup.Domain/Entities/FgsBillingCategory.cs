namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Tenant- and company-scoped billing category catalog seeded from GloBillingCategory.
/// </summary>
public class FgsBillingCategory : FgsTenantCompanySetupEntityBase<long>
{
    public string BillingCategoryType { get; set; } = null!;

    public string BillingCategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public short DisplayOrder { get; set; } = 1;

    public bool IsSystemDefined { get; set; }

    public bool ShowToFieldTech { get; set; }

    public bool AllowToPick { get; set; } = true;
}
