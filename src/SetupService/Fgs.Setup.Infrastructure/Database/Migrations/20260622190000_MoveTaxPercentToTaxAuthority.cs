using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class MoveTaxPercentToTaxAuthority : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TaxPercent",
                schema: "setup",
                table: "FgsSetupTaxAuthority",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddCheckConstraint(
                name: "CK_FgsSetupTaxAuthority_TaxPercent",
                schema: "setup",
                table: "FgsSetupTaxAuthority",
                sql: "\"TaxPercent\" >= 0 AND \"TaxPercent\" <= 100");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FgsSetupTaxDetail_TaxPercent",
                schema: "setup",
                table: "FgsSetupTaxDetail");

            migrationBuilder.DropColumn(
                name: "TaxPercent",
                schema: "setup",
                table: "FgsSetupTaxDetail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TaxPercent",
                schema: "setup",
                table: "FgsSetupTaxDetail",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.DropCheckConstraint(
                name: "CK_FgsSetupTaxAuthority_TaxPercent",
                schema: "setup",
                table: "FgsSetupTaxAuthority");

            migrationBuilder.DropColumn(
                name: "TaxPercent",
                schema: "setup",
                table: "FgsSetupTaxAuthority");

            migrationBuilder.AddCheckConstraint(
                name: "CK_FgsSetupTaxDetail_TaxPercent",
                schema: "setup",
                table: "FgsSetupTaxDetail",
                sql: "\"TaxPercent\" >= 0 AND \"TaxPercent\" <= 100");
        }
    }
}
