START TRANSACTION;

DROP TABLE IF EXISTS glo."GloEstimateFlavor";

DROP TABLE IF EXISTS glo."GloEstimateStatus";

DELETE FROM setup."__EFMigrationsHistory"
WHERE "MigrationId" = '20260616183017_AddGloEstimateEntities';

COMMIT;
