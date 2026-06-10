using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsWarehouseDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.Warehouse,
        EntityName: "FgsWarehouse",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsWarehouse),
        SummaryDtoType: typeof(FgsWarehouseSummaryDto),
        DetailDtoType: typeof(FgsWarehouseDetailDto),
        CreateDtoType: typeof(FgsWarehouseCreateDto),
        UpdateDtoType: typeof(FgsWarehouseUpdateDto),
        PatchDtoType: typeof(FgsWarehousePatchDto),
        TableName: "FgsWarehouse",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "warehouses",
        SwaggerTag: "Setup - Warehouses",
        TableComment: "FgsWarehouse",
        SupportsSoftDelete: true,
        Columns:
        [
            new CatalogEntityColumnDescriptor(
                "Id", "Id", typeof(long), false, null, true, false, true, "Primary key."),
            new CatalogEntityColumnDescriptor(
                "TenantId", "TenantId", typeof(long), false, null, true, false, false, "Tenant identifier."),
            new CatalogEntityColumnDescriptor(
                "CompanyId", "CompanyId", typeof(long), false, null, true, false, false, "Company identifier."),
            new CatalogEntityColumnDescriptor(
                "WarehouseCode", "WarehouseCode", typeof(string), false, 0, false, true, true, "Unique warehouse code within the tenant and company scope."),
            new CatalogEntityColumnDescriptor(
                "Name", "Name", typeof(string), false, 200, false, true, true, "Display name of the warehouse or inventory location."),
            new CatalogEntityColumnDescriptor(
                "WarehouseType", "WarehouseType", typeof(string), false, 30, false, true, true, "Optional reference to the warehouse address record."),
            new CatalogEntityColumnDescriptor(
                "AddressId", "AddressId", typeof(Guid?), false, null, false, false, true, "AddressId"),
            new CatalogEntityColumnDescriptor(
                "Description", "Description", typeof(string), false, 0, false, true, true, "Optional description or notes for the warehouse."),
            new CatalogEntityColumnDescriptor(
                "IsDefault", "IsDefault", typeof(bool), true, null, false, false, true, "Indicates whether this warehouse is the default inventory location for the company."),
            new CatalogEntityColumnDescriptor(
                "CreatedOn", "CreatedOn", typeof(DateTimeOffset), false, null, true, false, false, "Date and time the record was created."),
            new CatalogEntityColumnDescriptor(
                "CreatedBy", "CreatedBy", typeof(string), false, 0, true, false, false, "User who created the record."),
            new CatalogEntityColumnDescriptor(
                "UpdatedOn", "UpdatedOn", typeof(DateTimeOffset?), false, null, true, false, false, "Date and time the record was last updated."),
            new CatalogEntityColumnDescriptor(
                "UpdatedBy", "UpdatedBy", typeof(string), false, 0, true, false, false, "User who last updated the record."),
            new CatalogEntityColumnDescriptor(
                "IsActive", "IsActive", typeof(bool), true, null, false, false, true, "Indicates whether the warehouse is active and available for inventory operations."),
        ],
        UniqueKeys:
        [
            new CatalogEntityUniqueKeyDescriptor("UQ_FgsWarehouse", ["TenantId", "CompanyId", "WarehouseCode"]),
        ],
        SearchableColumns: ["WarehouseCode", "Name", "WarehouseType", "Description"],
        SortableColumns: ["Id", "WarehouseCode", "Name", "WarehouseType", "AddressId", "Description", "IsDefault", "IsActive"]);
}
