using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFgsNonWorkingDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FgsNonWorkingDate",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key identity of the non-working date record.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier owning this non-working date."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier within the tenant owning this non-working date."),
                    NonWorkingDate = table.Column<DateOnly>(type: "date", nullable: false, comment: "Calendar date on which the company does not operate under its normal working schedule."),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Name identifying the non-working date, such as New Year's Day, Thanksgiving, Company Holiday, or Emergency Closure."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the non-working date record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User identifier that created the non-working date record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the non-working date record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User identifier that last updated the non-working date record."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the non-working date is active and should be considered when determining business availability and scheduling.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsNonWorkingDate", x => x.Id);
                    table.UniqueConstraint("UQ_FgsNonWorkingDate_TenantId_CompanyId_NonWorkingDate", x => new { x.TenantId, x.CompanyId, x.NonWorkingDate });
                    table.ForeignKey(
                        name: "FK_FgsNonWorkingDate_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores tenant/company specific calendar dates on which normal business operations are not scheduled.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsNonWorkingDate_TenantId_CompanyId_IsActive",
                schema: "setup",
                table: "FgsNonWorkingDate",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsNonWorkingDate",
                schema: "setup");
        }
    }
}
