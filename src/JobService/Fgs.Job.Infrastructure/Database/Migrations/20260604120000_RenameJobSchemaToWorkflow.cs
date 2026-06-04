using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Job.Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class RenameJobSchemaToWorkflow : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            """
            DO $rename$
            BEGIN
                IF EXISTS (SELECT 1 FROM pg_namespace WHERE nspname = 'job')
                   AND NOT EXISTS (SELECT 1 FROM pg_namespace WHERE nspname = 'workflow') THEN
                    ALTER SCHEMA job RENAME TO workflow;
                ELSIF EXISTS (SELECT 1 FROM pg_namespace WHERE nspname = 'job')
                   AND EXISTS (SELECT 1 FROM pg_namespace WHERE nspname = 'workflow') THEN
                    DROP SCHEMA job CASCADE;
                END IF;
            END
            $rename$;
            """);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            """
            DO $rename$
            BEGIN
                IF EXISTS (SELECT 1 FROM pg_namespace WHERE nspname = 'workflow')
                   AND NOT EXISTS (SELECT 1 FROM pg_namespace WHERE nspname = 'job') THEN
                    ALTER SCHEMA workflow RENAME TO job;
                END IF;
            END
            $rename$;
            """);
    }
}
