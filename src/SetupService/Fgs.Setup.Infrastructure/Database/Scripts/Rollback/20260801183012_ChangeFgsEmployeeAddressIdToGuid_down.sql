-- Rollback for 20260801183012_ChangeFgsEmployeeAddressIdToGuid
START TRANSACTION;

ALTER TABLE setup."FgsEmployee"
ALTER COLUMN "AddressId" DROP DEFAULT;

ALTER TABLE setup."FgsEmployee"
ALTER COLUMN "AddressId" TYPE bigint
USING NULL;

COMMENT ON COLUMN setup."FgsEmployee"."AddressId"
    IS 'Reference to the employee mailing or home address record. No FK by design.';

DELETE FROM setup."__EFMigrationsHistory"
WHERE "MigrationId" = '20260801183012_ChangeFgsEmployeeAddressIdToGuid';

COMMIT;
