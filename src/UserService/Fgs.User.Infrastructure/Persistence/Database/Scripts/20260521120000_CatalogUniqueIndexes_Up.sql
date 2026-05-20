-- =============================================================================
-- Migration: 20260521120000_CatalogUniqueIndexes
-- Adds UX_* unique CONSTRAINTs (table constraint section), not standalone indexes.
-- Tables: GloAccountingIntegrationType, GloBusinessType, FgsTenantCompany
-- Pair with: Database/Migrations/20260521120000_CatalogUniqueIndexes.cs
-- =============================================================================

START TRANSACTION;

-- ---------------------------------------------------------------------------
-- GloAccountingIntegrationType
-- ---------------------------------------------------------------------------
DROP INDEX IF EXISTS dbo."IX_GloAccountingIntegrationType_Code";
DROP INDEX IF EXISTS dbo."UX_AccountingIntegrationType_Code";

DO $EF$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint c
        INNER JOIN pg_class t ON t.oid = c.conrelid
        INNER JOIN pg_namespace n ON n.oid = t.relnamespace
        WHERE n.nspname = 'dbo'
          AND t.relname = 'GloAccountingIntegrationType'
          AND c.conname = 'UX_AccountingIntegrationType_Code'
    ) THEN
        ALTER TABLE dbo."GloAccountingIntegrationType"
            ADD CONSTRAINT "UX_AccountingIntegrationType_Code" UNIQUE ("Code");
    END IF;
END $EF$;

-- ---------------------------------------------------------------------------
-- GloBusinessType
-- ---------------------------------------------------------------------------
DROP INDEX IF EXISTS dbo."IX_GloBusinessType_Code";
DROP INDEX IF EXISTS dbo."UX_BusinessType_Code";

DO $EF$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint c
        INNER JOIN pg_class t ON t.oid = c.conrelid
        INNER JOIN pg_namespace n ON n.oid = t.relnamespace
        WHERE n.nspname = 'dbo'
          AND t.relname = 'GloBusinessType'
          AND c.conname = 'UX_BusinessType_Code'
    ) THEN
        ALTER TABLE dbo."GloBusinessType"
            ADD CONSTRAINT "UX_BusinessType_Code" UNIQUE ("Code");
    END IF;
END $EF$;

-- ---------------------------------------------------------------------------
-- FgsTenantCompany (TenantId, CompanyNumber)
-- ---------------------------------------------------------------------------
DROP INDEX IF EXISTS dbo."IX_FgsTenantCompany_TenantId_CompanyNumber";
DROP INDEX IF EXISTS dbo."UX_Company_Tenant_CompanyNumber";

DO $EF$
BEGIN
    IF EXISTS (
        SELECT 1
        FROM pg_constraint c
        INNER JOIN pg_class t ON t.oid = c.conrelid
        INNER JOIN pg_namespace n ON n.oid = t.relnamespace
        WHERE n.nspname = 'dbo'
          AND t.relname = 'FgsTenantCompany'
          AND c.conname = 'AK_FgsTenantCompany_TenantId_CompanyNumber'
    )
    AND NOT EXISTS (
        SELECT 1
        FROM pg_constraint c
        INNER JOIN pg_class t ON t.oid = c.conrelid
        INNER JOIN pg_namespace n ON n.oid = t.relnamespace
        WHERE n.nspname = 'dbo'
          AND t.relname = 'FgsTenantCompany'
          AND c.conname = 'UX_Company_Tenant_CompanyNumber'
    ) THEN
        ALTER TABLE dbo."FgsTenantCompany"
            RENAME CONSTRAINT "AK_FgsTenantCompany_TenantId_CompanyNumber"
            TO "UX_Company_Tenant_CompanyNumber";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint c
        INNER JOIN pg_class t ON t.oid = c.conrelid
        INNER JOIN pg_namespace n ON n.oid = t.relnamespace
        WHERE n.nspname = 'dbo'
          AND t.relname = 'FgsTenantCompany'
          AND c.conname = 'UX_Company_Tenant_CompanyNumber'
    ) THEN
        ALTER TABLE dbo."FgsTenantCompany"
            ADD CONSTRAINT "UX_Company_Tenant_CompanyNumber" UNIQUE ("TenantId", "CompanyNumber");
    END IF;
END $EF$;

-- ---------------------------------------------------------------------------
-- FgsTenantCompany (TenantId, Code)
-- ---------------------------------------------------------------------------
DROP INDEX IF EXISTS dbo."IX_FgsTenantCompany_TenantId_Code";
DROP INDEX IF EXISTS dbo."UX_Company_Tenant_Code";

DO $EF$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint c
        INNER JOIN pg_class t ON t.oid = c.conrelid
        INNER JOIN pg_namespace n ON n.oid = t.relnamespace
        WHERE n.nspname = 'dbo'
          AND t.relname = 'FgsTenantCompany'
          AND c.conname = 'UX_Company_Tenant_Code'
    ) THEN
        ALTER TABLE dbo."FgsTenantCompany"
            ADD CONSTRAINT "UX_Company_Tenant_Code" UNIQUE ("TenantId", "Code");
    END IF;
END $EF$;

INSERT INTO dbo."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260521120000_CatalogUniqueIndexes', '10.0.8')
ON CONFLICT ("MigrationId") DO NOTHING;

COMMIT;
