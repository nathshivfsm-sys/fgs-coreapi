using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFgsVehicleMaintenanceIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "setup",
                table: "FgsVehicleMaintenance",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                comment: "Indicates whether the maintenance record is active and available for use.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicleMaintenance_TenantId_CompanyId_IsActive",
                schema: "setup",
                table: "FgsVehicleMaintenance",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FgsVehicleMaintenance_TenantId_CompanyId_IsActive",
                schema: "setup",
                table: "FgsVehicleMaintenance");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "setup",
                table: "FgsVehicleMaintenance");
        }
    }
}
