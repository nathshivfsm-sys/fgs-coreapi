-- Rollback for 20260801184247_AddEmployeeMasterEntityType
START TRANSACTION;

DELETE FROM glo."GloMasterEntityType"
WHERE "Code" = 'EMPLOYEE';

DELETE FROM setup."__EFMigrationsHistory"
WHERE "MigrationId" = '20260801184247_AddEmployeeMasterEntityType';

COMMIT;
