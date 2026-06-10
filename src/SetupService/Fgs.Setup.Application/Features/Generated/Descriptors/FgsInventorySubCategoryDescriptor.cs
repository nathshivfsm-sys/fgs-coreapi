using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsInventorySubCategoryDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.InventorySubCategory,
        EntityName: "FgsInventorySubCategory",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsInventorySubCategory),
        SummaryDtoType: typeof(FgsInventorySubCategorySummaryDto),
        DetailDtoType: typeof(FgsInventorySubCategoryDetailDto),
        CreateDtoType: typeof(FgsInventorySubCategoryCreateDto),
        UpdateDtoType: typeof(FgsInventorySubCategoryUpdateDto),
        PatchDtoType: typeof(FgsInventorySubCategoryPatchDto),
        TableName: "FgsInventorySubCategory",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "inventorysubcategories",
        SwaggerTag: "Setup - Inventory",
        TableComment: "FgsInventorySubCategory",
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
                "InventoryCategoryId", "InventoryCategoryId", typeof(long), true, null, false, false, true, "InventoryCategoryId"),
            new CatalogEntityColumnDescriptor(
                "SubCategoryCode", "SubCategoryCode", typeof(string), false, 0, false, true, true, "SubCategoryCode"),
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
            new CatalogEntityUniqueKeyDescriptor("UQ_FgsInventorySubCategory", ["TenantId", "CompanyId", "InventoryCategoryId", "SubCategoryCode"]),
        ],
        SearchableColumns: ["SubCategoryCode", "Name", "Description"],
        SortableColumns: ["Id", "InventoryCategoryId", "SubCategoryCode", "Name", "Description", "DisplayOrder", "IsSystem", "IsActive"]);
}
