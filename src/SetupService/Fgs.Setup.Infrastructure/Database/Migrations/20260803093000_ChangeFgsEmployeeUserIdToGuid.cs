using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class ChangeFgsEmployeeUserIdToGuid : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Existing bigint values cannot map to identity FgsUser (uuid). Clear then convert.
        migrationBuilder.Sql(
            """
            DROP INDEX IF EXISTS setup."UX_FgsEmployee_UserId";

            ALTER TABLE setup."FgsEmployee"
            ALTER COLUMN "UserId" DROP DEFAULT;

            ALTER TABLE setup."FgsEmployee"
            ALTER COLUMN "UserId" TYPE uuid
            USING NULL;

            COMMENT ON COLUMN setup."FgsEmployee"."UserId"
                IS 'Optional reference to the system user account associated with this employee. One user may be linked to only one employee. References identity service; no FK by design.';

            CREATE UNIQUE INDEX "UX_FgsEmployee_UserId"
                ON setup."FgsEmployee" ("UserId")
                WHERE "UserId" IS NOT NULL;
            """);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            """
            DROP INDEX IF EXISTS setup."UX_FgsEmployee_UserId";

            ALTER TABLE setup."FgsEmployee"
            ALTER COLUMN "UserId" DROP DEFAULT;

            ALTER TABLE setup."FgsEmployee"
            ALTER COLUMN "UserId" TYPE bigint
            USING NULL;

            COMMENT ON COLUMN setup."FgsEmployee"."UserId"
                IS 'Optional reference to the system user account associated with this employee. One user may be linked to only one employee. References identity service; no FK by design.';

            CREATE UNIQUE INDEX "UX_FgsEmployee_UserId"
                ON setup."FgsEmployee" ("UserId")
                WHERE "UserId" IS NOT NULL;
            """);
    }
}
