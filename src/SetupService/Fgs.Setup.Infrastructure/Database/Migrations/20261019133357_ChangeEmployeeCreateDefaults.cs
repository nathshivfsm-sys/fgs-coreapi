using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEmployeeCreateDefaults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "CanBeScheduled",
                schema: "setup",
                table: "FgsEmployeeTechnicianProfile",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Indicates whether the technician can receive appointments and appear on the dispatch board.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true,
                oldComment: "Indicates whether the technician can receive appointments and appear on the dispatch board.");

            migrationBuilder.AlterColumn<string>(
                name: "LegalMiddleName",
                schema: "setup",
                table: "FgsEmployee",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Paycheck middle name printed on the paycheck and used on the timecard Excel export.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Employee legal middle name.");

            migrationBuilder.AlterColumn<string>(
                name: "LegalLastName",
                schema: "setup",
                table: "FgsEmployee",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Paycheck last name printed on the paycheck and used on the timecard Excel export.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldComment: "Employee legal last name.");

            migrationBuilder.AlterColumn<string>(
                name: "LegalFirstName",
                schema: "setup",
                table: "FgsEmployee",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Paycheck first name printed on the paycheck and used on the timecard Excel export.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldComment: "Employee legal first name.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "CanBeScheduled",
                schema: "setup",
                table: "FgsEmployeeTechnicianProfile",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                comment: "Indicates whether the technician can receive appointments and appear on the dispatch board.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false,
                oldComment: "Indicates whether the technician can receive appointments and appear on the dispatch board.");

            migrationBuilder.AlterColumn<string>(
                name: "LegalMiddleName",
                schema: "setup",
                table: "FgsEmployee",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Employee legal middle name.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Paycheck middle name printed on the paycheck and used on the timecard Excel export.");

            migrationBuilder.AlterColumn<string>(
                name: "LegalLastName",
                schema: "setup",
                table: "FgsEmployee",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                comment: "Employee legal last name.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Paycheck last name printed on the paycheck and used on the timecard Excel export.");

            migrationBuilder.AlterColumn<string>(
                name: "LegalFirstName",
                schema: "setup",
                table: "FgsEmployee",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                comment: "Employee legal first name.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Paycheck first name printed on the paycheck and used on the timecard Excel export.");
        }
    }
}
