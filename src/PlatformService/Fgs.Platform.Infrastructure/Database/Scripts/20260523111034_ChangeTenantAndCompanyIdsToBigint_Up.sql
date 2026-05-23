-- =============================================================================
-- Migration: 20260523111034_ChangeTenantAndCompanyIdsToBigint
-- Script:   20260523111034_ChangeTenantAndCompanyIdsToBigint_Up.sql
-- Changes TenantId / CompanyId from uuid to bigint (numeric FGS ids)
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260523111034_ChangeTenantAndCompanyIdsToBigint') THEN
    DROP INDEX IF EXISTS dbo."IX_FgsSetupCommunicationTemplate_TenantId_CompanyId_TemplateType_Code";
    DROP INDEX IF EXISTS dbo."IX_FgsSetupCommunicationTemplate_TenantId_CompanyId";
    ALTER TABLE dbo."FgsSetupCommunicationTemplate" DROP COLUMN IF EXISTS "TenantId";
    ALTER TABLE dbo."FgsSetupCommunicationTemplate" DROP COLUMN IF EXISTS "CompanyId";
    ALTER TABLE dbo."FgsSetupCommunicationTemplate" ADD "TenantId" bigint NULL;
    ALTER TABLE dbo."FgsSetupCommunicationTemplate" ADD "CompanyId" bigint NULL;
    CREATE INDEX "IX_FgsSetupCommunicationTemplate_TenantId_CompanyId"
        ON dbo."FgsSetupCommunicationTemplate" ("TenantId", "CompanyId");
    CREATE UNIQUE INDEX "IX_FgsSetupCommunicationTemplate_TenantId_CompanyId_TemplateType_Code"
        ON dbo."FgsSetupCommunicationTemplate" ("TenantId", "CompanyId", "TemplateType", "Code");
  END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260523111034_ChangeTenantAndCompanyIdsToBigint') THEN
    DROP INDEX IF EXISTS dbo."IX_FgsNotificationHistory_TenantId_CreatedOn";
    ALTER TABLE dbo."FgsNotificationHistory" DROP COLUMN IF EXISTS "TenantId";
    ALTER TABLE dbo."FgsNotificationHistory" ADD "TenantId" bigint NOT NULL DEFAULT 0;
    CREATE INDEX "IX_FgsNotificationHistory_TenantId_CreatedOn"
        ON dbo."FgsNotificationHistory" ("TenantId", "CreatedOn");
  END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260523111034_ChangeTenantAndCompanyIdsToBigint') THEN
    INSERT INTO dbo."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260523111034_ChangeTenantAndCompanyIdsToBigint', '10.0.8');
    END IF;
END $EF$;

COMMIT;
