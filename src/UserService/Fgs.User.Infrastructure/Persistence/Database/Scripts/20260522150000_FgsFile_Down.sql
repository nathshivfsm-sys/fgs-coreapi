-- =============================================================================
-- Migration: 20260522150000_FgsFile (Down)
-- Drops FgsFile and related indexes.
-- Pair with: Database/Migrations/20260522150000_FgsFile.cs
-- =============================================================================

START TRANSACTION;

DROP TABLE IF EXISTS dbo."FgsFile";

DELETE FROM dbo."__EFMigrationsHistory"
WHERE "MigrationId" = '20260522150000_FgsFile';

COMMIT;
