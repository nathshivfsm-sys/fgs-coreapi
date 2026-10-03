START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001212151_ReplaceServiceLocationTypeIdWithEnum') THEN
    ALTER TABLE crm."CrmServiceLocation" RENAME COLUMN "ServiceLocationTypeId" TO "ServiceLocationType";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001212151_ReplaceServiceLocationTypeIdWithEnum') THEN
    UPDATE crm."CrmServiceLocation"
    SET "ServiceLocationType" = 5
    WHERE "ServiceLocationType" NOT IN (1, 2, 3, 4, 5);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001212151_ReplaceServiceLocationTypeIdWithEnum') THEN
    ALTER TABLE crm."CrmServiceLocation" ALTER COLUMN "ServiceLocationType" DROP DEFAULT;
    COMMENT ON COLUMN crm."CrmServiceLocation"."ServiceLocationType" IS 'Specifies the type of the service location. Valid values: 1=Residential, 2=Commercial, 3=Industrial, 4=Government, 5=Other. Corresponds to the ServiceLocationType enum in the application.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001212151_ReplaceServiceLocationTypeIdWithEnum') THEN
    ALTER TABLE crm."CrmServiceLocation" ADD CONSTRAINT "CK_CrmServiceLocation_ServiceLocationType" CHECK ("ServiceLocationType" IN (1, 2, 3, 4, 5));
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001212151_ReplaceServiceLocationTypeIdWithEnum') THEN
    INSERT INTO crm."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20261001212151_ReplaceServiceLocationTypeIdWithEnum', '10.0.8');
    END IF;
END $EF$;
COMMIT;

