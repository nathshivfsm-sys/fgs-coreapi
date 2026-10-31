using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceBookVersionIdToFgsPriceBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PriceBookVersionId",
                schema: "setup",
                table: "FgsPriceBook",
                type: "bigint",
                nullable: true,
                comment: "Identifier of the Price Book version associated with this price book record. The referenced version contains the version number and effective date range.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsPriceBook_PriceBookVersionId",
                schema: "setup",
                table: "FgsPriceBook",
                column: "PriceBookVersionId");

            migrationBuilder.AddForeignKey(
                name: "FK_FgsPriceBook_FgsSetupPriceBookVersion_PriceBookVersionId",
                schema: "setup",
                table: "FgsPriceBook",
                column: "PriceBookVersionId",
                principalSchema: "setup",
                principalTable: "FgsSetupPriceBookVersion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FgsPriceBook_FgsSetupPriceBookVersion_PriceBookVersionId",
                schema: "setup",
                table: "FgsPriceBook");

            migrationBuilder.DropIndex(
                name: "IX_FgsPriceBook_PriceBookVersionId",
                schema: "setup",
                table: "FgsPriceBook");

            migrationBuilder.DropColumn(
                name: "PriceBookVersionId",
                schema: "setup",
                table: "FgsPriceBook");
        }
    }
}
