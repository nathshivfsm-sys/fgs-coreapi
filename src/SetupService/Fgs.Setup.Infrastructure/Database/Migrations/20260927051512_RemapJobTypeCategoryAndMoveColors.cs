using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemapJobTypeCategoryAndMoveColors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackgroundColor",
                schema: "setup",
                table: "FgsJobCategory",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "#FFFFFF",
                comment: "Background color for displaying the Job Category, stored as a HEX color value such as #3B82F6.");

            migrationBuilder.AddColumn<string>(
                name: "TextColor",
                schema: "setup",
                table: "FgsJobCategory",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "#000000",
                comment: "Text color for displaying the Job Category, stored as a HEX color value such as #FFFFFF.");

            migrationBuilder.DropColumn(
                name: "BackgroundColor",
                schema: "setup",
                table: "FgsJobType");

            migrationBuilder.DropColumn(
                name: "TextColor",
                schema: "setup",
                table: "FgsJobType");

            // Keep FgsJobTypeTask.JobTypeCategoryId and its indexes; drop only the FK so the mapping table can be replaced.
            migrationBuilder.DropForeignKey(
                name: "FK_FgsJobTypeTask_FgsJobTypeCategory",
                schema: "setup",
                table: "FgsJobTypeTask");

            migrationBuilder.Sql("""DELETE FROM setup."FgsJobTypeTask";""");

            migrationBuilder.DropTable(
                name: "FgsJobTypeCategory",
                schema: "setup");

            migrationBuilder.CreateTable(
                name: "FgsJobTypeCategory",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique identifier for the Job Type Category mapping.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the tenant that owns this Job Type Category mapping."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the company within the tenant that owns this Job Type Category mapping."),
                    JobTypeId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the Job Type."),
                    JobTypeTaskId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the Job Type Task assigned to the Job Type."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls the display sequence of Job Type Tasks within the Job Type."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time when the mapping was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the mapping."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time when the mapping was last modified."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last modified the mapping."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the Job Type Task assignment is active.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsJobTypeCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsJobTypeCategory_FgsJobType",
                        column: x => x.JobTypeId,
                        principalSchema: "setup",
                        principalTable: "FgsJobType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsJobTypeCategory_FgsJobTypeTask",
                        column: x => x.JobTypeTaskId,
                        principalSchema: "setup",
                        principalTable: "FgsJobTypeTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsJobTypeCategory_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Maps Job Type Tasks to Job Types. A Job Type can contain multiple Job Type Tasks, each with its own display order.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeCategory_JobTypeId",
                schema: "setup",
                table: "FgsJobTypeCategory",
                column: "JobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeCategory_JobTypeTaskId",
                schema: "setup",
                table: "FgsJobTypeCategory",
                column: "JobTypeTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeCategory_Tenant_Company",
                schema: "setup",
                table: "FgsJobTypeCategory",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeCategory_Tenant_Company_JobType",
                schema: "setup",
                table: "FgsJobTypeCategory",
                columns: new[] { "TenantId", "CompanyId", "JobTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeCategory_Tenant_Company_JobType_DisplayOrder",
                schema: "setup",
                table: "FgsJobTypeCategory",
                columns: new[] { "TenantId", "CompanyId", "JobTypeId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsJobTypeCategory_Tenant_Company_JobType_Task",
                schema: "setup",
                table: "FgsJobTypeCategory",
                columns: new[] { "TenantId", "CompanyId", "JobTypeId", "JobTypeTaskId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsJobTypeTask_FgsJobTypeCategory",
                schema: "setup",
                table: "FgsJobTypeTask",
                column: "JobTypeCategoryId",
                principalSchema: "setup",
                principalTable: "FgsJobTypeCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FgsJobTypeTask_FgsJobTypeCategory",
                schema: "setup",
                table: "FgsJobTypeTask");

            migrationBuilder.Sql("""DELETE FROM setup."FgsJobTypeTask";""");

            migrationBuilder.DropTable(
                name: "FgsJobTypeCategory",
                schema: "setup");

            migrationBuilder.CreateTable(
                name: "FgsJobTypeCategory",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique identifier for the Job Type Category mapping.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the tenant that owns this Job Type Category mapping."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the company within the tenant that owns this Job Type Category mapping."),
                    JobTypeId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the Job Type."),
                    JobCategoryId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the Job Category assigned to the Job Type."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls the display sequence of Job Categories within the Job Type."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time when the mapping was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the mapping."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time when the mapping was last modified."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last modified the mapping."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the Job Category assignment is active.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsJobTypeCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsJobTypeCategory_FgsJobCategory",
                        column: x => x.JobCategoryId,
                        principalSchema: "setup",
                        principalTable: "FgsJobCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsJobTypeCategory_FgsJobType",
                        column: x => x.JobTypeId,
                        principalSchema: "setup",
                        principalTable: "FgsJobType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsJobTypeCategory_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Maps Job Categories to Job Types. A Job Type can contain one or more Job Categories, each with its own display order.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeCategory_JobCategoryId",
                schema: "setup",
                table: "FgsJobTypeCategory",
                column: "JobCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeCategory_JobTypeId",
                schema: "setup",
                table: "FgsJobTypeCategory",
                column: "JobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeCategory_Tenant_Company",
                schema: "setup",
                table: "FgsJobTypeCategory",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeCategory_Tenant_Company_JobCategory",
                schema: "setup",
                table: "FgsJobTypeCategory",
                columns: new[] { "TenantId", "CompanyId", "JobCategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeCategory_Tenant_Company_JobType",
                schema: "setup",
                table: "FgsJobTypeCategory",
                columns: new[] { "TenantId", "CompanyId", "JobTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeCategory_Tenant_Company_DisplayOrder",
                schema: "setup",
                table: "FgsJobTypeCategory",
                columns: new[] { "TenantId", "CompanyId", "JobTypeId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsJobTypeCategory_Tenant_Company_JobType_Category",
                schema: "setup",
                table: "FgsJobTypeCategory",
                columns: new[] { "TenantId", "CompanyId", "JobTypeId", "JobCategoryId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsJobTypeTask_FgsJobTypeCategory",
                schema: "setup",
                table: "FgsJobTypeTask",
                column: "JobTypeCategoryId",
                principalSchema: "setup",
                principalTable: "FgsJobTypeCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.DropColumn(
                name: "BackgroundColor",
                schema: "setup",
                table: "FgsJobCategory");

            migrationBuilder.DropColumn(
                name: "TextColor",
                schema: "setup",
                table: "FgsJobCategory");

            migrationBuilder.AddColumn<string>(
                name: "BackgroundColor",
                schema: "setup",
                table: "FgsJobType",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                comment: "Optional background color used when displaying the Job Type in the user interface.");

            migrationBuilder.AddColumn<string>(
                name: "TextColor",
                schema: "setup",
                table: "FgsJobType",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                comment: "Optional text color used when displaying the Job Type in the user interface.");
        }
    }
}
