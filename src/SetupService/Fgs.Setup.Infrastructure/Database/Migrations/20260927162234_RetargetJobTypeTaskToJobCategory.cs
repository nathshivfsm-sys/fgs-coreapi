using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RetargetJobTypeTaskToJobCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FgsJobTypeTask_FgsJobTypeCategory",
                schema: "setup",
                table: "FgsJobTypeTask");

            migrationBuilder.Sql(
                """
                DELETE FROM setup."FgsJobTypeTask" t
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM setup."FgsJobCategory" jc
                    WHERE jc."Id" = t."JobTypeCategoryId"
                      AND jc."TenantId" = t."TenantId"
                      AND jc."CompanyId" = t."CompanyId"
                );
                """);

            migrationBuilder.RenameColumn(
                name: "JobTypeCategoryId",
                schema: "setup",
                table: "FgsJobTypeTask",
                newName: "JobCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_FgsJobTypeTask_JobTypeCategoryId",
                schema: "setup",
                table: "FgsJobTypeTask",
                newName: "IX_FgsJobTypeTask_JobCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_FgsJobTypeTask_Tenant_Company_JobTypeCategory",
                schema: "setup",
                table: "FgsJobTypeTask",
                newName: "IX_FgsJobTypeTask_Tenant_Company_JobCategory");

            migrationBuilder.RenameIndex(
                name: "UX_FgsJobTypeTask_Tenant_Company_JobTypeCategory_Name",
                schema: "setup",
                table: "FgsJobTypeTask",
                newName: "UX_FgsJobTypeTask_Tenant_Company_JobCategory_Name");

            migrationBuilder.AlterTable(
                name: "FgsJobTypeTask",
                schema: "setup",
                comment: "Stores the tasks that belong to a Job Category (master catalog). Each task defines the work to be performed, along with its associated Trade, Priority, and estimated labor hours.",
                oldComment: "Stores the tasks that belong to a Job Type Category. Each task defines the work to be performed, along with its associated Trade, Priority, and estimated labor hours.");

            migrationBuilder.AlterColumn<long>(
                name: "JobCategoryId",
                schema: "setup",
                table: "FgsJobTypeTask",
                type: "bigint",
                nullable: false,
                comment: "Identifier of the Job Category (master catalog) that owns this task.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Identifier of the Job Type Category that owns this task.");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "setup",
                table: "FgsJobTypeTask",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                comment: "Sub-category name unique within the parent Job Category.",
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150,
                oldComment: "Sub-category name unique within the parent Job Type Category.");

            migrationBuilder.AlterColumn<short>(
                name: "DisplayOrder",
                schema: "setup",
                table: "FgsJobTypeTask",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1,
                comment: "Controls the display sequence of tasks within the Job Category.",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldDefaultValue: (short)1,
                oldComment: "Controls the display sequence of tasks within the Job Type Category.");

            migrationBuilder.AddForeignKey(
                name: "FK_FgsJobTypeTask_FgsJobCategory",
                schema: "setup",
                table: "FgsJobTypeTask",
                column: "JobCategoryId",
                principalSchema: "setup",
                principalTable: "FgsJobCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FgsJobTypeTask_FgsJobCategory",
                schema: "setup",
                table: "FgsJobTypeTask");

            migrationBuilder.Sql(
                """
                DELETE FROM setup."FgsJobTypeTask" t
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM setup."FgsJobTypeCategory" jtc
                    WHERE jtc."Id" = t."JobCategoryId"
                      AND jtc."TenantId" = t."TenantId"
                      AND jtc."CompanyId" = t."CompanyId"
                );
                """);

            migrationBuilder.RenameColumn(
                name: "JobCategoryId",
                schema: "setup",
                table: "FgsJobTypeTask",
                newName: "JobTypeCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_FgsJobTypeTask_JobCategoryId",
                schema: "setup",
                table: "FgsJobTypeTask",
                newName: "IX_FgsJobTypeTask_JobTypeCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_FgsJobTypeTask_Tenant_Company_JobCategory",
                schema: "setup",
                table: "FgsJobTypeTask",
                newName: "IX_FgsJobTypeTask_Tenant_Company_JobTypeCategory");

            migrationBuilder.RenameIndex(
                name: "UX_FgsJobTypeTask_Tenant_Company_JobCategory_Name",
                schema: "setup",
                table: "FgsJobTypeTask",
                newName: "UX_FgsJobTypeTask_Tenant_Company_JobTypeCategory_Name");

            migrationBuilder.AlterTable(
                name: "FgsJobTypeTask",
                schema: "setup",
                comment: "Stores the tasks that belong to a Job Type Category. Each task defines the work to be performed, along with its associated Trade, Priority, and estimated labor hours.",
                oldComment: "Stores the tasks that belong to a Job Category (master catalog). Each task defines the work to be performed, along with its associated Trade, Priority, and estimated labor hours.");

            migrationBuilder.AlterColumn<long>(
                name: "JobTypeCategoryId",
                schema: "setup",
                table: "FgsJobTypeTask",
                type: "bigint",
                nullable: false,
                comment: "Identifier of the Job Type Category that owns this task.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Identifier of the Job Category (master catalog) that owns this task.");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "setup",
                table: "FgsJobTypeTask",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                comment: "Sub-category name unique within the parent Job Type Category.",
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150,
                oldComment: "Sub-category name unique within the parent Job Category.");

            migrationBuilder.AlterColumn<short>(
                name: "DisplayOrder",
                schema: "setup",
                table: "FgsJobTypeTask",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1,
                comment: "Controls the display sequence of tasks within the Job Type Category.",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldDefaultValue: (short)1,
                oldComment: "Controls the display sequence of tasks within the Job Category.");

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
    }
}
