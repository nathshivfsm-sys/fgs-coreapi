-- =============================================================================
-- 20260508174113_AddFsgSetupAccountingLanguageMasterEntityLocation (DOWN)
-- Reverses 20260508174113_AddFsgSetupAccountingLanguageMasterEntityLocation.up.sql
-- =============================================================================

BEGIN;

DROP TABLE IF EXISTS fgs."FSGSetupAccountingIntegrationType";

DROP TABLE IF EXISTS fgs."FSGSetupLanguage";

DROP TABLE IF EXISTS fgs."FSGSetupLocationType";

DROP TABLE IF EXISTS fgs."FSGSetupMasterEntityType";

DELETE FROM fgs.__ef_migrations_history
WHERE "MigrationId" = '20260508174113_AddFsgSetupAccountingLanguageMasterEntityLocation';

COMMIT;
