using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsInventoryItemDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.InventoryItem,
        EntityName: "FgsInventoryItem",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsInventoryItem),
        SummaryDtoType: typeof(FgsInventoryItemSummaryDto),
        DetailDtoType: typeof(FgsInventoryItemDetailDto),
        CreateDtoType: typeof(FgsInventoryItemCreateDto),
        UpdateDtoType: typeof(FgsInventoryItemUpdateDto),
        PatchDtoType: typeof(FgsInventoryItemPatchDto),
        TableName: "FgsInventoryItem",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "inventoryitems",
        SwaggerTag: "Setup - Inventory",
        TableComment: "FgsInventoryItem",
        SupportsSoftDelete: true,
        Columns:
        [
            new CatalogEntityColumnDescriptor(
                "Id", "Id", typeof(long), false, null, true, false, true, "Id"),
            new CatalogEntityColumnDescriptor(
                "TenantId", "TenantId", typeof(long), false, null, true, false, false, "TenantId"),
            new CatalogEntityColumnDescriptor(
                "CompanyId", "CompanyId", typeof(long), false, null, true, false, false, "CompanyId"),
            new CatalogEntityColumnDescriptor(
                "InventoryItemTypeId", "InventoryItemTypeId", typeof(long), true, null, false, false, true, "InventoryItemTypeId"),
            new CatalogEntityColumnDescriptor(
                "InventoryCategoryId", "InventoryCategoryId", typeof(long?), false, null, false, false, true, "InventoryCategoryId"),
            new CatalogEntityColumnDescriptor(
                "InventorySubCategoryId", "InventorySubCategoryId", typeof(long?), false, null, false, false, true, "InventorySubCategoryId"),
            new CatalogEntityColumnDescriptor(
                "ItemCode", "ItemCode", typeof(string), false, 0, false, true, true, "ItemCode"),
            new CatalogEntityColumnDescriptor(
                "Name", "Name", typeof(string), false, 200, false, true, true, "Name"),
            new CatalogEntityColumnDescriptor(
                "Description", "Description", typeof(string), false, 100, false, true, true, "Description"),
            new CatalogEntityColumnDescriptor(
                "PurchaseDescription", "PurchaseDescription", typeof(string), false, 0, false, true, true, "PurchaseDescription"),
            new CatalogEntityColumnDescriptor(
                "SalesDescription", "SalesDescription", typeof(string), false, 0, false, true, true, "SalesDescription"),
            new CatalogEntityColumnDescriptor(
                "ManufacturerPartNumber", "ManufacturerPartNumber", typeof(string), false, 0, false, true, true, "ManufacturerPartNumber"),
            new CatalogEntityColumnDescriptor(
                "UPCCode", "UPCCode", typeof(string), false, 100, false, true, true, "UPCCode"),
            new CatalogEntityColumnDescriptor(
                "UnitOfMeasure", "UnitOfMeasure", typeof(string), false, 50, false, true, true, "UnitOfMeasure"),
            new CatalogEntityColumnDescriptor(
                "TrackQuantity", "TrackQuantity", typeof(bool), true, null, false, false, true, "TrackQuantity"),
            new CatalogEntityColumnDescriptor(
                "Cost", "Cost", typeof(decimal), true, null, false, false, true, "Cost"),
            new CatalogEntityColumnDescriptor(
                "SalesPrice", "SalesPrice", typeof(decimal), true, null, false, false, true, "SalesPrice"),
            new CatalogEntityColumnDescriptor(
                "DefaultTaxable", "DefaultTaxable", typeof(bool), true, null, false, false, true, "DefaultTaxable"),
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
            new CatalogEntityUniqueKeyDescriptor("UQ_FgsInventoryItem", ["TenantId", "CompanyId", "ItemCode"]),
        ],
        SearchableColumns: ["ItemCode", "Name", "Description", "PurchaseDescription", "SalesDescription", "ManufacturerPartNumber", "UPCCode", "UnitOfMeasure"],
        SortableColumns: ["Id", "InventoryItemTypeId", "InventoryCategoryId", "InventorySubCategoryId", "ItemCode", "Name", "Description", "PurchaseDescription", "SalesDescription", "ManufacturerPartNumber", "UPCCode", "UnitOfMeasure", "TrackQuantity", "Cost", "SalesPrice", "DefaultTaxable", "IsActive"]);
}
