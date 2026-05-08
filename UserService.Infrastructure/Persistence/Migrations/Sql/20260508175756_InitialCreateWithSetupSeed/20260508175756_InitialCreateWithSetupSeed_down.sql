-- Reverts 20260508175756_InitialCreateWithSetupSeed: drops application tables then removes the migration history row.

START TRANSACTION;
DROP TABLE fgs."AuthIdentity";

DROP TABLE fgs."FSGSetupAccountingIntegrationType";

DROP TABLE fgs."FSGSetupLanguage";

DROP TABLE fgs."FSGSetupLocationType";

DROP TABLE fgs."FSGSetupMasterEntityType";

DROP TABLE fgs."FSGSetupTimeCardOption";

DROP TABLE fgs."Invite";

DROP TABLE fgs."Users";

DROP TABLE fgs."Company";

DROP TABLE fgs."FSGSetupBusinessType";

DROP TABLE fgs."Tenant";

DELETE FROM fgs.__ef_migrations_history
WHERE "MigrationId" = '20260508175756_InitialCreateWithSetupSeed';

COMMIT;

