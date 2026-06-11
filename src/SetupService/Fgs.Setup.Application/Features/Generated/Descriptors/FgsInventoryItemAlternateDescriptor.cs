using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsInventoryItemAlternateDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.InventoryItemAlternate,
        EntityName: "FgsInventoryItemAlternate",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsInventoryItemAlternate),
        SummaryDtoType: typeof(FgsInventoryItemAlternateSummaryDto),
        DetailDtoType: typeof(FgsInventoryItemAlternateDetailDto),
        CreateDtoType: typeof(FgsInventoryItemAlternateCreateDto),
        UpdateDtoType: typeof(FgsInventoryItemAlternateUpdateDto),
        PatchDtoType: typeof(FgsInventoryItemAlternatePatchDto),
        TableName: "FgsInventoryItemAlternate",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "inventoryitemalternates",
        SwaggerTag: "Setup - Inventory",
        TableComment: "FgsInventoryItemAlternate",
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
                "AlternateInventoryItemId", "AlternateInventoryItemId", typeof(long), true, null, false, false, true, "AlternateInventoryItemId"),
            new CatalogEntityColumnDescriptor(
                "AlternateType", "AlternateType", typeof(string), false, 0, false, true, true, "AlternateType"),
            new CatalogEntityColumnDescriptor(
                "PriorityOrder", "PriorityOrder", typeof(short), true, null, false, false, true, "PriorityOrder"),
            new CatalogEntityColumnDescriptor(
                "Notes", "Notes", typeof(string), false, 0, false, true, true, "Notes"),
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
            new CatalogEntityUniqueKeyDescriptor("UQ_FgsInventoryItemAlternate", ["TenantId", "CompanyId", "InventoryItemId", "AlternateInventoryItemId"]),
        ],
        SearchableColumns: ["AlternateType", "Notes"],
        SortableColumns: ["Id", "InventoryItemId", "AlternateInventoryItemId", "AlternateType", "PriorityOrder", "Notes", "IsActive"]);
}
