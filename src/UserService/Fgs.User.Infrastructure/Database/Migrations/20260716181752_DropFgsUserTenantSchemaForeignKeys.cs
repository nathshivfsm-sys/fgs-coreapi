using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class DropFgsUserTenantSchemaForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FgsUser_FgsTenantCompany_TenantId_CompanyId",
                schema: "identity",
                table: "FgsUser");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsUser_FgsTenant_TenantId",
                schema: "identity",
                table: "FgsUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_FgsUser_FgsTenantCompany_TenantId_CompanyId",
                schema: "identity",
                table: "FgsUser",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "tenant",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyNumber" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsUser_FgsTenant_TenantId",
                schema: "identity",
                table: "FgsUser",
                column: "TenantId",
                principalSchema: "tenant",
                principalTable: "FgsTenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
