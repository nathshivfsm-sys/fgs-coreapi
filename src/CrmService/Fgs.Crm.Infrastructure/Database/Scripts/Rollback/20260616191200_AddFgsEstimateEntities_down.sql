START TRANSACTION;

DROP TABLE IF EXISTS crm."FgsEstimateTemplateOptionLine";

DROP TABLE IF EXISTS crm."FgsEstimateTemplateOption";

DROP TABLE IF EXISTS crm."FgsEstimateTemplate";

DROP TABLE IF EXISTS crm."FgsEstimateOptionLine";

DROP TABLE IF EXISTS crm."FgsEstimateClauseItem";

DROP TABLE IF EXISTS crm."FgsEstimateOption";

DROP TABLE IF EXISTS crm."FgsEstimate";

DROP TABLE IF EXISTS crm."FgsEstimateTemplateCategory";

DROP TABLE IF EXISTS crm."FgsEstimateClause";

DROP TABLE IF EXISTS crm."FgsEstimateFlavor";

DROP TABLE IF EXISTS crm."FgsEstimateStatus";

DELETE FROM crm."__EFMigrationsHistory"
WHERE "MigrationId" = '20260616191200_AddFgsEstimateEntities';

COMMIT;
