using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Asset.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddAssetUnitNumberAttributeValueAndRenameIsOurInstallation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                ALTER TABLE asset."FgsAsset"
                ADD COLUMN IF NOT EXISTS "UnitNumber" character varying(50);

                COMMENT ON COLUMN asset."FgsAsset"."UnitNumber"
                IS 'Apartment, suite, unit, condo, office, bay, or other sub-location identifier within the service location where the asset is installed. Examples: Apt 1205, Suite 400, Unit 8B, Bay 12.';

                ALTER TABLE asset."FgsAsset"
                ADD COLUMN IF NOT EXISTS "IsOurInstallation" boolean NOT NULL DEFAULT false;

                COMMENT ON COLUMN asset."FgsAsset"."IsOurInstallation"
                IS 'Indicates whether the asset was originally installed by the company. TRUE if installed by this company; FALSE if installed by another contractor or the customer.';

                ALTER TABLE asset."FgsAsset"
                ALTER COLUMN "ServiceLocationId" DROP NOT NULL;

                COMMENT ON COLUMN asset."FgsAsset"."ServiceLocationId"
                IS 'Optional service location where the asset is installed.';

                ALTER TABLE asset."FgsAssetAttribute"
                DROP COLUMN IF EXISTS "AssetId";
                """);

            migrationBuilder.CreateTable(
                name: "FgsAssetAttributeValue",
                schema: "asset",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique asset attribute value identifier.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssetId = table.Column<long>(type: "bigint", nullable: false, comment: "Asset that owns the attribute value."),
                    AssetAttributeId = table.Column<long>(type: "bigint", nullable: false, comment: "Reference to the asset attribute definition."),
                    OptionId = table.Column<long>(type: "bigint", nullable: true, comment: "Selected option identifier when the attribute input type is DROPDOWN."),
                    ValueText = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Text value for TEXT or TEXTAREA attributes."),
                    ValueInteger = table.Column<int>(type: "integer", nullable: true, comment: "Integer value for INTEGER attributes."),
                    ValueDecimal = table.Column<decimal>(type: "numeric(18,4)", nullable: true, comment: "Decimal value for DECIMAL attributes."),
                    ValueDate = table.Column<DateOnly>(type: "date", nullable: true, comment: "Date value for DATE attributes."),
                    ValueBoolean = table.Column<bool>(type: "boolean", nullable: true, comment: "Boolean value for BOOLEAN attributes."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "User who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsAssetAttributeValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsAssetAttributeValue_Asset",
                        column: x => x.AssetId,
                        principalSchema: "asset",
                        principalTable: "FgsAsset",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsAssetAttributeValue_Attribute",
                        column: x => x.AssetAttributeId,
                        principalSchema: "asset",
                        principalTable: "FgsAssetAttribute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsAssetAttributeValue_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "asset",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores the values of custom attributes for individual assets. Each record contains the value of one attribute assigned to one asset.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetAttributeValue_AssetAttributeId",
                schema: "asset",
                table: "FgsAssetAttributeValue",
                column: "AssetAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetAttributeValue_AssetId",
                schema: "asset",
                table: "FgsAssetAttributeValue",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetAttributeValue_TenantCompany",
                schema: "asset",
                table: "FgsAssetAttributeValue",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UQ_FgsAssetAttributeValue_AssetAttribute",
                schema: "asset",
                table: "FgsAssetAttributeValue",
                columns: new[] { "TenantId", "CompanyId", "AssetId", "AssetAttributeId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsAssetAttributeValue",
                schema: "asset");

            migrationBuilder.Sql(
                """
                ALTER TABLE asset."FgsAsset"
                DROP COLUMN IF EXISTS "UnitNumber";

                ALTER TABLE asset."FgsAsset"
                DROP COLUMN IF EXISTS "IsOurInstallation";

                UPDATE asset."FgsAsset" SET "ServiceLocationId" = 0 WHERE "ServiceLocationId" IS NULL;
                ALTER TABLE asset."FgsAsset"
                ALTER COLUMN "ServiceLocationId" SET NOT NULL;

                COMMENT ON COLUMN asset."FgsAsset"."ServiceLocationId"
                IS 'Service location where the asset is installed.';
                """);
        }
    }
}
