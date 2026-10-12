using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Crm.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCrmServiceLocationBreakIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Break1Id",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: true,
                comment: "First accounting segment used for GL exports and reporting.");

            migrationBuilder.AddColumn<long>(
                name: "Break2Id",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: true,
                comment: "Second accounting segment used for GL exports and reporting.");

            migrationBuilder.CreateIndex(
                name: "IX_CrmServiceLocation_Break1Id",
                schema: "crm",
                table: "CrmServiceLocation",
                columns: new[] { "TenantId", "CompanyId", "Break1Id" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmServiceLocation_Break2Id",
                schema: "crm",
                table: "CrmServiceLocation",
                columns: new[] { "TenantId", "CompanyId", "Break2Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CrmServiceLocation_Break1Id",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropIndex(
                name: "IX_CrmServiceLocation_Break2Id",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "Break1Id",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "Break2Id",
                schema: "crm",
                table: "CrmServiceLocation");
        }
    }
}
