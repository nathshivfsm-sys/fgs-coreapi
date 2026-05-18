using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class FgsCompanyIdMapsToCompanyNumber : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            """
            DO $convert$
            DECLARE
                r record;
            BEGIN
                FOR r IN
                    SELECT c.conname, n.nspname, t.relname AS table_name
                    FROM pg_constraint c
                    JOIN pg_class t ON c.conrelid = t.oid
                    JOIN pg_namespace n ON t.relnamespace = n.oid
                    WHERE n.nspname = 'dbo'
                      AND c.contype = 'f'
                      AND pg_get_constraintdef(c.oid) LIKE '%REFERENCES dbo."FgsTenantCompany"%'
                LOOP
                    EXECUTE format(
                        'ALTER TABLE %I.%I DROP CONSTRAINT IF EXISTS %I',
                        r.nspname,
                        r.table_name,
                        r.conname);
                END LOOP;

                FOR r IN
                    SELECT c.table_name
                    FROM information_schema.columns c
                    JOIN information_schema.tables t
                        ON c.table_schema = t.table_schema
                       AND c.table_name = t.table_name
                    WHERE c.table_schema = 'dbo'
                      AND c.column_name = 'CompanyId'
                      AND c.udt_name = 'uuid'
                      AND t.table_type = 'BASE TABLE'
                      AND c.table_name LIKE 'Fgs%'
                      AND c.table_name <> 'FgsTenant'
                LOOP
                    EXECUTE format(
                        'ALTER TABLE dbo.%I ADD COLUMN IF NOT EXISTS "__EF_CompanyIdBigint" bigint',
                        r.table_name);

                    EXECUTE format(
                        'UPDATE dbo.%I child
                         SET "__EF_CompanyIdBigint" = tc."CompanyNumber"
                         FROM dbo."FgsTenantCompany" tc
                         WHERE child."TenantId" = tc."TenantId"
                           AND child."CompanyId" = tc."CompanyGuid"',
                        r.table_name);

                    EXECUTE format(
                        'ALTER TABLE dbo.%I DROP COLUMN "CompanyId"',
                        r.table_name);

                    EXECUTE format(
                        'ALTER TABLE dbo.%I RENAME COLUMN "__EF_CompanyIdBigint" TO "CompanyId"',
                        r.table_name);

                    EXECUTE format(
                        'ALTER TABLE dbo.%I ALTER COLUMN "CompanyId" SET NOT NULL',
                        r.table_name);
                END LOOP;
            END
            $convert$;
            """);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            """
            DO $revert$
            DECLARE
                r record;
            BEGIN
                FOR r IN
                    SELECT c.conname, n.nspname, t.relname AS table_name
                    FROM pg_constraint c
                    JOIN pg_class t ON c.conrelid = t.oid
                    JOIN pg_namespace n ON t.relnamespace = n.oid
                    WHERE n.nspname = 'dbo'
                      AND c.contype = 'f'
                      AND pg_get_constraintdef(c.oid) LIKE '%REFERENCES dbo."FgsTenantCompany"%'
                LOOP
                    EXECUTE format(
                        'ALTER TABLE %I.%I DROP CONSTRAINT IF EXISTS %I',
                        r.nspname,
                        r.table_name,
                        r.conname);
                END LOOP;

                FOR r IN
                    SELECT c.table_name
                    FROM information_schema.columns c
                    JOIN information_schema.tables t
                        ON c.table_schema = t.table_schema
                       AND c.table_name = t.table_name
                    WHERE c.table_schema = 'dbo'
                      AND c.column_name = 'CompanyId'
                      AND c.udt_name = 'int8'
                      AND t.table_type = 'BASE TABLE'
                      AND c.table_name LIKE 'Fgs%'
                      AND c.table_name <> 'FgsTenant'
                LOOP
                    EXECUTE format(
                        'ALTER TABLE dbo.%I ADD COLUMN IF NOT EXISTS "__EF_CompanyIdUuid" uuid',
                        r.table_name);

                    EXECUTE format(
                        'UPDATE dbo.%I child
                         SET "__EF_CompanyIdUuid" = tc."CompanyGuid"
                         FROM dbo."FgsTenantCompany" tc
                         WHERE child."TenantId" = tc."TenantId"
                           AND child."CompanyId" = tc."CompanyNumber"',
                        r.table_name);

                    EXECUTE format(
                        'ALTER TABLE dbo.%I DROP COLUMN "CompanyId"',
                        r.table_name);

                    EXECUTE format(
                        'ALTER TABLE dbo.%I RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId"',
                        r.table_name);

                    EXECUTE format(
                        'ALTER TABLE dbo.%I ALTER COLUMN "CompanyId" SET NOT NULL',
                        r.table_name);
                END LOOP;
            END
            $revert$;
            """);
    }
}
