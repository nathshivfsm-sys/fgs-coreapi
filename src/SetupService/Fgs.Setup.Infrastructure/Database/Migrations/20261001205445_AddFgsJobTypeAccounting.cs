using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFgsJobTypeAccounting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FgsJobTypeAccounting",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key identity of the job type accounting record.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier owning this job type accounting configuration."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier within the tenant owning this job type accounting configuration."),
                    JobTypeId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the job type to which the accounting configuration applies."),
                    ArAccountId = table.Column<long>(type: "bigint", nullable: true, comment: "Optional GL account used to record accounts receivable for transactions associated with the job type. This account is independent of billing category."),
                    DiscountAccountId = table.Column<long>(type: "bigint", nullable: true, comment: "Optional GL account used to record discounts associated with the job type. This account is independent of billing category."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the job type accounting configuration was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User identifier that created the job type accounting configuration."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the job type accounting configuration was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User identifier that last updated the job type accounting configuration.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsJobTypeAccounting", x => x.Id);
                    table.UniqueConstraint("UQ_FgsJobTypeAccounting_TenantId_CompanyId_JobTypeId", x => new { x.TenantId, x.CompanyId, x.JobTypeId });
                    table.ForeignKey(
                        name: "FK_FgsJobTypeAccounting_FgsJobType",
                        column: x => x.JobTypeId,
                        principalSchema: "setup",
                        principalTable: "FgsJobType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsJobTypeAccounting_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores optional accounting configuration that applies at the job type level independently of billing category, including Accounts Receivable and Discount GL account mappings.");

            migrationBuilder.CreateTable(
                name: "FgsJobTypeBillingCategoryAccounting",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key identity of the Job Type and Billing Category accounting configuration record.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier owning this Job Type and Billing Category accounting configuration."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier within the tenant owning this Job Type and Billing Category accounting configuration."),
                    JobTypeId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the Job Type to which the accounting override applies."),
                    BillingCategoryId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the Billing Category for which the Job Type accounting override is defined."),
                    RevenueAccountId = table.Column<long>(type: "bigint", nullable: true, comment: "Optional GL account used for revenue generated by the Job Type when the specified Billing Category applies. If not configured, the Billing Category default Revenue account can be used."),
                    CogsAccountId = table.Column<long>(type: "bigint", nullable: true, comment: "Optional GL account used for cost of goods sold when the specified Billing Category applies to the Job Type. If not configured, the Billing Category default COGS account can be used."),
                    InventoryOffsetAccountId = table.Column<long>(type: "bigint", nullable: true, comment: "Optional GL account used as the inventory offset when the specified Billing Category is an Inventory billing category. If not configured, the Billing Category default Inventory Offset account can be used."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the Job Type and Billing Category accounting configuration was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User identifier that created the Job Type and Billing Category accounting configuration."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the Job Type and Billing Category accounting configuration was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User identifier that last updated the Job Type and Billing Category accounting configuration.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsJobTypeBillingCategoryAccounting", x => x.Id);
                    table.UniqueConstraint("UQ_FgsJobTypeBillingCategoryAccounting_TenantId_CompanyId_JobTypeId_BillingCategoryId", x => new { x.TenantId, x.CompanyId, x.JobTypeId, x.BillingCategoryId });
                    table.ForeignKey(
                        name: "FK_FgsJobTypeBillingCategoryAccounting_FgsBillingCategory",
                        column: x => x.BillingCategoryId,
                        principalSchema: "setup",
                        principalTable: "FgsBillingCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsJobTypeBillingCategoryAccounting_FgsJobType",
                        column: x => x.JobTypeId,
                        principalSchema: "setup",
                        principalTable: "FgsJobType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsJobTypeBillingCategoryAccounting_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores optional accounting overrides for a specific Job Type and Billing Category combination. These mappings allow a Job Type to override the default Revenue, COGS, and Inventory Offset GL accounts defined for the Billing Category.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeAccounting_JobTypeId",
                schema: "setup",
                table: "FgsJobTypeAccounting",
                columns: new[] { "TenantId", "CompanyId", "JobTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeAccounting_TenantId_CompanyId",
                schema: "setup",
                table: "FgsJobTypeAccounting",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeBillingCategoryAccounting_TenantId_CompanyId_BillingCategoryId",
                schema: "setup",
                table: "FgsJobTypeBillingCategoryAccounting",
                columns: new[] { "TenantId", "CompanyId", "BillingCategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeBillingCategoryAccounting_TenantId_CompanyId_JobTypeId",
                schema: "setup",
                table: "FgsJobTypeBillingCategoryAccounting",
                columns: new[] { "TenantId", "CompanyId", "JobTypeId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsJobTypeAccounting",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsJobTypeBillingCategoryAccounting",
                schema: "setup");
        }
    }
}
