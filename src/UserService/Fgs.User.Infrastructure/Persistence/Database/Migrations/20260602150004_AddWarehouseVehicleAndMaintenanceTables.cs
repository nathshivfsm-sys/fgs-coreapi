using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.User.Infrastructure.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddWarehouseVehicleAndMaintenanceTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FgsWarehouse",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    WarehouseCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Unique warehouse code within the tenant and company scope."),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Display name of the warehouse or inventory location."),
                    WarehouseType = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, comment: "Type of inventory location. Allowed values: Warehouse, Truck, Trailer, JobSite, Consignment, Vendor."),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Optional reference to the physical address or geo location in FgsLocation."),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Optional description or notes for the warehouse."),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether this warehouse is the default inventory location for the company."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last updated the record."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the warehouse is active and available for inventory operations.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsWarehouse", x => x.Id);
                    table.UniqueConstraint("UQ_FgsWarehouse_TenantId_CompanyId_WarehouseCode", x => new { x.TenantId, x.CompanyId, x.WarehouseCode });
                    table.CheckConstraint("CK_FgsWarehouse_WarehouseType", "\"WarehouseType\" IN ('Warehouse', 'Truck', 'Trailer', 'JobSite', 'Consignment', 'Vendor')");
                    table.ForeignKey(
                        name: "FK_FgsWarehouse_FgsLocation_LocationId",
                        column: x => x.LocationId,
                        principalSchema: "shared",
                        principalTable: "FgsLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsWarehouse_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "tenant",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores inventory warehouse, truck, trailer, job site, consignment, and vendor storage locations.");

            migrationBuilder.CreateTable(
                name: "GloVehicleMaintenanceType",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaintenanceTypeCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Unique system code identifying the maintenance type."),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Display name of the maintenance type."),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Description of the maintenance type."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls display order in lists and dropdowns."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the maintenance type is active and available for selection."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloVehicleMaintenanceType", x => x.Id);
                    table.UniqueConstraint("UQ_GloVehicleMaintenanceType_MaintenanceTypeCode", x => x.MaintenanceTypeCode);
                },
                comment: "Stores standard vehicle maintenance types used when recording maintenance activities for company vehicles.");

            migrationBuilder.CreateTable(
                name: "FgsVehicle",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    WarehouseId = table.Column<long>(type: "bigint", nullable: false, comment: "Associated truck warehouse used as the vehicle inventory location."),
                    OwnershipType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Owned", comment: "Indicates whether the vehicle is owned, leased, or rented."),
                    OwnershipCompany = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Name of the leasing company, rental provider, or other organization that owns the vehicle when it is not company-owned."),
                    Year = table.Column<short>(type: "smallint", nullable: true, comment: "Vehicle model year."),
                    Make = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Vehicle manufacturer such as Ford, Chevrolet, GMC, Ram, Toyota, etc."),
                    Model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Vehicle model such as F-150, Transit, Silverado, Express, etc."),
                    Color = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "Vehicle exterior color."),
                    VIN = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Vehicle Identification Number assigned by the manufacturer."),
                    LicensePlate = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "Vehicle registration plate number."),
                    LicensePlateState = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "State or province issuing the vehicle registration."),
                    PurchaseDate = table.Column<DateOnly>(type: "date", nullable: true, comment: "Date the vehicle was purchased or acquired."),
                    PurchasePrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true, comment: "Amount paid to acquire the vehicle."),
                    PurchasedFrom = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Name of the dealership, seller, auction, fleet provider, or other source from which the vehicle was acquired."),
                    IsPurchasedNew = table.Column<bool>(type: "boolean", nullable: true, comment: "Indicates whether the vehicle was purchased new or used."),
                    Notes = table.Column<string>(type: "text", nullable: true, comment: "Internal notes and remarks regarding the vehicle."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last updated the record."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the vehicle is active and available for service operations.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsVehicle", x => x.Id);
                    table.UniqueConstraint("UQ_FgsVehicle_WarehouseId", x => x.WarehouseId);
                    table.CheckConstraint("CK_FgsVehicle_OwnershipType", "\"OwnershipType\" IN ('Owned', 'Leased', 'Rented')");
                    table.ForeignKey(
                        name: "FK_FgsVehicle_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "tenant",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsVehicle_FgsWarehouse_WarehouseId",
                        column: x => x.WarehouseId,
                        principalSchema: "setup",
                        principalTable: "FgsWarehouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores company-owned or leased vehicles used for field service operations. Each vehicle is associated with a truck warehouse that serves as an inventory location.");

            migrationBuilder.CreateTable(
                name: "FgsVehicleMaintenance",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    VehicleId = table.Column<long>(type: "bigint", nullable: false, comment: "Vehicle that received or is scheduled to receive maintenance service."),
                    VehicleMaintenanceTypeId = table.Column<int>(type: "integer", nullable: false, comment: "Type of maintenance activity being performed or scheduled."),
                    ServiceDate = table.Column<DateOnly>(type: "date", nullable: false, comment: "Date the maintenance was performed or is scheduled to be performed."),
                    MileageAtService = table.Column<int>(type: "integer", nullable: true, comment: "Vehicle odometer reading at the time the maintenance was performed."),
                    ServiceProvider = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Name of the repair shop, dealership, service provider, or maintenance vendor."),
                    InvoiceNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Vendor invoice, receipt, repair order, or work order number associated with the maintenance activity."),
                    Cost = table.Column<decimal>(type: "numeric(18,2)", nullable: true, comment: "Total cost incurred for the maintenance activity."),
                    NextServiceDate = table.Column<DateOnly>(type: "date", nullable: true, comment: "Recommended next service date based on maintenance provider recommendations."),
                    NextServiceMileage = table.Column<int>(type: "integer", nullable: true, comment: "Recommended next service mileage based on maintenance provider recommendations."),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the maintenance activity has been completed. False indicates a scheduled or pending maintenance item."),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Short summary of the maintenance activity performed or scheduled."),
                    Notes = table.Column<string>(type: "text", nullable: true, comment: "Detailed notes, observations, recommendations, or repair information related to the maintenance activity."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsVehicleMaintenance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsVehicleMaintenance_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "tenant",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsVehicleMaintenance_FgsVehicle_VehicleId",
                        column: x => x.VehicleId,
                        principalSchema: "setup",
                        principalTable: "FgsVehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsVehicleMaintenance_GloVehicleMaintenanceType_VehicleMaintenanceTypeId",
                        column: x => x.VehicleMaintenanceTypeId,
                        principalSchema: "glo",
                        principalTable: "GloVehicleMaintenanceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores completed and scheduled maintenance activities, inspections, repairs, and service history for company vehicles.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicle_TenantId_CompanyId_IsActive",
                schema: "setup",
                table: "FgsVehicle",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicle_TenantId_CompanyId_WarehouseId",
                schema: "setup",
                table: "FgsVehicle",
                columns: new[] { "TenantId", "CompanyId", "WarehouseId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicleMaintenance_TenantId_CompanyId_IsCompleted",
                schema: "setup",
                table: "FgsVehicleMaintenance",
                columns: new[] { "TenantId", "CompanyId", "IsCompleted" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicleMaintenance_TenantId_CompanyId_NextServiceDate",
                schema: "setup",
                table: "FgsVehicleMaintenance",
                columns: new[] { "TenantId", "CompanyId", "NextServiceDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicleMaintenance_TenantId_CompanyId_ServiceDate",
                schema: "setup",
                table: "FgsVehicleMaintenance",
                columns: new[] { "TenantId", "CompanyId", "ServiceDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicleMaintenance_TenantId_CompanyId_VehicleId",
                schema: "setup",
                table: "FgsVehicleMaintenance",
                columns: new[] { "TenantId", "CompanyId", "VehicleId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicleMaintenance_TenantId_CompanyId_VehicleMaintenanceTypeId",
                schema: "setup",
                table: "FgsVehicleMaintenance",
                columns: new[] { "TenantId", "CompanyId", "VehicleMaintenanceTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicleMaintenance_VehicleId",
                schema: "setup",
                table: "FgsVehicleMaintenance",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicleMaintenance_VehicleMaintenanceTypeId",
                schema: "setup",
                table: "FgsVehicleMaintenance",
                column: "VehicleMaintenanceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsWarehouse_LocationId",
                schema: "setup",
                table: "FgsWarehouse",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsWarehouse_TenantId_CompanyId_IsActive",
                schema: "setup",
                table: "FgsWarehouse",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsWarehouse_TenantId_CompanyId_WarehouseType",
                schema: "setup",
                table: "FgsWarehouse",
                columns: new[] { "TenantId", "CompanyId", "WarehouseType" });

            migrationBuilder.CreateIndex(
                name: "IX_GloVehicleMaintenanceType_DisplayOrder",
                schema: "glo",
                table: "GloVehicleMaintenanceType",
                column: "DisplayOrder");

            migrationBuilder.Sql(
                """
                INSERT INTO glo."GloVehicleMaintenanceType"
                (
                    "MaintenanceTypeCode",
                    "Name",
                    "Description",
                    "DisplayOrder"
                )
                SELECT
                    v."MaintenanceTypeCode",
                    v."Name",
                    v."Description",
                    v."DisplayOrder"
                FROM (
                    VALUES
                        ('OIL_CHANGE',           'Oil Change',           'Engine oil and filter replacement.',                              1::smallint),
                        ('TIRE_ROTATION',        'Tire Rotation',        'Rotation of vehicle tires to promote even wear.',                 2::smallint),
                        ('TIRE_REPLACEMENT',     'Tire Replacement',     'Replacement of one or more vehicle tires.',                       3::smallint),
                        ('BRAKE_SERVICE',        'Brake Service',        'Inspection, repair, or replacement of brake components.',           4::smallint),
                        ('INSPECTION',           'Inspection',           'General vehicle inspection and safety check.',                    5::smallint),
                        ('BATTERY_REPLACEMENT',  'Battery Replacement',  'Replacement of vehicle battery.',                                 6::smallint),
                        ('TRANSMISSION_SERVICE', 'Transmission Service', 'Maintenance or repair of transmission system.',                   7::smallint),
                        ('REGISTRATION_RENEWAL', 'Registration Renewal', 'Vehicle registration renewal.',                                   8::smallint),
                        ('REPAIR',               'Repair',               'General repair work not covered by a specific maintenance type.', 9::smallint),
                        ('OTHER',                'Other',                'Other maintenance activity.',                                     99::smallint)
                ) AS v("MaintenanceTypeCode", "Name", "Description", "DisplayOrder")
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM glo."GloVehicleMaintenanceType" t
                    WHERE t."MaintenanceTypeCode" = v."MaintenanceTypeCode"
                );

                SELECT setval(
                    pg_get_serial_sequence('glo."GloVehicleMaintenanceType"', 'Id'),
                    COALESCE((SELECT MAX("Id") FROM glo."GloVehicleMaintenanceType"), 1),
                    true);

                INSERT INTO glo."GloMasterEntityType"
                (
                    "Code",
                    "IsDocumentAllowed",
                    "SortOrder",
                    "IsActive",
                    "CreatedOn",
                    "CreatedBy"
                )
                SELECT
                    v."Code",
                    v."IsDocumentAllowed",
                    v."SortOrder",
                    v."IsActive",
                    timezone('utc', now()),
                    'System'
                FROM (
                    VALUES
                        ('Warehouse',          true, 12, true),
                        ('Vehicle',            true, 13, true),
                        ('VehicleMaintenance', true, 14, true)
                ) AS v("Code", "IsDocumentAllowed", "SortOrder", "IsActive")
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM glo."GloMasterEntityType" t
                    WHERE t."Code" = v."Code"
                );

                SELECT setval(
                    pg_get_serial_sequence('glo."GloMasterEntityType"', 'Id'),
                    COALESCE((SELECT MAX("Id") FROM glo."GloMasterEntityType"), 1),
                    true);
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsVehicleMaintenance",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsVehicle",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "GloVehicleMaintenanceType",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "FgsWarehouse",
                schema: "setup");
        }
    }
}
