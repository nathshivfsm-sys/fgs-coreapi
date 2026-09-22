using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFgsJobTypeTaskSkillLevelId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SkillLevelId",
                schema: "setup",
                table: "FgsJobTypeTask",
                type: "bigint",
                nullable: true,
                comment: "Optional skill level required to perform this task.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeTask_SkillLevelId",
                schema: "setup",
                table: "FgsJobTypeTask",
                column: "SkillLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_FgsJobTypeTask_FgsSetupTechSkillLevel",
                schema: "setup",
                table: "FgsJobTypeTask",
                column: "SkillLevelId",
                principalSchema: "setup",
                principalTable: "FgsSetupTechSkillLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FgsJobTypeTask_FgsSetupTechSkillLevel",
                schema: "setup",
                table: "FgsJobTypeTask");

            migrationBuilder.DropIndex(
                name: "IX_FgsJobTypeTask_SkillLevelId",
                schema: "setup",
                table: "FgsJobTypeTask");

            migrationBuilder.DropColumn(
                name: "SkillLevelId",
                schema: "setup",
                table: "FgsJobTypeTask");
        }
    }
}
