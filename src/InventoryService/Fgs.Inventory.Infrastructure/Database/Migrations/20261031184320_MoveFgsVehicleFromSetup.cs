using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Inventory.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class MoveFgsVehicleFromSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FgsVehicle",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    InventoryLocationId = table.Column<long>(type: "bigint", nullable: false, comment: "Associated truck inventory location."),
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
                    table.UniqueConstraint("UQ_FgsVehicle_InventoryLocationId", x => x.InventoryLocationId);
                    table.CheckConstraint("CK_FgsVehicle_OwnershipType", "\"OwnershipType\" IN ('Owned', 'Leased', 'Rented')");
                    table.ForeignKey(
                        name: "FK_FgsVehicle_FgsInventoryLocation_InventoryLocationId",
                        column: x => x.InventoryLocationId,
                        principalSchema: "inventory",
                        principalTable: "FgsInventoryLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsVehicle_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "inventory",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores company-owned or leased vehicles used for field service operations. Each vehicle is associated with an inventory location that serves as a truck storage location.");

            migrationBuilder.CreateTable(
                name: "FgsVehicleMaintenance",
                schema: "inventory",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VehicleId = table.Column<long>(type: "bigint", nullable: false, comment: "Vehicle that received or is scheduled to receive maintenance service."),
                    VehicleMaintenanceTypeId = table.Column<int>(type: "integer", nullable: false, comment: "Type of maintenance activity being performed or scheduled. References glo.GloVehicleMaintenanceType as scalar only."),
                    ServiceDate = table.Column<DateOnly>(type: "date", nullable: false, comment: "Date the maintenance was performed or is scheduled to be performed."),
                    MileageAtService = table.Column<int>(type: "integer", nullable: true, comment: "Vehicle odometer reading at the time the maintenance was performed."),
                    ServiceProvider = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Name of the repair shop, dealership, service provider, or maintenance vendor."),
                    InvoiceNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Vendor invoice, receipt, repair order, or work order number associated with the maintenance activity."),
                    Cost = table.Column<decimal>(type: "numeric(18,2)", nullable: true, comment: "Total cost incurred for the maintenance activity."),
                    NextServiceDate = table.Column<DateOnly>(type: "date", nullable: true, comment: "Recommended next service date based on maintenance provider recommendations."),
                    NextServiceMileage = table.Column<int>(type: "integer", nullable: true, comment: "Recommended next service mileage based on maintenance provider recommendations."),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the maintenance activity has been completed. False indicates a scheduled or pending maintenance item."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the maintenance record is active and available for use."),
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
                        name: "FK_FgsVehicleMaintenance_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "inventory",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsVehicleMaintenance_FgsVehicle_VehicleId",
                        column: x => x.VehicleId,
                        principalSchema: "inventory",
                        principalTable: "FgsVehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores completed and scheduled maintenance activities, inspections, repairs, and service history for company vehicles.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicle_TenantId_CompanyId_InventoryLocationId",
                schema: "inventory",
                table: "FgsVehicle",
                columns: new[] { "TenantId", "CompanyId", "InventoryLocationId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicle_TenantId_CompanyId_IsActive",
                schema: "inventory",
                table: "FgsVehicle",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicleMaintenance_TenantId_CompanyId_IsActive",
                schema: "inventory",
                table: "FgsVehicleMaintenance",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicleMaintenance_TenantId_CompanyId_IsCompleted",
                schema: "inventory",
                table: "FgsVehicleMaintenance",
                columns: new[] { "TenantId", "CompanyId", "IsCompleted" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicleMaintenance_TenantId_CompanyId_NextServiceDate",
                schema: "inventory",
                table: "FgsVehicleMaintenance",
                columns: new[] { "TenantId", "CompanyId", "NextServiceDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicleMaintenance_TenantId_CompanyId_ServiceDate",
                schema: "inventory",
                table: "FgsVehicleMaintenance",
                columns: new[] { "TenantId", "CompanyId", "ServiceDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicleMaintenance_TenantId_CompanyId_VehicleId",
                schema: "inventory",
                table: "FgsVehicleMaintenance",
                columns: new[] { "TenantId", "CompanyId", "VehicleId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicleMaintenance_TenantId_CompanyId_VehicleMaintenanceTypeId",
                schema: "inventory",
                table: "FgsVehicleMaintenance",
                columns: new[] { "TenantId", "CompanyId", "VehicleMaintenanceTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicleMaintenance_VehicleId",
                schema: "inventory",
                table: "FgsVehicleMaintenance",
                column: "VehicleId");

            migrationBuilder.Sql("""
                DO $copy$
                BEGIN
                    IF to_regclass('setup."FgsVehicle"') IS NOT NULL
                       AND NOT EXISTS (SELECT 1 FROM inventory."FgsVehicle" LIMIT 1) THEN
                        INSERT INTO inventory."FgsVehicle" (
                            "Id", "TenantId", "CompanyId", "InventoryLocationId", "OwnershipType", "OwnershipCompany",
                            "Year", "Make", "Model", "Color", "VIN", "LicensePlate", "LicensePlateState",
                            "PurchaseDate", "PurchasePrice", "PurchasedFrom", "IsPurchasedNew", "Notes",
                            "CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy", "IsActive")
                        OVERRIDING SYSTEM VALUE
                        SELECT
                            v."Id", v."TenantId", v."CompanyId", v."InventoryLocationId", v."OwnershipType", v."OwnershipCompany",
                            v."Year", v."Make", v."Model", v."Color", v."VIN", v."LicensePlate", v."LicensePlateState",
                            v."PurchaseDate", v."PurchasePrice", v."PurchasedFrom", v."IsPurchasedNew", v."Notes",
                            v."CreatedOn", v."CreatedBy", v."UpdatedOn", v."UpdatedBy", v."IsActive"
                        FROM setup."FgsVehicle" v
                        WHERE EXISTS (
                            SELECT 1 FROM inventory."FgsInventoryLocation" l
                            WHERE l."Id" = v."InventoryLocationId")
                          AND EXISTS (
                            SELECT 1 FROM inventory."FgsTenantCompanyCache" c
                            WHERE c."TenantId" = v."TenantId" AND c."CompanyId" = v."CompanyId");

                        PERFORM setval(
                            pg_get_serial_sequence('inventory."FgsVehicle"', 'Id'),
                            COALESCE((SELECT MAX("Id") FROM inventory."FgsVehicle"), 1),
                            true);
                    END IF;

                    IF to_regclass('setup."FgsVehicleMaintenance"') IS NOT NULL
                       AND NOT EXISTS (SELECT 1 FROM inventory."FgsVehicleMaintenance" LIMIT 1) THEN
                        INSERT INTO inventory."FgsVehicleMaintenance" (
                            "Id", "TenantId", "CompanyId", "VehicleId", "VehicleMaintenanceTypeId", "ServiceDate",
                            "MileageAtService", "ServiceProvider", "InvoiceNumber", "Cost", "NextServiceDate",
                            "NextServiceMileage", "IsCompleted", "IsActive", "Description", "Notes",
                            "CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy")
                        OVERRIDING SYSTEM VALUE
                        SELECT
                            m."Id", m."TenantId", m."CompanyId", m."VehicleId", m."VehicleMaintenanceTypeId", m."ServiceDate",
                            m."MileageAtService", m."ServiceProvider", m."InvoiceNumber", m."Cost", m."NextServiceDate",
                            m."NextServiceMileage", m."IsCompleted", m."IsActive", m."Description", m."Notes",
                            m."CreatedOn", m."CreatedBy", m."UpdatedOn", m."UpdatedBy"
                        FROM setup."FgsVehicleMaintenance" m
                        WHERE EXISTS (
                            SELECT 1 FROM inventory."FgsVehicle" v WHERE v."Id" = m."VehicleId")
                          AND EXISTS (
                            SELECT 1 FROM inventory."FgsTenantCompanyCache" c
                            WHERE c."TenantId" = m."TenantId" AND c."CompanyId" = m."CompanyId");

                        PERFORM setval(
                            pg_get_serial_sequence('inventory."FgsVehicleMaintenance"', 'Id'),
                            COALESCE((SELECT MAX("Id") FROM inventory."FgsVehicleMaintenance"), 1),
                            true);
                    END IF;
                END $copy$;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsVehicleMaintenance",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "FgsVehicle",
                schema: "inventory");
        }
    }
}