using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeMasterEntityType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                INSERT INTO glo."GloMasterEntityType"
                (
                    "Code",
                    "IsDocumentAllowed",
                    "IsActive",
                    "SortOrder",
                    "CreatedOn",
                    "CreatedBy"
                )
                SELECT
                    'EMPLOYEE',
                    TRUE,
                    TRUE,
                    15,
                    timezone('utc', now()),
                    'System'
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM glo."GloMasterEntityType" t
                    WHERE t."Code" = 'EMPLOYEE'
                );
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DELETE FROM glo."GloMasterEntityType"
                WHERE "Code" = 'EMPLOYEE';
                """);
        }
    }
}
