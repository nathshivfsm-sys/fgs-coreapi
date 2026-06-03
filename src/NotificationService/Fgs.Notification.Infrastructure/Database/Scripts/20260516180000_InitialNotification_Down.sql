-- =============================================================================
-- Migration: 20260516180000_InitialPlatform
-- Script:   20260516180000_InitialPlatform_Down.sql
-- Database: fgs_platform (PostgreSQL, schema: dbo)
--
-- Rollback: drops notification/idempotency tables while MigrationId is still
--           recorded. Removes the history row last.
--
-- Notes:
--   - Idempotent DO blocks.
--   - Matches Down() in: 20260516180000_InitialPlatform.cs
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260516180000_InitialPlatform') THEN
    DROP TABLE dbo."FgsProcessedIntegrationEvent";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260516180000_InitialPlatform') THEN
    DROP TABLE dbo."FgsNotificationHistory";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260516180000_InitialPlatform') THEN
    DELETE FROM dbo."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260516180000_InitialPlatform';
    END IF;
END $EF$;

COMMIT;
