using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.User.Infrastructure.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGloBillingCategoryAndFgsSetupGLBreakTrade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Trades",
                schema: "dbo",
                table: "FgsSetupGLBreak");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "GloBillingCategory",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "DisplayOrder",
                schema: "dbo",
                table: "GloBillingCategory",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1);

            migrationBuilder.CreateTable(
                name: "FgsSetupGLBreakTrade",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    GLBreakId = table.Column<long>(type: "bigint", nullable: false),
                    TradeCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupGLBreakTrade", x => x.Id);
                    table.UniqueConstraint("UQ_FgsSetupGLBreakTrade", x => new { x.TenantId, x.CompanyId, x.GLBreakId, x.TradeCode });
                    table.ForeignKey(
                        name: "FK_FgsSetupGLBreakTrade_FgsSetupGLBreak_GLBreakId",
                        column: x => x.GLBreakId,
                        principalSchema: "dbo",
                        principalTable: "FgsSetupGLBreak",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsSetupGLBreakTrade_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupGLBreakTrade_GLBreakId",
                schema: "dbo",
                table: "FgsSetupGLBreakTrade",
                column: "GLBreakId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupGLBreakTrade_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupGLBreakTrade",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupGLBreakTrade_TradeCode",
                schema: "dbo",
                table: "FgsSetupGLBreakTrade",
                columns: new[] { "TenantId", "CompanyId", "TradeCode" });

            migrationBuilder.Sql(
                """
                COMMENT ON TABLE dbo."GloBillingCategory" IS 'Global billing line category lookup used during tenant provisioning (equipment, labor, tax, etc.).';
                COMMENT ON COLUMN dbo."GloBillingCategory"."BillingCategoryType" IS 'Short billing category code (primary key), e.g. IN, LB, TX.';
                COMMENT ON COLUMN dbo."GloBillingCategory"."BillingCategoryName" IS 'Display name of the billing category.';
                COMMENT ON COLUMN dbo."GloBillingCategory"."Description" IS 'Optional description of how the billing category is used.';
                COMMENT ON COLUMN dbo."GloBillingCategory"."DisplayOrder" IS 'Controls sorting/display order of billing categories in dropdowns and setup screens.';

                COMMENT ON TABLE dbo."FgsSetupGLBreak" IS 'Stores GL break configuration for financial reporting segmentation by trade, division, branch, or organizational unit.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreak"."Id" IS 'Surrogate primary key.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreak"."TenantId" IS 'Owning tenant identifier.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreak"."CompanyId" IS 'Tenant-scoped company number (maps to FgsTenantCompany.CompanyNumber).';
                COMMENT ON COLUMN dbo."FgsSetupGLBreak"."Code" IS 'Unique GL break code within tenant, company, and break level scope.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreak"."Name" IS 'Display name of the GL break.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreak"."BreakLabel" IS 'Optional label displayed in UI and financial documents.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreak"."BreakLevel" IS 'Break hierarchy level. Allowed values: 1 or 2.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreak"."LogoFileId" IS 'Optional reference to uploaded logo file in FgsFile.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreak"."AddressId" IS 'Optional reference to branch or break address in FgsLocation.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreak"."IsActive" IS 'Indicates whether the GL break is active.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreak"."CreatedOn" IS 'UTC timestamp when the record was created.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreak"."CreatedBy" IS 'User or process that created the record.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreak"."UpdatedOn" IS 'UTC timestamp when the record was last updated.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreak"."UpdatedBy" IS 'User or process that last updated the record.';

                COMMENT ON TABLE dbo."FgsSetupGLBreakTrade" IS 'Stores trade-to-GL-break mappings used for financial segmentation and reporting.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreakTrade"."Id" IS 'Surrogate primary key.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreakTrade"."TenantId" IS 'Owning tenant identifier.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreakTrade"."CompanyId" IS 'Tenant-scoped company number.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreakTrade"."GLBreakId" IS 'Reference to the associated GL break configuration.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreakTrade"."TradeCode" IS 'Technician or operational trade code associated with the GL break such as HVAC, Plumbing, Electrical, or Drain.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreakTrade"."CreatedOn" IS 'UTC timestamp when the record was created.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreakTrade"."CreatedBy" IS 'User or process that created the record.';
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsSetupGLBreakTrade",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "dbo",
                table: "GloBillingCategory");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                schema: "dbo",
                table: "GloBillingCategory");

            migrationBuilder.AddColumn<string[]>(
                name: "Trades",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "text[]",
                nullable: true);
        }
    }
}
