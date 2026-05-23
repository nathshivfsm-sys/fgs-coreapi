-- =============================================================================
-- Migration: 20260523083815_AddGloSeedTableMappings
-- Script:   20260523083815_AddGloSeedTableMappings_down.sql
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260523083815_AddGloSeedTableMappings') THEN
    DROP TABLE IF EXISTS dbo."GloSeedTableColumnMapping";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260523083815_AddGloSeedTableMappings') THEN
    DROP TABLE IF EXISTS dbo."GloSeedTableMapping";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260523083815_AddGloSeedTableMappings') THEN
    DELETE FROM dbo."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260523083815_AddGloSeedTableMappings';
    END IF;
END $EF$;

COMMIT;
