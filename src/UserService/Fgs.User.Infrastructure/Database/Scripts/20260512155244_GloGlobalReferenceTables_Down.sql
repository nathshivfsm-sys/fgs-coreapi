-- =============================================================================
-- Migration: 20260512155244_GloGlobalReferenceTables
-- Script:   20260512155244_GloGlobalReferenceTables_Down.sql
-- Database: PostgreSQL (schema: dbo)
--
-- Rollback: drops Glo* tables created by this migration (child before parent),
--            then removes the history row for MigrationId
--            '20260512155244_GloGlobalReferenceTables'.
--            Apply only to databases that have this migration applied and no
--            dependent FKs from tenant tables onto these catalogs.
-- =============================================================================

START TRANSACTION;

DROP TABLE IF EXISTS dbo."GloStateProvince";

DROP TABLE IF EXISTS dbo."GloCountry";

DROP TABLE IF EXISTS dbo."GloAccountingIntegrationType";

DROP TABLE IF EXISTS dbo."GloBusinessType";

DROP TABLE IF EXISTS dbo."GloCredentialCategory";

DROP TABLE IF EXISTS dbo."GloCredentialProviderType";

DROP TABLE IF EXISTS dbo."GloLanguage";

DROP TABLE IF EXISTS dbo."GloLocationType";

DROP TABLE IF EXISTS dbo."GloResolutionType";

DELETE FROM dbo."__EFMigrationsHistory"
WHERE "MigrationId" = '20260512155244_GloGlobalReferenceTables';

COMMIT;
