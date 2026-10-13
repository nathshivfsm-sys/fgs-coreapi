using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFgsUserGridPreference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_FgsUser_Id_TenantId_CompanyId",
                schema: "identity",
                table: "FgsUser",
                columns: new[] { "Id", "TenantId", "CompanyId" });

            migrationBuilder.CreateTable(
                name: "FgsUserGridPreference",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique identifier for the user grid preference record.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the tenant to which the user and grid preference belong."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the company within the tenant to which the user and grid preference belong."),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Identifier of the user whose grid preference is being stored."),
                    GridCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Unique application-defined code identifying the grid for which the user preferences are stored, such as WorkOrderList or EstimateList."),
                    GridConfiguration = table.Column<string>(type: "jsonb", nullable: false, defaultValueSql: "'{}'::jsonb", comment: "JSON configuration containing user-specific grid preferences such as column width, visibility, display order, pinning, sorting, filtering, grouping, and other supported grid settings."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "UTC timestamp when the user grid preference record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Identifier of the user or process that created the user grid preference record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "UTC timestamp when the user grid preference record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Identifier of the user or process that last updated the user grid preference record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsUserGridPreference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsUserGridPreference_FgsTenantCompanyCache",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "identity",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsUserGridPreference_FgsUser",
                        columns: x => new { x.UserId, x.TenantId, x.CompanyId },
                        principalSchema: "identity",
                        principalTable: "FgsUser",
                        principalColumns: new[] { "Id", "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Stores user-specific grid layout and display preferences for each grid within a tenant and company.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsUserGridPreference_TenantId_CompanyId_GridCode",
                schema: "identity",
                table: "FgsUserGridPreference",
                columns: new[] { "TenantId", "CompanyId", "GridCode" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsUserGridPreference_UserId_TenantId_CompanyId",
                schema: "identity",
                table: "FgsUserGridPreference",
                columns: new[] { "UserId", "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UQ_FgsUserGridPreference_TenantId_CompanyId_UserId_GridCode",
                schema: "identity",
                table: "FgsUserGridPreference",
                columns: new[] { "TenantId", "CompanyId", "UserId", "GridCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsUserGridPreference",
                schema: "identity");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_FgsUser_Id_TenantId_CompanyId",
                schema: "identity",
                table: "FgsUser");
        }
    }
}
