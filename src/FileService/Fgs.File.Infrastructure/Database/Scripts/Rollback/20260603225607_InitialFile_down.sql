-- Rollback: 20260603225607_InitialFile
DROP TABLE IF EXISTS file."FgsFile" CASCADE;
DELETE FROM file."__EFMigrationsHistory" WHERE "MigrationId" = '20260603225607_InitialFile';
