using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFgsBillingCategoryAccounting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FgsBillingCategoryAccounting",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key identity of the billing category accounting record.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier owning this billing category accounting configuration."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier within the tenant owning this billing category accounting configuration."),
                    BillingCategoryId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the billing category to which the accounting configuration applies."),
                    RevenueAccountId = table.Column<long>(type: "bigint", nullable: true, comment: "Optional GL account used to record revenue generated from the billing category."),
                    CogsAccountId = table.Column<long>(type: "bigint", nullable: true, comment: "Optional GL account used to record cost of goods sold associated with the billing category."),
                    InventoryOffsetAccountId = table.Column<long>(type: "bigint", nullable: true, comment: "Optional GL account used as the inventory offset account for inventory billing categories. This account applies only when the associated billing category is an Inventory billing category."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the billing category accounting configuration was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User identifier that created the billing category accounting configuration."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the billing category accounting configuration was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User identifier that last updated the billing category accounting configuration.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsBillingCategoryAccounting", x => x.Id);
                    table.UniqueConstraint("UQ_FgsBillingCategoryAccounting_TenantId_CompanyId_BillingCategoryId", x => new { x.TenantId, x.CompanyId, x.BillingCategoryId });
                    table.ForeignKey(
                        name: "FK_FgsBillingCategoryAccounting_FgsBillingCategory",
                        column: x => x.BillingCategoryId,
                        principalSchema: "setup",
                        principalTable: "FgsBillingCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsBillingCategoryAccounting_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores optional accounting configuration for tenant/company specific billing categories, including Revenue, COGS, and Inventory Offset GL account mappings. Accounting configuration is maintained separately from the billing category so it can remain optional.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsBillingCategoryAccounting_BillingCategoryId",
                schema: "setup",
                table: "FgsBillingCategoryAccounting",
                column: "BillingCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsBillingCategoryAccounting_TenantId_CompanyId",
                schema: "setup",
                table: "FgsBillingCategoryAccounting",
                columns: new[] { "TenantId", "CompanyId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsBillingCategoryAccounting",
                schema: "setup");
        }
    }
}
