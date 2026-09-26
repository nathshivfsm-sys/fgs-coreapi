using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFgsJobTypeTaskSubCategoryName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TaskName",
                schema: "setup",
                table: "FgsJobTypeTask",
                type: "character varying(350)",
                maxLength: 350,
                nullable: false,
                comment: "Name of the task to be performed. Defaults to category name plus a space plus the sub-category name.",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldComment: "Name of the task to be performed.");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "setup",
                table: "FgsJobTypeTask",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                comment: "Sub-category name unique within the parent Job Type Category.");

            migrationBuilder.CreateIndex(
                name: "UX_FgsJobTypeTask_Tenant_Company_JobTypeCategory_Name",
                schema: "setup",
                table: "FgsJobTypeTask",
                columns: new[] { "TenantId", "CompanyId", "JobTypeCategoryId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_FgsJobTypeTask_Tenant_Company_JobTypeCategory_Name",
                schema: "setup",
                table: "FgsJobTypeTask");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "setup",
                table: "FgsJobTypeTask");

            migrationBuilder.AlterColumn<string>(
                name: "TaskName",
                schema: "setup",
                table: "FgsJobTypeTask",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                comment: "Name of the task to be performed.",
                oldClrType: typeof(string),
                oldType: "character varying(350)",
                oldMaxLength: 350,
                oldComment: "Name of the task to be performed. Defaults to category name plus a space plus the sub-category name.");
        }
    }
}
