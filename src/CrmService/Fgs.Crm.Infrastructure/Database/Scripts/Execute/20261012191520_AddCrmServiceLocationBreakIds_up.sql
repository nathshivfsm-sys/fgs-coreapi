START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261012191520_AddCrmServiceLocationBreakIds') THEN
    ALTER TABLE crm."CrmServiceLocation" ADD "Break1Id" bigint;
    COMMENT ON COLUMN crm."CrmServiceLocation"."Break1Id" IS 'First accounting segment used for GL exports and reporting.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261012191520_AddCrmServiceLocationBreakIds') THEN
    ALTER TABLE crm."CrmServiceLocation" ADD "Break2Id" bigint;
    COMMENT ON COLUMN crm."CrmServiceLocation"."Break2Id" IS 'Second accounting segment used for GL exports and reporting.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261012191520_AddCrmServiceLocationBreakIds') THEN
    CREATE INDEX "IX_CrmServiceLocation_Break1Id" ON crm."CrmServiceLocation" ("TenantId", "CompanyId", "Break1Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261012191520_AddCrmServiceLocationBreakIds') THEN
    CREATE INDEX "IX_CrmServiceLocation_Break2Id" ON crm."CrmServiceLocation" ("TenantId", "CompanyId", "Break2Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261012191520_AddCrmServiceLocationBreakIds') THEN
    INSERT INTO crm."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20261012191520_AddCrmServiceLocationBreakIds', '10.0.8');
    END IF;
END $EF$;
COMMIT;

