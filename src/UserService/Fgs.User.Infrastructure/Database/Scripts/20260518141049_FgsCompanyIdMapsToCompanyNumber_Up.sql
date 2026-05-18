-- =============================================================================
-- Migration: 20260518141049_FgsCompanyIdMapsToCompanyNumber
-- Script:   20260518141049_FgsCompanyIdMapsToCompanyNumber_Up.sql
-- Database: PostgreSQL (schema: dbo)
--
-- Behavior:
--   Converts CompanyId on all Fgs* tables (except FgsTenant) from uuid to bigint,
--   populated from FgsTenantCompany.CompanyNumber via CompanyGuid match.
--   FgsTenantCompany is not modified.
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518141049_FgsCompanyIdMapsToCompanyNumber') THEN
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
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518141049_FgsCompanyIdMapsToCompanyNumber') THEN
    INSERT INTO dbo."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260518141049_FgsCompanyIdMapsToCompanyNumber', '10.0.0');
    END IF;
END $EF$;

COMMIT;
