START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165335_AddEstimateRevisionCreationMode') THEN
    ALTER TABLE tenant."FgsTenantServiceSetup"
        DROP CONSTRAINT IF EXISTS "CK_FgsTenantServiceSetup_EstimateRevisionCreationMode";

    ALTER TABLE tenant."FgsTenantServiceSetup"
        DROP COLUMN IF EXISTS "EstimateRevisionCreationMode";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165335_AddEstimateRevisionCreationMode') THEN
    DELETE FROM identity."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260817165335_AddEstimateRevisionCreationMode';
    END IF;
END $EF$;
COMMIT;
