START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165335_AddEstimateRevisionCreationMode') THEN
    ALTER TABLE tenant."FgsTenantServiceSetup" ADD "EstimateRevisionCreationMode" character varying(50) NOT NULL DEFAULT 'OnDemand';
    COMMENT ON COLUMN tenant."FgsTenantServiceSetup"."EstimateRevisionCreationMode" IS 'Controls when estimate revisions are created. Valid values: OnDemand = user manually creates a revision; OnPostSignatureChange = automatically creates a revision when a signed estimate is changed.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165335_AddEstimateRevisionCreationMode') THEN
    ALTER TABLE tenant."FgsTenantServiceSetup" ADD CONSTRAINT "CK_FgsTenantServiceSetup_EstimateRevisionCreationMode" CHECK ("EstimateRevisionCreationMode" IN ('OnDemand', 'OnPostSignatureChange'));
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165335_AddEstimateRevisionCreationMode') THEN
    INSERT INTO identity."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260817165335_AddEstimateRevisionCreationMode', '10.0.8');
    END IF;
END $EF$;
COMMIT;

