-- =============================================================================
-- Migration: 20260518135530_CompanySignupSchemaAlignments
-- Script:   20260518135530_CompanySignupSchemaAlignments_Down.sql
-- Database: PostgreSQL (schema: dbo)
--
-- Rollback:
--   1. Restores uuid audit columns, integer CompanySize, and FgsUser identity columns.
--   2. Removes MigrationId from "__EFMigrationsHistory".
--
-- Notes:
--   - Idempotent DO blocks (matches Down() in 20260518135530_CompanySignupSchemaAlignments.cs)
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518135530_CompanySignupSchemaAlignments') THEN
    DROP INDEX IF EXISTS dbo."IX_FgsUser_TenantId_Email";
    END IF;
END $EF$;

DO $EF$
DECLARE
    r record;
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518135530_CompanySignupSchemaAlignments') THEN
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
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518135530_CompanySignupSchemaAlignments') THEN
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
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518135530_CompanySignupSchemaAlignments') THEN
    ALTER TABLE dbo."FgsUser" ADD COLUMN IF NOT EXISTS "NormalizedEmail" character varying(300);
    ALTER TABLE dbo."FgsUser" ADD COLUMN IF NOT EXISTS "PasswordHash" character varying(500);
    UPDATE dbo."FgsUser" SET "NormalizedEmail" = UPPER("Email") WHERE "NormalizedEmail" IS NULL;
    ALTER TABLE dbo."FgsUser" ALTER COLUMN "NormalizedEmail" SET NOT NULL;
    CREATE UNIQUE INDEX IF NOT EXISTS "IX_FgsUser_TenantId_NormalizedEmail"
        ON dbo."FgsUser" ("TenantId", "NormalizedEmail")
        WHERE "IsDeleted" = false;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518135530_CompanySignupSchemaAlignments') THEN
    DELETE FROM dbo."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260518135530_CompanySignupSchemaAlignments';
    END IF;
END $EF$;

COMMIT;
