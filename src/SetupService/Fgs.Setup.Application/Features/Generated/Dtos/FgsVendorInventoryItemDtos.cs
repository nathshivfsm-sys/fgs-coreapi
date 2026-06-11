namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsVendorInventoryItem</summary>
public sealed record FgsVendorInventoryItemSummaryDto(
    /// <summary>Vendor-specific part number for the inventory item.</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>VendorId</summary>
    long VendorId,
    /// <summary>InventoryItemId</summary>
    long InventoryItemId,
    /// <summary>VendorPartNumber</summary>
    string? VendorPartNumber,
    /// <summary>Last received cost from the vendor based on purchase order receiving.</summary>
    string? VendorPartName,
    /// <summary>LastCost</summary>
    decimal LastCost,
    /// <summary>Last date inventory was received from the vendor.</summary>
    DateTimeOffset? LastReceivedDate,
    /// <summary>Comments automatically copied to purchase orders for this vendor item combination.</summary>
    string? PurchaseOrderComments,
    /// <summary>Indicates whether this vendor is the preferred vendor for the inventory item.</summary>
    bool IsPreferredVendor,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsVendorInventoryItemDetailDto(
    /// <summary>Vendor-specific part number for the inventory item.</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>VendorId</summary>
    long VendorId,
    /// <summary>InventoryItemId</summary>
    long InventoryItemId,
    /// <summary>VendorPartNumber</summary>
    string? VendorPartNumber,
    /// <summary>Last received cost from the vendor based on purchase order receiving.</summary>
    string? VendorPartName,
    /// <summary>LastCost</summary>
    decimal LastCost,
    /// <summary>Last date inventory was received from the vendor.</summary>
    DateTimeOffset? LastReceivedDate,
    /// <summary>Comments automatically copied to purchase orders for this vendor item combination.</summary>
    string? PurchaseOrderComments,
    /// <summary>Indicates whether this vendor is the preferred vendor for the inventory item.</summary>
    bool IsPreferredVendor,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>CreatedBy</summary>
    string? CreatedBy,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>UpdatedBy</summary>
    string? UpdatedBy,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsVendorInventoryItemCreateDto(
    /// <summary>VendorId</summary>
    long VendorId,
    /// <summary>InventoryItemId</summary>
    long InventoryItemId,
    /// <summary>VendorPartNumber</summary>
    string? VendorPartNumber,
    /// <summary>Last received cost from the vendor based on purchase order receiving.</summary>
    string? VendorPartName,
    /// <summary>LastCost</summary>
    decimal LastCost,
    /// <summary>Last date inventory was received from the vendor.</summary>
    DateTimeOffset? LastReceivedDate,
    /// <summary>Comments automatically copied to purchase orders for this vendor item combination.</summary>
    string? PurchaseOrderComments,
    /// <summary>Indicates whether this vendor is the preferred vendor for the inventory item.</summary>
    bool IsPreferredVendor)
;

public sealed record FgsVendorInventoryItemUpdateDto(
    /// <summary>VendorId</summary>
    long VendorId,
    /// <summary>InventoryItemId</summary>
    long InventoryItemId,
    /// <summary>VendorPartNumber</summary>
    string? VendorPartNumber,
    /// <summary>Last received cost from the vendor based on purchase order receiving.</summary>
    string? VendorPartName,
    /// <summary>LastCost</summary>
    decimal LastCost,
    /// <summary>Last date inventory was received from the vendor.</summary>
    DateTimeOffset? LastReceivedDate,
    /// <summary>Comments automatically copied to purchase orders for this vendor item combination.</summary>
    string? PurchaseOrderComments,
    /// <summary>Indicates whether this vendor is the preferred vendor for the inventory item.</summary>
    bool IsPreferredVendor)
;

public sealed record FgsVendorInventoryItemPatchDto(
    /// <summary>VendorId</summary>
    long? VendorId,
    /// <summary>InventoryItemId</summary>
    long? InventoryItemId,
    /// <summary>VendorPartNumber</summary>
    string? VendorPartNumber,
    /// <summary>Last received cost from the vendor based on purchase order receiving.</summary>
    string? VendorPartName,
    /// <summary>LastCost</summary>
    decimal? LastCost,
    /// <summary>Last date inventory was received from the vendor.</summary>
    DateTimeOffset? LastReceivedDate,
    /// <summary>Comments automatically copied to purchase orders for this vendor item combination.</summary>
    string? PurchaseOrderComments,
    /// <summary>Indicates whether this vendor is the preferred vendor for the inventory item.</summary>
    bool? IsPreferredVendor)
;

