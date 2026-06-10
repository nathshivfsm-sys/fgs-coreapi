using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsInventoryItemDependencyDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.InventoryItemDependency,
        EntityName: "FgsInventoryItemDependency",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsInventoryItemDependency),
        SummaryDtoType: typeof(FgsInventoryItemDependencySummaryDto),
        DetailDtoType: typeof(FgsInventoryItemDependencyDetailDto),
        CreateDtoType: typeof(FgsInventoryItemDependencyCreateDto),
        UpdateDtoType: typeof(FgsInventoryItemDependencyUpdateDto),
        PatchDtoType: typeof(FgsInventoryItemDependencyPatchDto),
        TableName: "FgsInventoryItemDependency",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "inventoryitemdependencies",
        SwaggerTag: "Setup - Inventory",
        TableComment: "FgsInventoryItemDependency",
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
                "InventoryItemId", "InventoryItemId", typeof(long), true, null, false, false, true, "InventoryItemId"),
            new CatalogEntityColumnDescriptor(
                "DependentInventoryItemId", "DependentInventoryItemId", typeof(long), true, null, false, false, true, "DependentInventoryItemId"),
            new CatalogEntityColumnDescriptor(
                "Quantity", "Quantity", typeof(decimal), true, null, false, false, true, "Quantity"),
            new CatalogEntityColumnDescriptor(
                "DependencyType", "DependencyType", typeof(string), false, 0, false, true, true, "DependencyType"),
            new CatalogEntityColumnDescriptor(
                "IsRequired", "IsRequired", typeof(bool), true, null, false, false, true, "IsRequired"),
            new CatalogEntityColumnDescriptor(
                "Notes", "Notes", typeof(string), false, 0, false, true, true, "Notes"),
            new CatalogEntityColumnDescriptor(
                "DisplayOrder", "DisplayOrder", typeof(short), true, null, false, false, true, "DisplayOrder"),
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
            new CatalogEntityUniqueKeyDescriptor("UQ_FgsInventoryItemDependency", ["TenantId", "CompanyId", "InventoryItemId", "DependentInventoryItemId"]),
        ],
        SearchableColumns: ["DependencyType", "Notes"],
        SortableColumns: ["Id", "InventoryItemId", "DependentInventoryItemId", "Quantity", "DependencyType", "IsRequired", "Notes", "DisplayOrder", "IsActive"]);
}
