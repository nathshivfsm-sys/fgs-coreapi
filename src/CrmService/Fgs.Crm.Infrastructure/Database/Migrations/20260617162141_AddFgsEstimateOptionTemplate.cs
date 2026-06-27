using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Crm.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFgsEstimateOptionTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FgsEstimateOptionTemplate",
                schema: "crm",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    EstimateOptionId = table.Column<long>(type: "bigint", nullable: false, comment: "Parent estimate option."),
                    EstimateTemplateId = table.Column<long>(type: "bigint", nullable: false, comment: "Source estimate template applied to the estimate option."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Order in which templates were applied to the estimate option."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that created the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsEstimateOptionTemplate", x => x.Id);
                    table.CheckConstraint("CK_FgsEstimateOptionTemplate_DisplayOrder", "\"DisplayOrder\" > 0");
                    table.ForeignKey(
                        name: "FK_FgsEstimateOptionTemplate_EstimateOption",
                        column: x => x.EstimateOptionId,
                        principalSchema: "crm",
                        principalTable: "FgsEstimateOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsEstimateOptionTemplate_EstimateTemplate",
                        column: x => x.EstimateTemplateId,
                        principalSchema: "crm",
                        principalTable: "FgsEstimateTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsEstimateOptionTemplate_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "crm",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores estimate templates applied to an estimate option and tracks template contributions to pricing lines, clauses, and other estimate content.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateOptionTemplate_EstimateOptionId",
                schema: "crm",
                table: "FgsEstimateOptionTemplate",
                column: "EstimateOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateOptionTemplate_EstimateTemplateId",
                schema: "crm",
                table: "FgsEstimateOptionTemplate",
                column: "EstimateTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateOptionTemplate_TenantId_CompanyId",
                schema: "crm",
                table: "FgsEstimateOptionTemplate",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateOptionTemplate_TenantId_CompanyId_DisplayOrder",
                schema: "crm",
                table: "FgsEstimateOptionTemplate",
                columns: new[] { "TenantId", "CompanyId", "EstimateOptionId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateOptionTemplate_TenantId_CompanyId_EstimateOptionId",
                schema: "crm",
                table: "FgsEstimateOptionTemplate",
                columns: new[] { "TenantId", "CompanyId", "EstimateOptionId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateOptionTemplate_TenantId_CompanyId_EstimateTemplateId",
                schema: "crm",
                table: "FgsEstimateOptionTemplate",
                columns: new[] { "TenantId", "CompanyId", "EstimateTemplateId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsEstimateOptionTemplate_TenantId_CompanyId_OptionId_TemplateId",
                schema: "crm",
                table: "FgsEstimateOptionTemplate",
                columns: new[] { "TenantId", "CompanyId", "EstimateOptionId", "EstimateTemplateId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsEstimateOptionTemplate",
                schema: "crm");
        }
    }
}
