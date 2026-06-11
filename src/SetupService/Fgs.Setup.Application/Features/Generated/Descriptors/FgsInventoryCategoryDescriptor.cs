using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsInventoryCategoryDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.InventoryCategory,
        EntityName: "FgsInventoryCategory",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsInventoryCategory),
        SummaryDtoType: typeof(FgsInventoryCategorySummaryDto),
        DetailDtoType: typeof(FgsInventoryCategoryDetailDto),
        CreateDtoType: typeof(FgsInventoryCategoryCreateDto),
        UpdateDtoType: typeof(FgsInventoryCategoryUpdateDto),
        PatchDtoType: typeof(FgsInventoryCategoryPatchDto),
        TableName: "FgsInventoryCategory",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "inventorycategories",
        SwaggerTag: "Setup - Inventory",
        TableComment: "FgsInventoryCategory",
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
                "CategoryCode", "CategoryCode", typeof(string), false, 0, false, true, true, "CategoryCode"),
            new CatalogEntityColumnDescriptor(
                "Name", "Name", typeof(string), false, 150, false, true, true, "Name"),
            new CatalogEntityColumnDescriptor(
                "Description", "Description", typeof(string), false, 0, false, true, true, "Description"),
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
            new CatalogEntityUniqueKeyDescriptor("UQ_FgsInventoryCategory", ["TenantId", "CompanyId", "CategoryCode"]),
        ],
        SearchableColumns: ["CategoryCode", "Name", "Description"],
        SortableColumns: ["Id", "CategoryCode", "Name", "Description", "DisplayOrder", "IsSystem", "IsActive"]);
}
