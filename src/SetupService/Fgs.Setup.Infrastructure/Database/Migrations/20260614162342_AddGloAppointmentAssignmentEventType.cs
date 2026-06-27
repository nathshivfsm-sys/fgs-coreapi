using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddGloAppointmentAssignmentEventType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GloAppointmentAssignmentEventType",
                schema: "glo",
                columns: table => new
                {
                    EventTypeId = table.Column<short>(type: "smallint", nullable: false, comment: "Primary key and event type identifier referenced by dispatch.FgsAppointmentAssignmentEvent."),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Unique event type code."),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Display name of the event type."),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Optional description of the event type."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Display order for UI and reporting."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the event type is active."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the record."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloAppointmentAssignmentEventType", x => x.EventTypeId);
                },
                comment: "Global catalog of technician appointment assignment event types used for dispatch tracking and payroll.");

            migrationBuilder.CreateIndex(
                name: "IX_GloAppointmentAssignmentEventType_DisplayOrder",
                schema: "glo",
                table: "GloAppointmentAssignmentEventType",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "UX_GloAppointmentAssignmentEventType_Code",
                schema: "glo",
                table: "GloAppointmentAssignmentEventType",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GloAppointmentAssignmentEventType",
                schema: "glo");
        }
    }
}
