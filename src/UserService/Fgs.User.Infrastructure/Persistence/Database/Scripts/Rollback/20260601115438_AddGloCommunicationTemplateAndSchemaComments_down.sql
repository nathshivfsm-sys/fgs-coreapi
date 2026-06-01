-- =============================================================================
-- Migration: 20260601115438_AddGloCommunicationTemplateAndSchemaComments
-- Script:   20260601115438_AddGloCommunicationTemplateAndSchemaComments_down.sql
-- Path:     Persistence/Database/Scripts/Rollback
-- Database: PostgreSQL
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260601115438_AddGloCommunicationTemplateAndSchemaComments') THEN
    DROP TABLE IF EXISTS glo."GloCommunicationTemplateToken";
    DROP TABLE IF EXISTS glo."GloCommunicationTemplate";
    END IF;
END $EF$;

COMMIT;
