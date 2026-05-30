-- =============================================================================
-- Migration: 20260530122449_MoveGloTablesToGloSchema
-- Script:   20260530122449_MoveGloTablesToGloSchema_up.sql
-- Path:     Persistence/Database/Scripts/Execute
-- Database: PostgreSQL
--
-- Moves all remaining Glo* tables into the glo schema.
-- Idempotent (dotnet ef migrations script --idempotent).
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530122449_MoveGloTablesToGloSchema') THEN
    ALTER TABLE dispatch."GloTimeCardOption" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530122449_MoveGloTablesToGloSchema') THEN
    ALTER TABLE tenant."GloSeedTableMapping" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530122449_MoveGloTablesToGloSchema') THEN
    ALTER TABLE tenant."GloSeedTableColumnMapping" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530122449_MoveGloTablesToGloSchema') THEN
    ALTER TABLE identity."GloRole" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530122449_MoveGloTablesToGloSchema') THEN
    ALTER TABLE shared."GloOutboxMessage" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530122449_MoveGloTablesToGloSchema') THEN
    ALTER TABLE integration."GloCredentialProviderType" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530122449_MoveGloTablesToGloSchema') THEN
    ALTER TABLE integration."GloCredentialCategory" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530122449_MoveGloTablesToGloSchema') THEN
    ALTER TABLE notification."GloCommunicationToken" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530122449_MoveGloTablesToGloSchema') THEN
    ALTER TABLE integration."GloAccountingIntegrationType" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530122449_MoveGloTablesToGloSchema') THEN
    INSERT INTO shared."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260530122449_MoveGloTablesToGloSchema', '10.0.8');
    END IF;
END $EF$;
COMMIT;

