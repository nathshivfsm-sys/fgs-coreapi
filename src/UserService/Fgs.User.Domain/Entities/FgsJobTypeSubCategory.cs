namespace Fgs.User.Domain.Entities;

/// <summary>
/// Tenant- and company-scoped job type subcategory catalog seeded from GloJobTypeSubCategory.
/// </summary>
public class FgsJobTypeSubCategory : FgsTenantCompanySetupEntityBase<long>
{
    public string SubCategoryCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public short DisplayOrder { get; set; } = 1;
}
