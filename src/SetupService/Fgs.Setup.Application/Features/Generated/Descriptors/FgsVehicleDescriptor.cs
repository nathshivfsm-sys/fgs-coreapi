using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsVehicleDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.Vehicle,
        EntityName: "FgsVehicle",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsVehicle),
        SummaryDtoType: typeof(FgsVehicleSummaryDto),
        DetailDtoType: typeof(FgsVehicleDetailDto),
        CreateDtoType: typeof(FgsVehicleCreateDto),
        UpdateDtoType: typeof(FgsVehicleUpdateDto),
        PatchDtoType: typeof(FgsVehiclePatchDto),
        TableName: "FgsVehicle",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "vehicles",
        SwaggerTag: "Setup - Vehicles",
        TableComment: "FgsVehicle",
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
                "WarehouseId", "WarehouseId", typeof(long), true, null, false, false, true, "Associated truck warehouse used as the vehicle inventory location."),
            new CatalogEntityColumnDescriptor(
                "OwnershipType", "OwnershipType", typeof(string), false, 0, false, true, true, "Indicates whether the vehicle is owned, leased, or rented."),
            new CatalogEntityColumnDescriptor(
                "OwnershipCompany", "OwnershipCompany", typeof(string), false, 200, false, true, true, "Vehicle model year."),
            new CatalogEntityColumnDescriptor(
                "Year", "Year", typeof(short?), false, null, false, false, true, "Year"),
            new CatalogEntityColumnDescriptor(
                "Make", "Make", typeof(string), false, 0, false, true, true, "Vehicle manufacturer such as Ford, Chevrolet, GMC, Ram, Toyota, etc."),
            new CatalogEntityColumnDescriptor(
                "Model", "Model", typeof(string), false, 100, false, true, true, "Vehicle model such as F-150, Transit, Silverado, Express, etc."),
            new CatalogEntityColumnDescriptor(
                "Color", "Color", typeof(string), false, 50, false, true, true, "Vehicle exterior color."),
            new CatalogEntityColumnDescriptor(
                "VIN", "VIN", typeof(string), false, 50, false, true, true, "Vehicle Identification Number assigned by the manufacturer."),
            new CatalogEntityColumnDescriptor(
                "LicensePlate", "LicensePlate", typeof(string), false, 50, false, true, true, "Vehicle registration plate number."),
            new CatalogEntityColumnDescriptor(
                "LicensePlateState", "LicensePlateState", typeof(string), false, 50, false, true, true, "State or province issuing the vehicle registration."),
            new CatalogEntityColumnDescriptor(
                "PurchasePrice", "PurchasePrice", typeof(decimal?), false, null, false, false, true, "Amount paid to acquire the vehicle."),
            new CatalogEntityColumnDescriptor(
                "PurchasedFrom", "PurchasedFrom", typeof(string), false, 0, false, true, true, "Indicates whether the vehicle was purchased new or used."),
            new CatalogEntityColumnDescriptor(
                "IsPurchasedNew", "IsPurchasedNew", typeof(bool?), false, null, false, false, true, "IsPurchasedNew"),
            new CatalogEntityColumnDescriptor(
                "Notes", "Notes", typeof(string), false, 0, false, true, true, "Internal notes and remarks regarding the vehicle."),
            new CatalogEntityColumnDescriptor(
                "CreatedOn", "CreatedOn", typeof(DateTimeOffset), false, null, true, false, false, "Date and time the record was created."),
            new CatalogEntityColumnDescriptor(
                "CreatedBy", "CreatedBy", typeof(string), false, 0, true, false, false, "User who created the record."),
            new CatalogEntityColumnDescriptor(
                "UpdatedOn", "UpdatedOn", typeof(DateTimeOffset?), false, null, true, false, false, "Date and time the record was last updated."),
            new CatalogEntityColumnDescriptor(
                "UpdatedBy", "UpdatedBy", typeof(string), false, 0, true, false, false, "User who last updated the record."),
            new CatalogEntityColumnDescriptor(
                "IsActive", "IsActive", typeof(bool), true, null, false, false, true, "Indicates whether the vehicle is active and available for service operations."),
        ],
        UniqueKeys:
        [
        ],
        SearchableColumns: ["OwnershipType", "OwnershipCompany", "Make", "Model", "Color", "VIN", "LicensePlate", "LicensePlateState", "PurchasedFrom", "Notes"],
        SortableColumns: ["Id", "WarehouseId", "OwnershipType", "OwnershipCompany", "Year", "Make", "Model", "Color", "VIN", "LicensePlate", "LicensePlateState", "PurchasePrice", "PurchasedFrom", "IsPurchasedNew", "Notes", "IsActive"]);
}
