using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFgsSetupTimeSlotCapacityAndExternalFlags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IncludeInCapacityPlanning",
                schema: "setup",
                table: "FgsSetupTimeSlot",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Indicates whether this time slot is considered during capacity planning and scheduling calculations. When false, the time slot is excluded from capacity planning.");

            migrationBuilder.AddColumn<bool>(
                name: "ShowToExternalSystem",
                schema: "setup",
                table: "FgsSetupTimeSlot",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Indicates whether this time slot is exposed to third-party integrations and external systems. When false, the time slot remains internal to the application.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncludeInCapacityPlanning",
                schema: "setup",
                table: "FgsSetupTimeSlot");

            migrationBuilder.DropColumn(
                name: "ShowToExternalSystem",
                schema: "setup",
                table: "FgsSetupTimeSlot");
        }
    }
}
