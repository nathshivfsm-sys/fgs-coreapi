START TRANSACTION;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001212151_ReplaceServiceLocationTypeIdWithEnum') THEN
    ALTER TABLE crm."CrmServiceLocation" DROP CONSTRAINT "CK_CrmServiceLocation_ServiceLocationType";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001212151_ReplaceServiceLocationTypeIdWithEnum') THEN
    ALTER TABLE crm."CrmServiceLocation" ALTER COLUMN "ServiceLocationType" SET DEFAULT 0;
    COMMENT ON COLUMN crm."CrmServiceLocation"."ServiceLocationType" IS 'Identifier of the service location type.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001212151_ReplaceServiceLocationTypeIdWithEnum') THEN
    ALTER TABLE crm."CrmServiceLocation" RENAME COLUMN "ServiceLocationType" TO "ServiceLocationTypeId";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001212151_ReplaceServiceLocationTypeIdWithEnum') THEN
    DELETE FROM crm."__EFMigrationsHistory"
    WHERE "MigrationId" = '20261001212151_ReplaceServiceLocationTypeIdWithEnum';
    END IF;
END $EF$;
COMMIT;

