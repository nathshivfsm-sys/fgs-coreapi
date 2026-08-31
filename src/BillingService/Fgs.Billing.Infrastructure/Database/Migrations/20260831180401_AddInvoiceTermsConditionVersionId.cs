using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Billing.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddInvoiceTermsConditionVersionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TermsConditionVersionId",
                schema: "billing",
                table: "FgsInvoice",
                type: "bigint",
                nullable: true,
                comment: "Reference to the specific terms and conditions version used by the invoice.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TermsConditionVersionId",
                schema: "billing",
                table: "FgsInvoice");
        }
    }
}
