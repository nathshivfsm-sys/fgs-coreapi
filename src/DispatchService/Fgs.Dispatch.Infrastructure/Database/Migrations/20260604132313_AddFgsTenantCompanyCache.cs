using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Dispatch.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFgsTenantCompanyCache : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dispatch");

            migrationBuilder.CreateTable(
                name: "FgsTenantCompanyCache",
                schema: "dispatch",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier mapped from tenant.FgsTenantCompany.CompanyNumber."),
                    CompanyGuid = table.Column<Guid>(type: "uuid", nullable: false, comment: "Globally unique company identifier used by integrations and external systems."),
                    CompanyCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Unique company code within a tenant."),
                    CompanyName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Display name of the company."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, comment: "Indicates whether the company is active."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Timestamp of the most recent synchronization from tenant.FgsTenantCompany.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsTenantCompanyCache", x => new { x.TenantId, x.CompanyId });
                },
                comment: "Local cache of tenant company information used by the Dispatch schema to eliminate cross-schema dependencies on tenant.FgsTenantCompany.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsTenantCompanyCache_CompanyName",
                schema: "dispatch",
                table: "FgsTenantCompanyCache",
                column: "CompanyName");

            migrationBuilder.CreateIndex(
                name: "IX_FgsTenantCompanyCache_IsActive",
                schema: "dispatch",
                table: "FgsTenantCompanyCache",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "UQ_FgsTenantCompanyCache_CompanyGuid",
                schema: "dispatch",
                table: "FgsTenantCompanyCache",
                column: "CompanyGuid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsTenantCompanyCache",
                schema: "dispatch");
        }
    }
}
