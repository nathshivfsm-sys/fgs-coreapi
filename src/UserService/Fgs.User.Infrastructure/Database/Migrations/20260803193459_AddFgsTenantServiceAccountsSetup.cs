using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFgsTenantServiceAccountsSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Model drift: unique email index is scoped by company (see FgsUserConfiguration).
            migrationBuilder.DropIndex(
                name: "IX_FgsUser_TenantId_CompanyId",
                schema: "identity",
                table: "FgsUser");

            migrationBuilder.DropIndex(
                name: "IX_FgsUser_TenantId_Email",
                schema: "identity",
                table: "FgsUser");

            migrationBuilder.CreateTable(
                name: "FgsTenantServiceAccountsSetup",
                schema: "tenant",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BankAccountId = table.Column<long>(type: "bigint", nullable: true, comment: "Default bank account used for customer payments, deposits, and cash transactions."),
                    AccountsReceivableAccountId = table.Column<long>(type: "bigint", nullable: true, comment: "General ledger account used to record customer accounts receivable."),
                    RevenueAccountId = table.Column<long>(type: "bigint", nullable: true, comment: "Default revenue or income account used when posting invoices and completed work orders."),
                    DiscountAccountId = table.Column<long>(type: "bigint", nullable: true, comment: "General ledger account used to record customer discounts and promotional adjustments."),
                    SalesTaxPayableAccountId = table.Column<long>(type: "bigint", nullable: true, comment: "Liability account used to record collected sales taxes owed to tax authorities."),
                    InventoryAccountId = table.Column<long>(type: "bigint", nullable: true, comment: "Asset account used to record the value of inventory on hand."),
                    COGSAccountId = table.Column<long>(type: "bigint", nullable: true, comment: "Cost of Goods Sold account used when inventory is consumed or sold."),
                    UndepositedFundsAccountId = table.Column<long>(type: "bigint", nullable: true, comment: "Holding account used for customer payments received but not yet deposited into a bank account."),
                    ProcessingFeeAccountId = table.Column<long>(type: "bigint", nullable: true, comment: "Expense account used to record merchant, credit card, and payment processing fees."),
                    AccountsPayableAccountId = table.Column<long>(type: "bigint", nullable: true, comment: "General ledger account used to record amounts owed to vendors and suppliers."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsTenantServiceAccountsSetup", x => new { x.TenantId, x.CompanyId });
                    table.ForeignKey(
                        name: "FK_FgsTenantServiceAccountsSetup_TenantCompany",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "tenant",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FgsUser_TenantId_CompanyId_Email",
                schema: "identity",
                table: "FgsUser",
                columns: new[] { "TenantId", "CompanyId", "Email" },
                unique: true,
                filter: "\"IsDeleted\" = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsTenantServiceAccountsSetup",
                schema: "tenant");

            migrationBuilder.DropIndex(
                name: "IX_FgsUser_TenantId_CompanyId_Email",
                schema: "identity",
                table: "FgsUser");

            migrationBuilder.CreateIndex(
                name: "IX_FgsUser_TenantId_CompanyId",
                schema: "identity",
                table: "FgsUser",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsUser_TenantId_Email",
                schema: "identity",
                table: "FgsUser",
                columns: new[] { "TenantId", "Email" },
                unique: true,
                filter: "\"IsDeleted\" = false");
        }
    }
}
