START TRANSACTION;

DROP TABLE IF EXISTS dispatch."FgsPayrollLine";
DROP TABLE IF EXISTS dispatch."FgsPayroll";
DROP TABLE IF EXISTS dispatch."FgsPayrollPayPeriod";
DROP TABLE IF EXISTS dispatch."FgsAppointmentAssignmentEvent";
DROP TABLE IF EXISTS dispatch."FgsAppointmentAssignment";
DROP TABLE IF EXISTS dispatch."FgsAppointment";
DROP TABLE IF EXISTS dispatch."FgsWorkOrderIntegration";
DROP TABLE IF EXISTS dispatch."FgsWorkOrderItem";
DROP TABLE IF EXISTS dispatch."FgsWorkOrderAsset";
DROP TABLE IF EXISTS dispatch."FgsWorkOrder";
DROP TABLE IF EXISTS dispatch."FgsCrewMember";
DROP TABLE IF EXISTS dispatch."FgsCrew";

DELETE FROM dispatch."__EFMigrationsHistory"
WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema';

COMMIT;
