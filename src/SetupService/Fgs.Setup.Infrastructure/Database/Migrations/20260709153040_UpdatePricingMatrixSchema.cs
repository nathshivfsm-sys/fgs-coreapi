using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePricingMatrixSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                schema: "setup",
                table: "FgsSetupPricingMatrix",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<long>(
                name: "TechSkillLevelId",
                schema: "setup",
                table: "FgsSetupPricingMatrixLaborTier",
                type: "bigint",
                nullable: true);

            migrationBuilder.DropCheckConstraint(
                name: "CK_FgsSetupPricingMatrixOther_MarkupPercent",
                schema: "setup",
                table: "FgsSetupPricingMatrixOther");

            migrationBuilder.RenameColumn(
                name: "MarkupPercent",
                schema: "setup",
                table: "FgsSetupPricingMatrixOther",
                newName: "AdjustmentValue");

            migrationBuilder.Sql("""
                COMMENT ON COLUMN setup."FgsSetupPricingMatrixOther"."AdjustmentValue"
                IS 'Value used by the selected pricing adjustment type. Examples: 25 = 25% markup, 150 = fixed dollar markup, 1.75 = multiplier.';
                """);

            migrationBuilder.AddCheckConstraint(
                name: "CK_FgsSetupPricingMatrixOther_AdjustmentValue",
                schema: "setup",
                table: "FgsSetupPricingMatrixOther",
                sql: "\"AdjustmentValue\" IS NULL OR \"AdjustmentValue\" >= 0");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPricingMatrixLaborTier_TechSkillLevelId",
                schema: "setup",
                table: "FgsSetupPricingMatrixLaborTier",
                column: "TechSkillLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupPricingMatrixLaborTier_TechSkillLevel",
                schema: "setup",
                table: "FgsSetupPricingMatrixLaborTier",
                column: "TechSkillLevelId",
                principalSchema: "setup",
                principalTable: "FgsSetupTechSkillLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupPricingMatrixLaborTier_TechSkillLevel",
                schema: "setup",
                table: "FgsSetupPricingMatrixLaborTier");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FgsSetupPricingMatrixOther_AdjustmentValue",
                schema: "setup",
                table: "FgsSetupPricingMatrixOther");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupPricingMatrixLaborTier_TechSkillLevelId",
                schema: "setup",
                table: "FgsSetupPricingMatrixLaborTier");

            migrationBuilder.RenameColumn(
                name: "AdjustmentValue",
                schema: "setup",
                table: "FgsSetupPricingMatrixOther",
                newName: "MarkupPercent");

            migrationBuilder.Sql("""
                COMMENT ON COLUMN setup."FgsSetupPricingMatrixOther"."MarkupPercent"
                IS 'Markup percentage applied to the base cost.';
                """);

            migrationBuilder.AddCheckConstraint(
                name: "CK_FgsSetupPricingMatrixOther_MarkupPercent",
                schema: "setup",
                table: "FgsSetupPricingMatrixOther",
                sql: "\"MarkupPercent\" IS NULL OR \"MarkupPercent\" >= 0");

            migrationBuilder.DropColumn(
                name: "TechSkillLevelId",
                schema: "setup",
                table: "FgsSetupPricingMatrixLaborTier");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                schema: "setup",
                table: "FgsSetupPricingMatrix");
        }
    }
}
