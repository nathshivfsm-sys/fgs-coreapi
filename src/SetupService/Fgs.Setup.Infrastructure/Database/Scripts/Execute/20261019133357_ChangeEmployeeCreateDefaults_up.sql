START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261019133357_ChangeEmployeeCreateDefaults') THEN
    ALTER TABLE setup."FgsEmployeeTechnicianProfile" ALTER COLUMN "CanBeScheduled" SET DEFAULT FALSE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261019133357_ChangeEmployeeCreateDefaults') THEN
    COMMENT ON COLUMN setup."FgsEmployee"."LegalMiddleName" IS 'Paycheck middle name printed on the paycheck and used on the timecard Excel export.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261019133357_ChangeEmployeeCreateDefaults') THEN
    ALTER TABLE setup."FgsEmployee" ALTER COLUMN "LegalLastName" DROP NOT NULL;
    COMMENT ON COLUMN setup."FgsEmployee"."LegalLastName" IS 'Paycheck last name printed on the paycheck and used on the timecard Excel export.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261019133357_ChangeEmployeeCreateDefaults') THEN
    ALTER TABLE setup."FgsEmployee" ALTER COLUMN "LegalFirstName" DROP NOT NULL;
    COMMENT ON COLUMN setup."FgsEmployee"."LegalFirstName" IS 'Paycheck first name printed on the paycheck and used on the timecard Excel export.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261019133357_ChangeEmployeeCreateDefaults') THEN
    INSERT INTO setup."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20261019133357_ChangeEmployeeCreateDefaults', '10.0.8');
    END IF;
END $EF$;
COMMIT;

