using Fgs.Foundation.Paging;

namespace Fgs.Inventory.Infrastructure.PurchaseOrders;

internal static class FgsPurchaseOrderSql
{
    public const string Table = "inventory.\"FgsPurchaseOrder\"";
    public const string DetailTable = "inventory.\"FgsPurchaseOrderDetail\"";
    public const string InventoryItemTable = "inventory.\"FgsInventoryItem\"";
    public const string InventoryLocationTable = "inventory.\"FgsInventoryLocation\"";

    public const string SelectSummaryColumns = """
        "Id", "PurchaseOrderNumber", "VendorId", "PurchaseOrderStatus", "PurchaseOrderDate", "ExpectedDeliveryDate", "TotalAmount"
        """;

    public const string SelectDetailColumns = """
        "Id", "PurchaseOrderNumber", "VendorId", "PurchaseOrderStatus", "PurchaseOrderDate", "ExpectedDeliveryDate",
        "RequestedByEmployeeId", "RequestedByName", "BuyerEmployeeId",
        "ShipToInventoryLocationId", "ShipToServiceLocationId",
        "ShipToName", "ShipToAddress1", "ShipToAddress2", "ShipToCity", "ShipToStateProvince", "ShipToPostalCode", "ShipToCountry",
        "VendorReferenceNumber", "VendorContactName", "VendorEmail", "VendorPhoneNumber",
        "Subtotal", "DiscountAmount", "TaxableAmount", "PurchaseTaxJson", "FreightAmount", "OtherCharges", "TotalAmount",
        "VendorNotes", "InternalNotes"
        """;

    public const string SelectLineColumns = """
        "Id", "LineNumber", "ItemId", "VendorPartNumber", "ItemDescription", "UnitOfMeasureCode",
        "OrderedQuantity", "ReceivedQuantity", "UnitCost", "DiscountAmount", "IsTaxable", "ExtendedAmount",
        "ExpectedDeliveryDate", "Notes"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "PurchaseOrderNumber", "VendorId", "PurchaseOrderStatus", "PurchaseOrderDate", "ExpectedDeliveryDate", "TotalAmount"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"Id\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return $"ORDER BY \"{column}\" {dir}";
    }
}
