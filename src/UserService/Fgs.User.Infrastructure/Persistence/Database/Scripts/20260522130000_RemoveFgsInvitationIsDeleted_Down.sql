-- =============================================================================
-- Migration: 20260522130000_RemoveFgsInvitationIsDeleted (rollback)
-- =============================================================================

START TRANSACTION;

ALTER TABLE dbo."FgsInvitation"
    ADD COLUMN IF NOT EXISTS "IsDeleted" boolean NOT NULL DEFAULT false;

DELETE FROM dbo."__EFMigrationsHistory"
WHERE "MigrationId" = '20260522130000_RemoveFgsInvitationIsDeleted';

COMMIT;
