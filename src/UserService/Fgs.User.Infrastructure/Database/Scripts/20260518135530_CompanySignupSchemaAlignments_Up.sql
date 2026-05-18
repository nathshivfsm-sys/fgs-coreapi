-- =============================================================================
-- Migration: 20260518135530_CompanySignupSchemaAlignments
-- Script:   20260518135530_CompanySignupSchemaAlignments_Up.sql
-- Database: PostgreSQL (schema: dbo)
--
-- Behavior:
--   1. Drops FgsUser.NormalizedEmail and PasswordHash; adds unique (TenantId, Email).
--   2. Changes FgsTenantCompany.CompanySize from integer to varchar(20) UI labels.
--   3. Converts CreatedBy / UpdatedBy from uuid to varchar(100) on all dbo tables.
--   4. Records MigrationId in "__EFMigrationsHistory" when not yet present.
--
-- Notes:
--   - Idempotent DO blocks (matches: 20260518135530_CompanySignupSchemaAlignments.cs)
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518135530_CompanySignupSchemaAlignments') THEN

    DROP INDEX IF EXISTS dbo."IX_FgsUser_TenantId_NormalizedEmail";
    ALTER TABLE dbo."FgsUser" DROP COLUMN IF EXISTS "NormalizedEmail";
    ALTER TABLE dbo."FgsUser" DROP COLUMN IF EXISTS "PasswordHash";

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

    END IF;
END $EF$;

DO $EF$
DECLARE
    r record;
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518135530_CompanySignupSchemaAlignments') THEN
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
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518135530_CompanySignupSchemaAlignments') THEN
    CREATE UNIQUE INDEX IF NOT EXISTS "IX_FgsUser_TenantId_Email"
        ON dbo."FgsUser" ("TenantId", "Email")
        WHERE "IsDeleted" = false;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518135530_CompanySignupSchemaAlignments') THEN
    INSERT INTO dbo."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260518135530_CompanySignupSchemaAlignments', '10.0.0');
    END IF;
END $EF$;

COMMIT;
