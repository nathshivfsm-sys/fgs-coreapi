using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddPricingMatrixIsUsedInPriceBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUsedInPriceBook",
                schema: "setup",
                table: "FgsSetupPricingMatrix",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Indicates whether this pricing matrix is used by the Price Book for pricing calculations.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUsedInPriceBook",
                schema: "setup",
                table: "FgsSetupPricingMatrix");
        }
    }
}
