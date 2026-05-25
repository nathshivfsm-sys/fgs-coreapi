-- =============================================================================
-- Migration: 20260523111034_ChangeTenantAndCompanyIdsToBigint
-- Script:   20260523111034_ChangeTenantAndCompanyIdsToBigint_Down.sql
-- Reverts TenantId / CompanyId from bigint back to uuid
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260523111034_ChangeTenantAndCompanyIdsToBigint') THEN
    DROP INDEX IF EXISTS dbo."IX_FgsSetupCommunicationTemplate_TenantId_CompanyId_TemplateType_Code";
    DROP INDEX IF EXISTS dbo."IX_FgsSetupCommunicationTemplate_TenantId_CompanyId";
    ALTER TABLE dbo."FgsSetupCommunicationTemplate" DROP COLUMN IF EXISTS "TenantId";
    ALTER TABLE dbo."FgsSetupCommunicationTemplate" DROP COLUMN IF EXISTS "CompanyId";
    ALTER TABLE dbo."FgsSetupCommunicationTemplate" ADD "TenantId" uuid NULL;
    ALTER TABLE dbo."FgsSetupCommunicationTemplate" ADD "CompanyId" uuid NULL;
    CREATE INDEX "IX_FgsSetupCommunicationTemplate_TenantId_CompanyId"
        ON dbo."FgsSetupCommunicationTemplate" ("TenantId", "CompanyId");
    CREATE UNIQUE INDEX "IX_FgsSetupCommunicationTemplate_TenantId_CompanyId_TemplateType_Code"
        ON dbo."FgsSetupCommunicationTemplate" ("TenantId", "CompanyId", "TemplateType", "Code");
  END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260523111034_ChangeTenantAndCompanyIdsToBigint') THEN
    DROP INDEX IF EXISTS dbo."IX_FgsNotificationHistory_TenantId_CreatedOn";
    ALTER TABLE dbo."FgsNotificationHistory" DROP COLUMN IF EXISTS "TenantId";
    ALTER TABLE dbo."FgsNotificationHistory" ADD "TenantId" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
    CREATE INDEX "IX_FgsNotificationHistory_TenantId_CreatedOn"
        ON dbo."FgsNotificationHistory" ("TenantId", "CreatedOn");
  END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260523111034_ChangeTenantAndCompanyIdsToBigint') THEN
    DELETE FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260523111034_ChangeTenantAndCompanyIdsToBigint';
    END IF;
END $EF$;

COMMIT;
