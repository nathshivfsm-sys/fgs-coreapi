using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class CompanySignupSchemaAlignments : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            """
            DROP INDEX IF EXISTS dbo."IX_FgsUser_TenantId_NormalizedEmail";
            ALTER TABLE dbo."FgsUser" DROP COLUMN IF EXISTS "NormalizedEmail";
            ALTER TABLE dbo."FgsUser" DROP COLUMN IF EXISTS "PasswordHash";

            DO $convert_company_size$
            BEGIN
                IF EXISTS (
                    SELECT 1
                    FROM information_schema.columns
                    WHERE table_schema = 'dbo'
                      AND table_name = 'FgsTenantCompany'
                      AND column_name = 'CompanySize'
                      AND udt_name = 'int4'
                ) THEN
                    ALTER TABLE dbo."FgsTenantCompany"
                    ALTER COLUMN "CompanySize" TYPE character varying(20)
                    USING (
                        CASE "CompanySize"::text
                            WHEN '1' THEN '1'
                            WHEN '2' THEN '2-5'
                            WHEN '3' THEN '6-10'
                            WHEN '4' THEN '11+'
                            ELSE "CompanySize"::text
                        END
                    );
                END IF;
            END
            $convert_company_size$;

            DO $convert_audit_actors$
            DECLARE
                r record;
            BEGIN
                FOR r IN
                    SELECT c.table_name, c.column_name
                    FROM information_schema.columns c
                    WHERE c.table_schema = 'dbo'
                      AND c.udt_name = 'uuid'
                      AND c.column_name IN ('CreatedBy', 'UpdatedBy')
                LOOP
                    EXECUTE format(
                        'ALTER TABLE dbo.%I ALTER COLUMN %I TYPE character varying(100) USING (
                            CASE %I::text
                                WHEN ''00000000-0000-0000-0000-000000000001'' THEN ''SYSTEM''
                                WHEN ''00000000-0000-0000-0000-000000000010'' THEN ''Prospect''
                                ELSE %I::text
                            END
                        )',
                        r.table_name,
                        r.column_name,
                        r.column_name,
                        r.column_name);
                END LOOP;
            END
            $convert_audit_actors$;

            CREATE UNIQUE INDEX IF NOT EXISTS "IX_FgsUser_TenantId_Email"
                ON dbo."FgsUser" ("TenantId", "Email")
                WHERE "IsDeleted" = false;
            """);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            """
            DROP INDEX IF EXISTS dbo."IX_FgsUser_TenantId_Email";

            DO $revert_audit_actors$
            DECLARE
                r record;
            BEGIN
                FOR r IN
                    SELECT c.table_name, c.column_name
                    FROM information_schema.columns c
                    WHERE c.table_schema = 'dbo'
                      AND c.udt_name = 'varchar'
                      AND c.character_maximum_length = 100
                      AND c.column_name IN ('CreatedBy', 'UpdatedBy')
                LOOP
                    EXECUTE format(
                        'ALTER TABLE dbo.%I ALTER COLUMN %I TYPE uuid USING (
                            CASE %I
                                WHEN ''SYSTEM'' THEN ''00000000-0000-0000-0000-000000000001''::uuid
                                WHEN ''Prospect'' THEN ''00000000-0000-0000-0000-000000000010''::uuid
                                WHEN %I ~ ''^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$''
                                    THEN %I::uuid
                                ELSE NULL
                            END
                        )',
                        r.table_name,
                        r.column_name,
                        r.column_name,
                        r.column_name,
                        r.column_name);
                END LOOP;
            END
            $revert_audit_actors$;

            DO $revert_company_size$
            BEGIN
                IF EXISTS (
                    SELECT 1
                    FROM information_schema.columns
                    WHERE table_schema = 'dbo'
                      AND table_name = 'FgsTenantCompany'
                      AND column_name = 'CompanySize'
                      AND udt_name = 'varchar'
                ) THEN
                    ALTER TABLE dbo."FgsTenantCompany"
                    ALTER COLUMN "CompanySize" TYPE integer
                    USING (
                        CASE "CompanySize"
                            WHEN '1' THEN 1
                            WHEN '2-5' THEN 2
                            WHEN '6-10' THEN 3
                            WHEN '11+' THEN 4
                            ELSE NULL
                        END
                    );
                END IF;
            END
            $revert_company_size$;

            ALTER TABLE dbo."FgsUser" ADD COLUMN IF NOT EXISTS "NormalizedEmail" character varying(300);
            ALTER TABLE dbo."FgsUser" ADD COLUMN IF NOT EXISTS "PasswordHash" character varying(500);
            UPDATE dbo."FgsUser" SET "NormalizedEmail" = UPPER("Email") WHERE "NormalizedEmail" IS NULL;
            ALTER TABLE dbo."FgsUser" ALTER COLUMN "NormalizedEmail" SET NOT NULL;

            CREATE UNIQUE INDEX IF NOT EXISTS "IX_FgsUser_TenantId_NormalizedEmail"
                ON dbo."FgsUser" ("TenantId", "NormalizedEmail")
                WHERE "IsDeleted" = false;
            """);
    }
}
