START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261019133357_ChangeEmployeeCreateDefaults') THEN
    ALTER TABLE IF EXISTS setup."FgsEmployeeTechnicianProfile"
        ALTER COLUMN "CanBeScheduled" SET DEFAULT TRUE;

    UPDATE setup."FgsEmployee"
    SET "LegalFirstName" = ''
    WHERE "LegalFirstName" IS NULL;

    ALTER TABLE IF EXISTS setup."FgsEmployee"
        ALTER COLUMN "LegalFirstName" SET NOT NULL;
    COMMENT ON COLUMN setup."FgsEmployee"."LegalFirstName" IS 'Employee legal first name.';

    UPDATE setup."FgsEmployee"
    SET "LegalLastName" = ''
    WHERE "LegalLastName" IS NULL;

    ALTER TABLE IF EXISTS setup."FgsEmployee"
        ALTER COLUMN "LegalLastName" SET NOT NULL;
    COMMENT ON COLUMN setup."FgsEmployee"."LegalLastName" IS 'Employee legal last name.';

    COMMENT ON COLUMN setup."FgsEmployee"."LegalMiddleName" IS 'Employee legal middle name.';

    DELETE FROM setup."__EFMigrationsHistory"
    WHERE "MigrationId" = '20261019133357_ChangeEmployeeCreateDefaults';
    END IF;
END $EF$;
COMMIT;
