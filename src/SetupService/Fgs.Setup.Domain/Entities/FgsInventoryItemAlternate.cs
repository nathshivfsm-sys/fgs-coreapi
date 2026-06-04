namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Alternate or substitute inventory item mapping.
/// </summary>
public class FgsInventoryItemAlternate : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long InventoryItemId { get; set; }

    public long AlternateInventoryItemId { get; set; }

    public string AlternateType { get; set; } = null!;

    public short PriorityOrder { get; set; } = 1;

    public string? Notes { get; set; }

    public bool IsActive { get; set; } = true;
}
