using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Billing.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class SyncInvoiceWorkDescriptionFkIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FgsInvoiceWorkDescription_InvoiceId1",
                schema: "billing",
                table: "FgsInvoiceWorkDescription",
                column: "InvoiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FgsInvoiceWorkDescription_InvoiceId1",
                schema: "billing",
                table: "FgsInvoiceWorkDescription");
        }
    }
}
