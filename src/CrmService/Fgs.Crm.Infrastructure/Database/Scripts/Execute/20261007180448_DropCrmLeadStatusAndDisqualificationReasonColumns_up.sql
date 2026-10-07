START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261007180448_DropCrmLeadStatusAndDisqualificationReasonColumns') THEN
    DROP INDEX IF EXISTS crm."IX_CrmLead_TenantId_CompanyId_DisqualificationReasonId";
    DROP INDEX IF EXISTS crm."IX_CrmLead_TenantId_CompanyId_LeadStatusId";
    ALTER TABLE crm."CrmLead" DROP COLUMN IF EXISTS "DisqualificationReasonId";
    ALTER TABLE crm."CrmLead" DROP COLUMN IF EXISTS "LeadStatusId";
    COMMENT ON COLUMN crm."CrmLead"."LeadSourceId" IS 'Source that generated the lead selected from setup.FgsSource.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261007180448_DropCrmLeadStatusAndDisqualificationReasonColumns') THEN
    INSERT INTO crm."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20261007180448_DropCrmLeadStatusAndDisqualificationReasonColumns', '10.0.8');
    END IF;
END $EF$;

COMMIT;
