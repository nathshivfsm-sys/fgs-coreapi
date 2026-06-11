using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsInventoryItemTypeDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.InventoryItemType,
        EntityName: "FgsInventoryItemType",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsInventoryItemType),
        SummaryDtoType: typeof(FgsInventoryItemTypeSummaryDto),
        DetailDtoType: typeof(FgsInventoryItemTypeDetailDto),
        CreateDtoType: typeof(FgsInventoryItemTypeCreateDto),
        UpdateDtoType: typeof(FgsInventoryItemTypeUpdateDto),
        PatchDtoType: typeof(FgsInventoryItemTypePatchDto),
        TableName: "FgsInventoryItemType",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "inventoryitemtypes",
        SwaggerTag: "Setup - Inventory",
        TableComment: "FgsInventoryItemType",
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
                "ItemTypeCode", "ItemTypeCode", typeof(string), false, 0, false, true, true, "ItemTypeCode"),
            new CatalogEntityColumnDescriptor(
                "Name", "Name", typeof(string), false, 50, false, true, true, "Name"),
            new CatalogEntityColumnDescriptor(
                "Description", "Description", typeof(string), false, 0, false, true, true, "Description"),
            new CatalogEntityColumnDescriptor(
                "TracksQuantity", "TracksQuantity", typeof(bool), true, null, false, false, true, "TracksQuantity"),
            new CatalogEntityColumnDescriptor(
                "DisplayOrder", "DisplayOrder", typeof(short), true, null, false, false, true, "DisplayOrder"),
            new CatalogEntityColumnDescriptor(
                "IsSystem", "IsSystem", typeof(bool), true, null, false, false, true, "IsSystem"),
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
            new CatalogEntityUniqueKeyDescriptor("UQ_FgsInventoryItemType", ["TenantId", "CompanyId", "ItemTypeCode"]),
        ],
        SearchableColumns: ["ItemTypeCode", "Name", "Description"],
        SortableColumns: ["Id", "ItemTypeCode", "Name", "Description", "TracksQuantity", "DisplayOrder", "IsSystem", "IsActive"]);
}
