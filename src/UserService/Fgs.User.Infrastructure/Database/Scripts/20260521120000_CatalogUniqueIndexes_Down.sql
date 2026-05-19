-- =============================================================================
-- Revert: 20260521120000_CatalogUniqueIndexes
-- =============================================================================

START TRANSACTION;

ALTER TABLE dbo."FgsTenantCompany"
    DROP CONSTRAINT IF EXISTS "UX_Company_Tenant_Code";

CREATE UNIQUE INDEX IF NOT EXISTS "IX_FgsTenantCompany_TenantId_Code"
    ON dbo."FgsTenantCompany" ("TenantId", "Code");

DO $EF$
BEGIN
    IF EXISTS (
        SELECT 1
        FROM pg_constraint c
        INNER JOIN pg_class t ON t.oid = c.conrelid
        INNER JOIN pg_namespace n ON n.oid = t.relnamespace
        WHERE n.nspname = 'dbo'
          AND t.relname = 'FgsTenantCompany'
          AND c.conname = 'UX_Company_Tenant_CompanyNumber'
    )
    AND NOT EXISTS (
        SELECT 1
        FROM pg_constraint c
        INNER JOIN pg_class t ON t.oid = c.conrelid
        INNER JOIN pg_namespace n ON n.oid = t.relnamespace
        WHERE n.nspname = 'dbo'
          AND t.relname = 'FgsTenantCompany'
          AND c.conname = 'AK_FgsTenantCompany_TenantId_CompanyNumber'
    ) THEN
        ALTER TABLE dbo."FgsTenantCompany"
            RENAME CONSTRAINT "UX_Company_Tenant_CompanyNumber"
            TO "AK_FgsTenantCompany_TenantId_CompanyNumber";
    END IF;
END $EF$;

CREATE UNIQUE INDEX IF NOT EXISTS "IX_FgsTenantCompany_TenantId_CompanyNumber"
    ON dbo."FgsTenantCompany" ("TenantId", "CompanyNumber");

ALTER TABLE dbo."GloBusinessType"
    DROP CONSTRAINT IF EXISTS "UX_BusinessType_Code";

CREATE UNIQUE INDEX IF NOT EXISTS "IX_GloBusinessType_Code"
    ON dbo."GloBusinessType" ("Code");

ALTER TABLE dbo."GloAccountingIntegrationType"
    DROP CONSTRAINT IF EXISTS "UX_AccountingIntegrationType_Code";

CREATE UNIQUE INDEX IF NOT EXISTS "IX_GloAccountingIntegrationType_Code"
    ON dbo."GloAccountingIntegrationType" ("Code");

COMMIT;
