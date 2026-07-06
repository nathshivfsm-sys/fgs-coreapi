using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class MovePriceAdjustmentTypeToPricingMatrix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "PriceAdjustmentTypeId",
                schema: "setup",
                table: "FgsSetupPricingMatrix",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1,
                comment: "Pricing adjustment method. Valid values: 1=Markup Percent, 2=Markup Amount, 3=Multiplier.");

            migrationBuilder.Sql("""
                UPDATE setup."FgsSetupPricingMatrix" pm
                SET "PriceAdjustmentTypeId" = sub."PriceAdjustmentTypeId"
                FROM (
                    SELECT DISTINCT ON ("PricingMatrixId") "PricingMatrixId", "PriceAdjustmentTypeId"
                    FROM setup."FgsSetupPricingMatrixMaterialTier"
                    ORDER BY "PricingMatrixId", "FromCost"
                ) sub
                WHERE pm."Id" = sub."PricingMatrixId";
                """);

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupPricingMatrixMaterialTier_TenantId_CompanyId_PriceAdjustmentTypeId",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FgsSetupPricingMatrixMaterialTier_PriceAdjustmentTypeId",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier");

            migrationBuilder.DropColumn(
                name: "PriceAdjustmentTypeId",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier");

            migrationBuilder.AddCheckConstraint(
                name: "CK_FgsSetupPricingMatrix_PriceAdjustmentTypeId",
                schema: "setup",
                table: "FgsSetupPricingMatrix",
                sql: "\"PriceAdjustmentTypeId\" BETWEEN 1 AND 3");

            migrationBuilder.Sql("""
                ALTER TABLE setup."FgsSetupPricingMatrix"
                ALTER COLUMN "PriceAdjustmentTypeId" DROP DEFAULT;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_FgsSetupPricingMatrix_PriceAdjustmentTypeId",
                schema: "setup",
                table: "FgsSetupPricingMatrix");

            migrationBuilder.AddColumn<short>(
                name: "PriceAdjustmentTypeId",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1,
                comment: "Pricing adjustment method. Valid values: 1=Markup Percent, 2=Markup Amount, 3=Multiplier.");

            migrationBuilder.Sql("""
                UPDATE setup."FgsSetupPricingMatrixMaterialTier" mt
                SET "PriceAdjustmentTypeId" = pm."PriceAdjustmentTypeId"
                FROM setup."FgsSetupPricingMatrix" pm
                WHERE mt."PricingMatrixId" = pm."Id";
                """);

            migrationBuilder.DropColumn(
                name: "PriceAdjustmentTypeId",
                schema: "setup",
                table: "FgsSetupPricingMatrix");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPricingMatrixMaterialTier_TenantId_CompanyId_PriceAdjustmentTypeId",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier",
                columns: new[] { "TenantId", "CompanyId", "PriceAdjustmentTypeId" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_FgsSetupPricingMatrixMaterialTier_PriceAdjustmentTypeId",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier",
                sql: "\"PriceAdjustmentTypeId\" BETWEEN 1 AND 3");

            migrationBuilder.Sql("""
                ALTER TABLE setup."FgsSetupPricingMatrixMaterialTier"
                ALTER COLUMN "PriceAdjustmentTypeId" DROP DEFAULT;
                """);
        }
    }
}
