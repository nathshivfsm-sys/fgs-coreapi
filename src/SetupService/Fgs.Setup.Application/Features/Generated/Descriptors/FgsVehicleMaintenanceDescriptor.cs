using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsVehicleMaintenanceDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.VehicleMaintenance,
        EntityName: "FgsVehicleMaintenance",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsVehicleMaintenance),
        SummaryDtoType: typeof(FgsVehicleMaintenanceSummaryDto),
        DetailDtoType: typeof(FgsVehicleMaintenanceDetailDto),
        CreateDtoType: typeof(FgsVehicleMaintenanceCreateDto),
        UpdateDtoType: typeof(FgsVehicleMaintenanceUpdateDto),
        PatchDtoType: typeof(FgsVehicleMaintenancePatchDto),
        TableName: "FgsVehicleMaintenance",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.HardDeleteScoped,
        RoutePlural: "vehiclemaintenances",
        SwaggerTag: "Setup - Vehicles",
        TableComment: "FgsVehicleMaintenance",
        SupportsSoftDelete: false,
        Columns:
        [
            new CatalogEntityColumnDescriptor(
                "Id", "Id", typeof(long), false, null, true, false, true, "Primary key."),
            new CatalogEntityColumnDescriptor(
                "TenantId", "TenantId", typeof(long), false, null, true, false, false, "Tenant identifier."),
            new CatalogEntityColumnDescriptor(
                "CompanyId", "CompanyId", typeof(long), false, null, true, false, false, "Company identifier."),
            new CatalogEntityColumnDescriptor(
                "VehicleId", "VehicleId", typeof(long), true, null, false, false, true, "Vehicle that received or is scheduled to receive maintenance service."),
            new CatalogEntityColumnDescriptor(
                "VehicleMaintenanceTypeId", "VehicleMaintenanceTypeId", typeof(int), true, null, false, false, true, "Type of maintenance activity being performed or scheduled."),
            new CatalogEntityColumnDescriptor(
                "MileageAtService", "MileageAtService", typeof(int?), false, null, false, false, true, "Vehicle odometer reading at the time the maintenance was performed."),
            new CatalogEntityColumnDescriptor(
                "ServiceProvider", "ServiceProvider", typeof(string), false, 0, false, true, true, "Name of the repair shop, dealership, service provider, or maintenance vendor."),
            new CatalogEntityColumnDescriptor(
                "InvoiceNumber", "InvoiceNumber", typeof(string), false, 100, false, true, true, "Total cost incurred for the maintenance activity."),
            new CatalogEntityColumnDescriptor(
                "Cost", "Cost", typeof(decimal?), false, null, false, false, true, "Cost"),
            new CatalogEntityColumnDescriptor(
                "NextServiceMileage", "NextServiceMileage", typeof(int?), false, null, false, false, true, "Recommended next service mileage based on maintenance provider recommendations."),
            new CatalogEntityColumnDescriptor(
                "IsCompleted", "IsCompleted", typeof(bool), true, null, false, false, true, "Short summary of the maintenance activity performed or scheduled."),
            new CatalogEntityColumnDescriptor(
                "Description", "Description", typeof(string), false, 0, false, true, true, "Description"),
            new CatalogEntityColumnDescriptor(
                "Notes", "Notes", typeof(string), false, 100, false, true, true, "Date and time the record was created."),
            new CatalogEntityColumnDescriptor(
                "CreatedOn", "CreatedOn", typeof(DateTimeOffset), false, null, true, false, false, "CreatedOn"),
            new CatalogEntityColumnDescriptor(
                "CreatedBy", "CreatedBy", typeof(string), false, 0, true, false, false, "User who created the record."),
            new CatalogEntityColumnDescriptor(
                "UpdatedOn", "UpdatedOn", typeof(DateTimeOffset?), false, null, true, false, false, "Date and time the record was last updated."),
            new CatalogEntityColumnDescriptor(
                "UpdatedBy", "UpdatedBy", typeof(string), false, 0, true, false, false, "User who last updated the record."),
        ],
        UniqueKeys:
        [
        ],
        SearchableColumns: ["ServiceProvider", "InvoiceNumber", "Description", "Notes"],
        SortableColumns: ["Id", "VehicleId", "VehicleMaintenanceTypeId", "MileageAtService", "ServiceProvider", "InvoiceNumber", "Cost", "NextServiceMileage", "IsCompleted", "Description", "Notes"]);
}
