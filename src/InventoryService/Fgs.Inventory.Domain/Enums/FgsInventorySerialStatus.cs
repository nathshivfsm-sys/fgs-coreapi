namespace Fgs.Inventory.Domain.Enums;

/// <summary>
/// Lifecycle status of an individual serialized inventory unit.
/// </summary>
public enum FgsInventorySerialStatus
{
    Available,
    Reserved,
    Installed,
    InTransit,
    Returned,
    Scrapped,
    Lost,
    OnHold
}
