-- =============================================================================
-- Migration: 20260530122449_MoveGloTablesToGloSchema
-- Script:   20260530122449_MoveGloTablesToGloSchema_down.sql
-- Path:     Persistence/Database/Scripts/Rollback
-- Database: PostgreSQL
--
-- Reverts Glo* tables to their prior domain schemas (pre-consolidation).
-- =============================================================================

START TRANSACTION;

DELETE FROM shared."__EFMigrationsHistory"
WHERE "MigrationId" = '20260530122449_MoveGloTablesToGloSchema';

ALTER TABLE IF EXISTS glo."GloTimeCardOption" SET SCHEMA dispatch;
ALTER TABLE IF EXISTS glo."GloSeedTableMapping" SET SCHEMA tenant;
ALTER TABLE IF EXISTS glo."GloSeedTableColumnMapping" SET SCHEMA tenant;
ALTER TABLE IF EXISTS glo."GloRole" SET SCHEMA identity;
ALTER TABLE IF EXISTS glo."GloOutboxMessage" SET SCHEMA shared;
ALTER TABLE IF EXISTS glo."GloCredentialProviderType" SET SCHEMA integration;
ALTER TABLE IF EXISTS glo."GloCredentialCategory" SET SCHEMA integration;
ALTER TABLE IF EXISTS glo."GloCommunicationToken" SET SCHEMA notification;
ALTER TABLE IF EXISTS glo."GloAccountingIntegrationType" SET SCHEMA integration;

COMMIT;
