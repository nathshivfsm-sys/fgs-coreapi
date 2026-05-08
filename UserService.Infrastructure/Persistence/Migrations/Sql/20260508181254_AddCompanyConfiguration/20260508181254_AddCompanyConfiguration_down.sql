-- Drops fgs."CompanyConfiguration" and removes migration 20260508181254_AddCompanyConfiguration from __ef_migrations_history.

START TRANSACTION;
DROP TABLE fgs."CompanyConfiguration";

DELETE FROM fgs.__ef_migrations_history
WHERE "MigrationId" = '20260508181254_AddCompanyConfiguration';

COMMIT;

