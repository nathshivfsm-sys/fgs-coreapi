namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global billing line category lookup (equipment, labor, tax, etc.).
/// </summary>
public class GloBillingCategory
{
    public string BillingCategoryType { get; set; } = null!;

    public string BillingCategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public short DisplayOrder { get; set; } = 1;

    public bool ShowToFieldTech { get; set; } = true;

    public bool AllowToPick { get; set; } = true;
}
