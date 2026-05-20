-- =============================================================================
-- Migration: 20260522130000_RemoveFgsInvitationIsDeleted
-- Drops IsDeleted from FgsInvitation (invitations use Status, not soft-delete).
-- Pair with: Database/Migrations/20260522130000_RemoveFgsInvitationIsDeleted.cs
-- =============================================================================

START TRANSACTION;

ALTER TABLE dbo."FgsInvitation"
    DROP COLUMN IF EXISTS "IsDeleted";

INSERT INTO dbo."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260522130000_RemoveFgsInvitationIsDeleted', '10.0.8')
ON CONFLICT ("MigrationId") DO NOTHING;

COMMIT;
