-- =============================================================================
-- 20260508172853_AddFsgSetupBusinessTypeAndTimeCardOption (DOWN) — PostgreSQL / fgs
-- Reverses 20260508172853_AddFsgSetupBusinessTypeAndTimeCardOption.up.sql
-- =============================================================================

BEGIN;

ALTER TABLE fgs."Company" DROP CONSTRAINT IF EXISTS "FK_Company_FSGSetupBusinessType_business_type_id";

DROP TABLE IF EXISTS fgs."FSGSetupTimeCardOption";

DROP TABLE IF EXISTS fgs."FSGSetupBusinessType";

DROP INDEX IF EXISTS fgs."IX_Company_business_type_id";

DELETE FROM fgs.__ef_migrations_history
WHERE "MigrationId" = '20260508172853_AddFsgSetupBusinessTypeAndTimeCardOption';

COMMIT;
