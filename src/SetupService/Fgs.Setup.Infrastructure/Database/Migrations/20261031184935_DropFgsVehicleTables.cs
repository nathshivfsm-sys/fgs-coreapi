using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class DropFgsVehicleTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsVehicleMaintenance",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsVehicle",
                schema: "setup");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FgsVehicle",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Color = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "Vehicle exterior color."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the record."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    InventoryLocationId = table.Column<long>(type: "bigint", nullable: false, comment: "Associated truck inventory location. References inventory.FgsInventoryLocation; scalar only — no cross-schema FK."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the vehicle is active and available for service operations."),
                    IsPurchasedNew = table.Column<bool>(type: "boolean", nullable: true, comment: "Indicates whether the vehicle was purchased new or used."),
                    LicensePlate = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "Vehicle registration plate number."),
                    LicensePlateState = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "State or province issuing the vehicle registration."),
                    Make = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Vehicle manufacturer such as Ford, Chevrolet, GMC, Ram, Toyota, etc."),
                    Model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Vehicle model such as F-150, Transit, Silverado, Express, etc."),
                    Notes = table.Column<string>(type: "text", nullable: true, comment: "Internal notes and remarks regarding the vehicle."),
                    OwnershipCompany = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Name of the leasing company, rental provider, or other organization that owns the vehicle when it is not company-owned."),
                    OwnershipType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Owned", comment: "Indicates whether the vehicle is owned, leased, or rented."),
                    PurchaseDate = table.Column<DateOnly>(type: "date", nullable: true, comment: "Date the vehicle was purchased or acquired."),
                    PurchasePrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true, comment: "Amount paid to acquire the vehicle."),
                    PurchasedFrom = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Name of the dealership, seller, auction, fleet provider, or other source from which the vehicle was acquired."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last updated the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    VIN = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Vehicle Identification Number assigned by the manufacturer."),
                    Year = table.Column<short>(type: "smallint", nullable: true, comment: "Vehicle model year.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsVehicle", x => x.Id);
                    table.UniqueConstraint("UQ_FgsVehicle_InventoryLocationId", x => x.InventoryLocationId);
                    table.CheckConstraint("CK_FgsVehicle_OwnershipType", "\"OwnershipType\" IN ('Owned', 'Leased', 'Rented')");
                    table.ForeignKey(
                        name: "FK_FgsVehicle_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores company-owned or leased vehicles used for field service operations. Each vehicle is associated with an inventory location that serves as a truck storage location.");

            migrationBuilder.CreateTable(
                name: "FgsVehicleMaintenance",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Cost = table.Column<decimal>(type: "numeric(18,2)", nullable: true, comment: "Total cost incurred for the maintenance activity."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the record."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Short summary of the maintenance activity performed or scheduled."),
                    InvoiceNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Vendor invoice, receipt, repair order, or work order number associated with the maintenance activity."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the maintenance record is active and available for use."),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the maintenance activity has been completed. False indicates a scheduled or pending maintenance item."),
                    MileageAtService = table.Column<int>(type: "integer", nullable: true, comment: "Vehicle odometer reading at the time the maintenance was performed."),
                    NextServiceDate = table.Column<DateOnly>(type: "date", nullable: true, comment: "Recommended next service date based on maintenance provider recommendations."),
                    NextServiceMileage = table.Column<int>(type: "integer", nullable: true, comment: "Recommended next service mileage based on maintenance provider recommendations."),
                    Notes = table.Column<string>(type: "text", nullable: true, comment: "Detailed notes, observations, recommendations, or repair information related to the maintenance activity."),
                    ServiceDate = table.Column<DateOnly>(type: "date", nullable: false, comment: "Date the maintenance was performed or is scheduled to be performed."),
                    ServiceProvider = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Name of the repair shop, dealership, service provider, or maintenance vendor."),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last updated the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    VehicleId = table.Column<long>(type: "bigint", nullable: false, comment: "Vehicle that received or is scheduled to receive maintenance service."),
                    VehicleMaintenanceTypeId = table.Column<int>(type: "integer", nullable: false, comment: "Type of maintenance activity being performed or scheduled.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsVehicleMaintenance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsVehicleMaintenance_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsVehicleMaintenance_FgsVehicle_VehicleId",
                        column: x => x.VehicleId,
                        principalSchema: "setup",
                        principalTable: "FgsVehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores completed and scheduled maintenance activities, inspections, repairs, and service history for company vehicles.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicle_TenantId_CompanyId_InventoryLocationId",
                schema: "setup",
                table: "FgsVehicle",
                columns: new[] { "TenantId", "CompanyId", "InventoryLocationId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicle_TenantId_CompanyId_IsActive",
                schema: "setup",
                table: "FgsVehicle",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicleMaintenance_TenantId_CompanyId_IsActive",
                schema: "setup",
                table: "FgsVehicleMaintenance",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

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
        }
    }
}
