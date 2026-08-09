namespace Fgs.Inventory.Application.Features.VendorInventoryItems.Dtos;

public sealed record FgsVendorInventoryItemSummaryDto(
    long Id,
    long VendorId,
    long InventoryItemId,
    string? VendorPartNumber,
    string? VendorPartName,
    decimal LastCost,
    DateTimeOffset? LastReceivedDate,
    string? PurchaseOrderComments,
    short VendorPriority,
    short? LeadTimeDays,
    bool IsActive);

public sealed record FgsVendorInventoryItemDetailDto(
    long Id,
    long VendorId,
    long InventoryItemId,
    string? VendorPartNumber,
    string? VendorPartName,
    decimal LastCost,
    DateTimeOffset? LastReceivedDate,
    string? PurchaseOrderComments,
    short VendorPriority,
    short? LeadTimeDays,
    bool IsActive);

public sealed record FgsVendorInventoryItemLookupDto(
    long Id,
    long VendorId,
    long InventoryItemId,
    string? VendorPartNumber);

public sealed record FgsVendorInventoryItemCreateDto(
    long VendorId,
    long InventoryItemId,
    string? VendorPartNumber,
    string? VendorPartName,
    decimal LastCost,
    DateTimeOffset? LastReceivedDate,
    string? PurchaseOrderComments,
    short VendorPriority,
    short? LeadTimeDays);

public sealed record FgsVendorInventoryItemUpdateDto(
    long VendorId,
    long InventoryItemId,
    string? VendorPartNumber,
    string? VendorPartName,
    decimal LastCost,
    DateTimeOffset? LastReceivedDate,
    string? PurchaseOrderComments,
    short VendorPriority,
    short? LeadTimeDays);

public sealed record FgsVendorInventoryItemPatchDto(
    long? VendorId,
    long? InventoryItemId,
    string? VendorPartNumber,
    string? VendorPartName,
    decimal? LastCost,
    DateTimeOffset? LastReceivedDate,
    string? PurchaseOrderComments,
    short? VendorPriority,
    short? LeadTimeDays,
    bool? IsActive);

public sealed record FgsVendorInventoryItemListFilters(
    long? VendorId = null,
    long? InventoryItemId = null,
    string? VendorPartNumber = null);
