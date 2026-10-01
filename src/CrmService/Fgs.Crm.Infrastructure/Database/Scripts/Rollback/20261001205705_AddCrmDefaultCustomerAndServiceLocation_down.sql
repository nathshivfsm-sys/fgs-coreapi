START TRANSACTION;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001205705_AddCrmDefaultCustomerAndServiceLocation') THEN
    DROP TABLE crm."CrmDefaultCustomer";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001205705_AddCrmDefaultCustomerAndServiceLocation') THEN
    DROP TABLE crm."CrmDefaultServiceLocation";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001205705_AddCrmDefaultCustomerAndServiceLocation') THEN
    DELETE FROM crm."__EFMigrationsHistory"
    WHERE "MigrationId" = '20261001205705_AddCrmDefaultCustomerAndServiceLocation';
    END IF;
END $EF$;
COMMIT;

