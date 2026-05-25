-- =============================================================================
-- Migration: 20260523100840_AddFgsTenantStorageBucketName
-- Script:   20260523100840_AddFgsTenantStorageBucketName_down.sql
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260523100840_AddFgsTenantStorageBucketName') THEN
    ALTER TABLE dbo."FgsTenant" DROP COLUMN IF EXISTS "StorageBucketName";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260523100840_AddFgsTenantStorageBucketName') THEN
    DELETE FROM dbo."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260523100840_AddFgsTenantStorageBucketName';
    END IF;
END $EF$;

COMMIT;
