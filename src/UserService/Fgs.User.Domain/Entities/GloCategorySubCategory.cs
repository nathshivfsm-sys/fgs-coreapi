namespace Fgs.User.Domain.Entities;

/// <summary>
/// Maps business type, category, and sub-category for onboarding seed logic.
/// </summary>
public class GloCategorySubCategory
{
    public int BusinessTypeId { get; set; }

    public short CategoryId { get; set; }

    public short SubCategoryId { get; set; }

    public DateTimeOffset CreatedOn { get; set; }
}
