-- =============================================================================
-- Migration: 20260515090709_UserIdentityInvitationOutbox
-- Script:   20260515090709_UserIdentityInvitationOutbox_Down.sql
-- Database: PostgreSQL (schema: dbo)
--
-- Rollback: drops identity/outbox tables while MigrationId is still recorded.
--           Removes the history row last.
--
-- Notes:
--   - Idempotent DO blocks.
--   - Matches Down() in: 20260515090709_UserIdentityInvitationOutbox.cs
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515090709_UserIdentityInvitationOutbox') THEN
    DROP TABLE dbo."FgsInvitation";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515090709_UserIdentityInvitationOutbox') THEN
    DROP TABLE dbo."FgsOutboxMessage";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515090709_UserIdentityInvitationOutbox') THEN
    DROP TABLE dbo."FgsUser";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515090709_UserIdentityInvitationOutbox') THEN
    DELETE FROM dbo."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260515090709_UserIdentityInvitationOutbox';
    END IF;
END $EF$;

COMMIT;
