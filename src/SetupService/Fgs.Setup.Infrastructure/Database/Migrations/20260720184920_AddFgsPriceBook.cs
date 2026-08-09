using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFgsPriceBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Guard: on some environments this unique rule already exists as an index, not a table constraint.
            migrationBuilder.Sql(
                """
                DO $EF$
                BEGIN
                    IF EXISTS (
                        SELECT 1
                        FROM pg_constraint c
                        JOIN pg_class t ON t.oid = c.conrelid
                        JOIN pg_namespace n ON n.oid = t.relnamespace
                        WHERE c.conname = 'UQ_FgsSetupPricingMatrixLabor'
                          AND n.nspname = 'setup'
                          AND t.relname = 'FgsSetupPricingMatrixLabor') THEN
                        ALTER TABLE setup."FgsSetupPricingMatrixLabor" DROP CONSTRAINT "UQ_FgsSetupPricingMatrixLabor";
                    END IF;
                END $EF$;
                """);

            migrationBuilder.AlterColumn<string>(
                name: "CategoryCode",
                schema: "setup",
                table: "FgsSetupPricingMatrixOther",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                comment: "Billing category type code (FgsBillingCategory.BillingCategoryType) such as NI, OT, or SF.",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldComment: "Unique category code within the pricing matrix.");

            migrationBuilder.AlterColumn<long>(
                name: "TechSkillLevelId",
                schema: "setup",
                table: "FgsSetupPricingMatrixLabor",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateTable(
                name: "FgsPriceBook",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique identifier of the price book record.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the tenant that owns the record."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the company that owns the record."),
                    PriceBookCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Unique business code of the price book item."),
                    PriceBookName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Display name of the service offered in the price book."),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Detailed description of the service."),
                    JobTypeId = table.Column<long>(type: "bigint", nullable: false, comment: "Default work order type associated with this service."),
                    PricingModel = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, comment: "Determines whether pricing is Flat Rate or Dynamic."),
                    EstimatedDurationMinutes = table.Column<int>(type: "integer", nullable: false, defaultValue: 60, comment: "Estimated time in minutes required to complete the service."),
                    BasePrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true, comment: "Base selling price when the pricing model is Flat Rate. Null for Dynamic pricing."),
                    IsTaxable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the service is taxable by default."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time when the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time when the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last updated the record."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the price book item is active and available for use.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsPriceBook", x => x.Id);
                    table.CheckConstraint("CK_FgsPriceBook_PricingModel", "\"PricingModel\" IN ('Flat Rate', 'Dynamic')");
                    table.ForeignKey(
                        name: "FK_FgsPriceBook_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsPriceBook_JobTypeId",
                        column: x => x.JobTypeId,
                        principalSchema: "setup",
                        principalTable: "FgsJobType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Defines the master catalog of services offered by a company. Each price book header represents a reusable service template used by estimates, work orders, invoices, scheduling, and pricing.");

            migrationBuilder.CreateTable(
                name: "FgsPriceBookItem",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key identifier of the price book item.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier owning the record."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier owning the record."),
                    PriceBookId = table.Column<long>(type: "bigint", nullable: false, comment: "Reference to the parent price book."),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: true, comment: "Identifier of the inventory or non-inventory item. No database foreign key is enforced because the inventory module resides in a separate schema."),
                    ItemCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "Business code of the selected item. Stored as a snapshot for reporting and historical consistency."),
                    ItemDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false, comment: "Description of the item as it should appear within the price book."),
                    Quantity = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false, defaultValue: 1m, comment: "Default quantity required to perform the service."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls the display order of items within the price book."),
                    Notes = table.Column<string>(type: "text", nullable: true, comment: "Optional notes or installation instructions."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsPriceBookItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsPriceBookItem_FgsPriceBook",
                        column: x => x.PriceBookId,
                        principalSchema: "setup",
                        principalTable: "FgsPriceBook",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsPriceBookItem_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Defines the inventory, non-inventory, and free-form items that make up a price book service.");

            migrationBuilder.Sql(
                """
                CREATE UNIQUE INDEX IF NOT EXISTS "UQ_FgsSetupPricingMatrixLabor"
                ON setup."FgsSetupPricingMatrixLabor"
                ("TenantId", "CompanyId", "PricingMatrixId", "LaborRateTypeId", "TechSkillLevelId");
                """);

            migrationBuilder.CreateIndex(
                name: "IX_FgsPriceBook_JobTypeId",
                schema: "setup",
                table: "FgsPriceBook",
                column: "JobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsPriceBook_Tenant_Company",
                schema: "setup",
                table: "FgsPriceBook",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPriceBook_Tenant_Company_IsActive",
                schema: "setup",
                table: "FgsPriceBook",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsPriceBook_Tenant_Company_PriceBookCode",
                schema: "setup",
                table: "FgsPriceBook",
                columns: new[] { "TenantId", "CompanyId", "PriceBookCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsPriceBookItem_PriceBookId",
                schema: "setup",
                table: "FgsPriceBookItem",
                column: "PriceBookId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsPriceBookItem_TenantId_CompanyId",
                schema: "setup",
                table: "FgsPriceBookItem",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPriceBookItem_TenantId_CompanyId_PriceBookId",
                schema: "setup",
                table: "FgsPriceBookItem",
                columns: new[] { "TenantId", "CompanyId", "PriceBookId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsPriceBookItem",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsPriceBook",
                schema: "setup");

            migrationBuilder.DropIndex(
                name: "UQ_FgsSetupPricingMatrixLabor",
                schema: "setup",
                table: "FgsSetupPricingMatrixLabor");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryCode",
                schema: "setup",
                table: "FgsSetupPricingMatrixOther",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                comment: "Unique category code within the pricing matrix.",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldComment: "Billing category type code (FgsBillingCategory.BillingCategoryType) such as NI, OT, or SF.");

            migrationBuilder.AlterColumn<long>(
                name: "TechSkillLevelId",
                schema: "setup",
                table: "FgsSetupPricingMatrixLabor",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "UQ_FgsSetupPricingMatrixLabor",
                schema: "setup",
                table: "FgsSetupPricingMatrixLabor",
                columns: new[] { "TenantId", "CompanyId", "PricingMatrixId", "LaborRateTypeId", "TechSkillLevelId" });
        }
    }
}
