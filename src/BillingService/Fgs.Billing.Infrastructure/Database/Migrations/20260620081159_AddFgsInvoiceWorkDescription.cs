using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Billing.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFgsInvoiceWorkDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FgsInvoiceWorkDescription",
                schema: "billing",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: false, comment: "Parent invoice identifier."),
                    ServiceDate = table.Column<DateOnly>(type: "date", nullable: false, comment: "Service date for the work performed entry."),
                    TechCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "Technician code associated with the work performed."),
                    UserName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "User who entered the work description."),
                    WorkDescription = table.Column<string>(type: "text", nullable: false, comment: "Work performed description."),
                    IsCustomerVisible = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the work description is visible to the customer."),
                    CreatedOn = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInvoiceWorkDescription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsInvoiceWorkDescription_FgsInvoice",
                        column: x => x.InvoiceId,
                        principalSchema: "billing",
                        principalTable: "FgsInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsInvoiceWorkDescription_TenantCompany",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "billing",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores technician and office-entered work descriptions associated with an invoice. Supports multiple work performed entries for an invoice.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvoiceWorkDescription_InvoiceId",
                schema: "billing",
                table: "FgsInvoiceWorkDescription",
                columns: new[] { "TenantId", "CompanyId", "InvoiceId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvoiceWorkDescription_IsCustomerVisible",
                schema: "billing",
                table: "FgsInvoiceWorkDescription",
                columns: new[] { "TenantId", "CompanyId", "IsCustomerVisible" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvoiceWorkDescription_ServiceDate",
                schema: "billing",
                table: "FgsInvoiceWorkDescription",
                columns: new[] { "TenantId", "CompanyId", "ServiceDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvoiceWorkDescription_TechCode",
                schema: "billing",
                table: "FgsInvoiceWorkDescription",
                columns: new[] { "TenantId", "CompanyId", "TechCode" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvoiceWorkDescription_TenantCompany",
                schema: "billing",
                table: "FgsInvoiceWorkDescription",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsInvoiceWorkDescription_Invoice_TechCode_ServiceDate",
                schema: "billing",
                table: "FgsInvoiceWorkDescription",
                columns: new[] { "TenantId", "CompanyId", "InvoiceId", "TechCode", "ServiceDate" },
                unique: true,
                filter: "\"TechCode\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsInvoiceWorkDescription",
                schema: "billing");
        }
    }
}
