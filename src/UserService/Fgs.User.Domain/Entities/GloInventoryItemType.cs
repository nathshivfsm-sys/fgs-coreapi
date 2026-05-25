namespace Fgs.User.Domain.Entities;

/// <summary>
/// Global inventory item type catalog (inventory, non-inventory, service, kit, tool).
/// </summary>
public class GloInventoryItemType
{
    public short Id { get; set; }

    public string ItemTypeCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool TracksQuantity { get; set; }

    public short DisplayOrder { get; set; } = 1;

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedOn { get; set; }
}
