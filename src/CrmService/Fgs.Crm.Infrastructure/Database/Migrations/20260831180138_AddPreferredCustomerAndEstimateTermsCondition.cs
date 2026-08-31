using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Crm.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddPreferredCustomerAndEstimateTermsCondition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TermsConditionVersionId",
                schema: "crm",
                table: "FgsEstimate",
                type: "bigint",
                nullable: true,
                comment: "Reference to the specific terms and conditions version used by the estimate.");

            migrationBuilder.AddColumn<bool>(
                name: "IsPreferredCustomer",
                schema: "crm",
                table: "CrmCustomer",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Indicates whether the customer is designated as a preferred customer. TRUE indicates preferred customer status; FALSE indicates standard customer status.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TermsConditionVersionId",
                schema: "crm",
                table: "FgsEstimate");

            migrationBuilder.DropColumn(
                name: "IsPreferredCustomer",
                schema: "crm",
                table: "CrmCustomer");
        }
    }
}
