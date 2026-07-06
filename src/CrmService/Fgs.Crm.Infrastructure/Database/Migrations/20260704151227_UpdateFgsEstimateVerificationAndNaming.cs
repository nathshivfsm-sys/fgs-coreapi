using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Crm.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFgsEstimateVerificationAndNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RecommendedByEmployeeId",
                schema: "crm",
                table: "FgsEstimate",
                type: "bigint",
                nullable: true,
                comment: "Stores the employee ID of the employee who recommended the estimate or proposed work to the customer.");

            migrationBuilder.AddColumn<bool>(
                name: "VerificationRequired",
                schema: "crm",
                table: "FgsEstimate",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Indicates whether the estimate requires internal verification before it can proceed through the estimate workflow.");

            migrationBuilder.AddColumn<long>(
                name: "VerifiedByEmployeeId",
                schema: "crm",
                table: "FgsEstimate",
                type: "bigint",
                nullable: true,
                comment: "Stores the employee ID of the employee who reviewed and verified the estimate.");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "VerifiedOn",
                schema: "crm",
                table: "FgsEstimate",
                type: "timestamptz",
                nullable: true,
                comment: "Stores the date and time when the estimate was verified.");

            migrationBuilder.RenameColumn(
                name: "QuoteName",
                schema: "crm",
                table: "FgsEstimate",
                newName: "EstimateName");

            migrationBuilder.AddColumn<string>(
                name: "InternalNotes",
                schema: "crm",
                table: "FgsEstimate",
                type: "text",
                nullable: true,
                comment: "Stores internal notes related to the estimate. This information is intended for internal company use and should not be displayed to the customer.");

            migrationBuilder.AddColumn<string>(
                name: "InstallationDescription",
                schema: "crm",
                table: "FgsEstimate",
                type: "text",
                nullable: true,
                comment: "Stores installation instructions, installation scope details, or other information intended for the installation or field service team.");

            migrationBuilder.RenameColumn(
                name: "QuoteDescription",
                schema: "crm",
                table: "FgsEstimate",
                newName: "EstimateDescription");

            migrationBuilder.Sql("""
                COMMENT ON COLUMN crm."FgsEstimate"."EstimateName" IS 'User-facing estimate name.';
                COMMENT ON COLUMN crm."FgsEstimate"."EstimateDescription" IS 'Detailed estimate description, scope summary, or explanatory information presented to the customer.';
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EstimateDescription",
                schema: "crm",
                table: "FgsEstimate",
                newName: "QuoteDescription");

            migrationBuilder.DropColumn(
                name: "InstallationDescription",
                schema: "crm",
                table: "FgsEstimate");

            migrationBuilder.DropColumn(
                name: "InternalNotes",
                schema: "crm",
                table: "FgsEstimate");

            migrationBuilder.RenameColumn(
                name: "EstimateName",
                schema: "crm",
                table: "FgsEstimate",
                newName: "QuoteName");

            migrationBuilder.DropColumn(
                name: "VerifiedOn",
                schema: "crm",
                table: "FgsEstimate");

            migrationBuilder.DropColumn(
                name: "VerifiedByEmployeeId",
                schema: "crm",
                table: "FgsEstimate");

            migrationBuilder.DropColumn(
                name: "VerificationRequired",
                schema: "crm",
                table: "FgsEstimate");

            migrationBuilder.DropColumn(
                name: "RecommendedByEmployeeId",
                schema: "crm",
                table: "FgsEstimate");

            migrationBuilder.Sql("""
                COMMENT ON COLUMN crm."FgsEstimate"."QuoteName" IS 'User-facing quote name.';
                COMMENT ON COLUMN crm."FgsEstimate"."QuoteDescription" IS 'Detailed quote description presented to the customer.';
                """);
        }
    }
}
