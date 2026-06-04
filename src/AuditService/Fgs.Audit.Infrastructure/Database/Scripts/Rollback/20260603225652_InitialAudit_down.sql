-- Rollback: 20260603225652_InitialAudit
DROP TABLE IF EXISTS audit."FgsCredentialAudit" CASCADE;
DELETE FROM audit."__EFMigrationsHistory" WHERE "MigrationId" = '20260603225652_InitialAudit';
