-- =============================================================================
-- Migration: 20260603222551_InitialSchema
-- Script:   InitialSchema_down.sql
-- Schema:   notification
-- =============================================================================

START TRANSACTION;

DELETE FROM notification."__EFMigrationsHistory"
WHERE "MigrationId" = '20260603222551_InitialSchema';

DROP TABLE IF EXISTS notification."FgsProcessedIntegrationEvent";
DROP TABLE IF EXISTS notification."FgsNotificationHistory";

COMMIT;
