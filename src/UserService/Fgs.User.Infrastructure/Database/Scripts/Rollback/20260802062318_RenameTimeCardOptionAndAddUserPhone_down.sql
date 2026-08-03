START TRANSACTION;

ALTER TABLE identity."FgsUser"
    DROP COLUMN IF EXISTS "PhoneNumber";

ALTER TABLE tenant."FgsTenantServiceSetup"
    DROP CONSTRAINT IF EXISTS "CK_FgsTenantServiceSetup_TimeCardOptionId";

UPDATE tenant."FgsTenantServiceSetup"
SET "TimeCardOptionId" = CASE "TimeCardOptionId"
    WHEN 1 THEN 1
    WHEN 2 THEN 3
    WHEN 3 THEN 2
    ELSE 1
END;

ALTER TABLE tenant."FgsTenantServiceSetup"
    ALTER COLUMN "TimeCardOptionId" TYPE integer
    USING "TimeCardOptionId"::integer;

ALTER TABLE tenant."FgsTenantServiceSetup"
    RENAME COLUMN "TimeCardOptionId" TO "GloTimeCardOptionId";

DELETE FROM identity."__EFMigrationsHistory"
WHERE "MigrationId" = '20260802062318_RenameTimeCardOptionAndAddUserPhone';

COMMIT;
