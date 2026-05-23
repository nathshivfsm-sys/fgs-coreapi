-- =============================================================================
-- Migration: 20260523100840_AddFgsTenantStorageBucketName
-- Script:   20260523100840_AddFgsTenantStorageBucketName_up.sql
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260523100840_AddFgsTenantStorageBucketName') THEN
    ALTER TABLE dbo."FgsTenant" ADD "StorageBucketName" character varying(255);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260523100840_AddFgsTenantStorageBucketName') THEN
    INSERT INTO dbo."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260523100840_AddFgsTenantStorageBucketName', '10.0.8');
    END IF;
END $EF$;

COMMIT;
