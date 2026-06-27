START TRANSACTION;

DROP TABLE IF EXISTS glo."GloAppointmentAssignmentEventType";

DELETE FROM setup."__EFMigrationsHistory"
WHERE "MigrationId" = '20260614162342_AddGloAppointmentAssignmentEventType';

COMMIT;
