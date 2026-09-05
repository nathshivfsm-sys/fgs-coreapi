START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20260831180138_AddPreferredCustomerAndEstimateTermsCondition') THEN
    ALTER TABLE crm."FgsEstimate" ADD "TermsConditionVersionId" bigint;
    COMMENT ON COLUMN crm."FgsEstimate"."TermsConditionVersionId" IS 'Reference to the specific terms and conditions version used by the estimate.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20260831180138_AddPreferredCustomerAndEstimateTermsCondition') THEN
    ALTER TABLE crm."CrmCustomer" ADD "IsPreferredCustomer" boolean NOT NULL DEFAULT FALSE;
    COMMENT ON COLUMN crm."CrmCustomer"."IsPreferredCustomer" IS 'Indicates whether the customer is designated as a preferred customer. TRUE indicates preferred customer status; FALSE indicates standard customer status.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20260831180138_AddPreferredCustomerAndEstimateTermsCondition') THEN
    INSERT INTO crm."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260831180138_AddPreferredCustomerAndEstimateTermsCondition', '10.0.8');
    END IF;
END $EF$;
COMMIT;

