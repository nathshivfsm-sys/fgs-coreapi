using Fgs.Inventory.Application.Features.VendorInventoryItems.Dtos;

namespace Fgs.Inventory.Infrastructure.VendorInventoryItems;

internal sealed class FgsVendorInventoryItemSummaryRow
{
    public long Id { get; set; }
    public long VendorId { get; set; }
    public long InventoryItemId { get; set; }
    public string? VendorPartNumber { get; set; }
    public string? VendorPartName { get; set; }
    public decimal LastCost { get; set; }
    public DateTimeOffset? LastReceivedDate { get; set; }
    public string? PurchaseOrderComments { get; set; }
    public short VendorPriority { get; set; }
    public short? LeadTimeDays { get; set; }
    public bool IsActive { get; set; }

    public FgsVendorInventoryItemSummaryDto ToDto() =>
        new(
            Id,
            VendorId,
            InventoryItemId,
            VendorPartNumber,
            VendorPartName,
            LastCost,
            LastReceivedDate,
            PurchaseOrderComments,
            VendorPriority,
            LeadTimeDays,
            IsActive);
}

internal sealed class FgsVendorInventoryItemDetailRow
{
    public long Id { get; set; }
    public long VendorId { get; set; }
    public long InventoryItemId { get; set; }
    public string? VendorPartNumber { get; set; }
    public string? VendorPartName { get; set; }
    public decimal LastCost { get; set; }
    public DateTimeOffset? LastReceivedDate { get; set; }
    public string? PurchaseOrderComments { get; set; }
    public short VendorPriority { get; set; }
    public short? LeadTimeDays { get; set; }
    public bool IsActive { get; set; }

    public FgsVendorInventoryItemDetailDto ToDto() =>
        new(
            Id,
            VendorId,
            InventoryItemId,
            VendorPartNumber,
            VendorPartName,
            LastCost,
            LastReceivedDate,
            PurchaseOrderComments,
            VendorPriority,
            LeadTimeDays,
            IsActive);
}

internal sealed class FgsVendorInventoryItemLookupRow
{
    public long Id { get; set; }
    public long VendorId { get; set; }
    public long InventoryItemId { get; set; }
    public string? VendorPartNumber { get; set; }

    public FgsVendorInventoryItemLookupDto ToDto() =>
        new(Id, VendorId, InventoryItemId, VendorPartNumber);
}
