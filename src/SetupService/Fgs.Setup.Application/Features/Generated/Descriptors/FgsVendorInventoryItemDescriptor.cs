using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsVendorInventoryItemDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.VendorInventoryItem,
        EntityName: "FgsVendorInventoryItem",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsVendorInventoryItem),
        SummaryDtoType: typeof(FgsVendorInventoryItemSummaryDto),
        DetailDtoType: typeof(FgsVendorInventoryItemDetailDto),
        CreateDtoType: typeof(FgsVendorInventoryItemCreateDto),
        UpdateDtoType: typeof(FgsVendorInventoryItemUpdateDto),
        PatchDtoType: typeof(FgsVendorInventoryItemPatchDto),
        TableName: "FgsVendorInventoryItem",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "vendorinventoryitems",
        SwaggerTag: "Setup - Vendors",
        TableComment: "FgsVendorInventoryItem",
        SupportsSoftDelete: true,
        Columns:
        [
            new CatalogEntityColumnDescriptor(
                "Id", "Id", typeof(long), false, null, true, false, true, "Vendor-specific part number for the inventory item."),
            new CatalogEntityColumnDescriptor(
                "TenantId", "TenantId", typeof(long), false, null, true, false, false, "TenantId"),
            new CatalogEntityColumnDescriptor(
                "CompanyId", "CompanyId", typeof(long), false, null, true, false, false, "CompanyId"),
            new CatalogEntityColumnDescriptor(
                "VendorId", "VendorId", typeof(long), true, null, false, false, true, "VendorId"),
            new CatalogEntityColumnDescriptor(
                "InventoryItemId", "InventoryItemId", typeof(long), true, null, false, false, true, "InventoryItemId"),
            new CatalogEntityColumnDescriptor(
                "VendorPartNumber", "VendorPartNumber", typeof(string), false, 0, false, true, true, "VendorPartNumber"),
            new CatalogEntityColumnDescriptor(
                "VendorPartName", "VendorPartName", typeof(string), false, 200, false, true, true, "Last received cost from the vendor based on purchase order receiving."),
            new CatalogEntityColumnDescriptor(
                "LastCost", "LastCost", typeof(decimal), true, null, false, false, true, "LastCost"),
            new CatalogEntityColumnDescriptor(
                "LastReceivedDate", "LastReceivedDate", typeof(DateTimeOffset?), false, null, false, false, true, "Last date inventory was received from the vendor."),
            new CatalogEntityColumnDescriptor(
                "PurchaseOrderComments", "PurchaseOrderComments", typeof(string), false, 0, false, true, true, "Comments automatically copied to purchase orders for this vendor item combination."),
            new CatalogEntityColumnDescriptor(
                "IsPreferredVendor", "IsPreferredVendor", typeof(bool), true, null, false, false, true, "Indicates whether this vendor is the preferred vendor for the inventory item."),
            new CatalogEntityColumnDescriptor(
                "CreatedOn", "CreatedOn", typeof(DateTimeOffset), false, null, true, false, false, "CreatedOn"),
            new CatalogEntityColumnDescriptor(
                "CreatedBy", "CreatedBy", typeof(string), false, 0, true, false, false, "CreatedBy"),
            new CatalogEntityColumnDescriptor(
                "UpdatedOn", "UpdatedOn", typeof(DateTimeOffset?), false, null, true, false, false, "UpdatedOn"),
            new CatalogEntityColumnDescriptor(
                "UpdatedBy", "UpdatedBy", typeof(string), false, 0, true, false, false, "UpdatedBy"),
            new CatalogEntityColumnDescriptor(
                "IsActive", "IsActive", typeof(bool), true, null, false, false, true, "IsActive"),
        ],
        UniqueKeys:
        [
            new CatalogEntityUniqueKeyDescriptor("UQ_FgsVendorInventoryItem", ["TenantId", "CompanyId", "VendorId", "InventoryItemId"]),
        ],
        SearchableColumns: ["VendorPartNumber", "VendorPartName", "PurchaseOrderComments"],
        SortableColumns: ["Id", "VendorId", "InventoryItemId", "VendorPartNumber", "VendorPartName", "LastCost", "LastReceivedDate", "PurchaseOrderComments", "IsPreferredVendor", "IsActive"]);
}
