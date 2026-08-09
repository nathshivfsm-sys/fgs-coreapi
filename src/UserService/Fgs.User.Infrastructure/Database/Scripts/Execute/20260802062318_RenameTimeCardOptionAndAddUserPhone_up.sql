START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260802062318_RenameTimeCardOptionAndAddUserPhone') THEN
        ALTER TABLE tenant."FgsTenantServiceSetup"
            RENAME COLUMN "GloTimeCardOptionId" TO "TimeCardOptionId";

        ALTER TABLE tenant."FgsTenantServiceSetup"
            ALTER COLUMN "TimeCardOptionId" TYPE smallint
            USING "TimeCardOptionId"::smallint;

        -- Remap legacy GloTimeCardOption seed ids to new enum semantics:
        -- 1=NONE→1, 2=DISPATCHARRIVECOMPLETE→3, 3=CHECKINCHECKOUT→2
        UPDATE tenant."FgsTenantServiceSetup"
        SET "TimeCardOptionId" = CASE "TimeCardOptionId"
            WHEN 1 THEN 1
            WHEN 2 THEN 3
            WHEN 3 THEN 2
            ELSE 1
        END;

        ALTER TABLE tenant."FgsTenantServiceSetup"
            DROP CONSTRAINT IF EXISTS "CK_FgsTenantServiceSetup_TimeCardOptionId";

        ALTER TABLE tenant."FgsTenantServiceSetup"
            ADD CONSTRAINT "CK_FgsTenantServiceSetup_TimeCardOptionId"
            CHECK ("TimeCardOptionId" IN (1, 2, 3, 4));

        COMMENT ON COLUMN tenant."FgsTenantServiceSetup"."TimeCardOptionId"
            IS 'Determines the technician time tracking workflow. Valid values: 1 = No formal technician time tracking workflow, 2 = Technician manually checks in and checks out, 3 = Tracks dispatch, arrival, and completion timestamps, 4 = Tracks dispatch, arrival, completion, and documentation time timestamps.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260802062318_RenameTimeCardOptionAndAddUserPhone') THEN
        ALTER TABLE identity."FgsUser"
            ADD COLUMN IF NOT EXISTS "PhoneNumber" character varying(20) NULL;

        COMMENT ON COLUMN identity."FgsUser"."PhoneNumber"
            IS 'Primary phone number used for SMS notifications and one-time password (OTP) verification when multi-factor authentication (MFA) using SMS is enabled.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260802062318_RenameTimeCardOptionAndAddUserPhone') THEN
        INSERT INTO identity."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
        VALUES ('20260802062318_RenameTimeCardOptionAndAddUserPhone', '10.0.8');
    END IF;
END $EF$;

COMMIT;
