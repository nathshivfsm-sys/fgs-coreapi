using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Crm.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCrmCustomerWebsite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Website",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                comment: "Customer website.");

            migrationBuilder.AddColumn<long>(
                name: "CustomerType",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: true,
                comment: "Classifies the service location customer type (Residential, Commercial, Property Management, Builder, HOA, Other).");

            migrationBuilder.AddCheckConstraint(
                name: "CK_CrmServiceLocation_CustomerType",
                schema: "crm",
                table: "CrmServiceLocation",
                sql: "\"CustomerType\" IS NULL OR \"CustomerType\" IN (1, 2, 3, 4, 5, 6)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_CrmServiceLocation_CustomerType",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "CustomerType",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "Website",
                schema: "crm",
                table: "CrmCustomer");
        }
    }
}
