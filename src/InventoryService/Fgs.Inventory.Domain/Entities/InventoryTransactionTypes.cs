namespace Fgs.Inventory.Domain.Entities;

public static class InventoryTransactionTypes
{
    public const string Initial = "INITIAL";

    public const string PurchaseReceipt = "PURCHASE_RECEIPT";

    public const string Transfer = "TRANSFER";

    public const string Usage = "USAGE";

    public const string Adjustment = "ADJUSTMENT";

    public const string Return = "RETURN";

    public const string PhysicalCount = "PHYSICAL_COUNT";
}
