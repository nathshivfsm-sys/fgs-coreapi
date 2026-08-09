using Fgs.Inventory.Domain.Enums;
using Fgs.Kernel.Entities;

namespace Fgs.Inventory.Domain.Entities;

/// <summary>
/// Individual serialized inventory unit and its current lifecycle status. Inventory movement
/// history is maintained in <see cref="FgsInventoryTransaction"/>.
/// </summary>
public class FgsInventorySerial : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long InventoryItemId { get; set; }

    public string SerialNumber { get; set; } = null!;

    public FgsInventorySerialStatus InventorySerialStatus { get; set; } = FgsInventorySerialStatus.Available;

    public string? Notes { get; set; }
}
