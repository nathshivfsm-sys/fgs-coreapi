-- =============================================================================
-- Migration: 20260517120000_NotificationTemplates
-- Script:   20260517120000_NotificationTemplates_Down.sql
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260517120000_NotificationTemplates') THEN
    DROP TABLE IF EXISTS dbo."FgsSetupCommunicationTemplate";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260517120000_NotificationTemplates') THEN
    DELETE FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260517120000_NotificationTemplates';
    END IF;
END $EF$;

COMMIT;
