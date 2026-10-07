START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261007180448_DropCrmLeadStatusAndDisqualificationReasonColumns') THEN
    ALTER TABLE crm."CrmLead" ADD "LeadStatusId" bigint NOT NULL DEFAULT 0;
    COMMENT ON COLUMN crm."CrmLead"."LeadStatusId" IS 'Current status of the lead selected from the configured sales pipeline statuses applicable to leads.';
    ALTER TABLE crm."CrmLead" ADD "DisqualificationReasonId" bigint;
    COMMENT ON COLUMN crm."CrmLead"."DisqualificationReasonId" IS 'Reason the lead was disqualified selected from setup.FgsLeadDisqualificationReason.';
    COMMENT ON COLUMN crm."CrmLead"."LeadSourceId" IS 'Source that generated the lead selected from setup.FgsLeadSource.';
    CREATE INDEX "IX_CrmLead_TenantId_CompanyId_LeadStatusId" ON crm."CrmLead" ("TenantId", "CompanyId", "LeadStatusId");
    CREATE INDEX "IX_CrmLead_TenantId_CompanyId_DisqualificationReasonId" ON crm."CrmLead" ("TenantId", "CompanyId", "DisqualificationReasonId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261007180448_DropCrmLeadStatusAndDisqualificationReasonColumns') THEN
    DELETE FROM crm."__EFMigrationsHistory"
    WHERE "MigrationId" = '20261007180448_DropCrmLeadStatusAndDisqualificationReasonColumns';
    END IF;
END $EF$;

COMMIT;
