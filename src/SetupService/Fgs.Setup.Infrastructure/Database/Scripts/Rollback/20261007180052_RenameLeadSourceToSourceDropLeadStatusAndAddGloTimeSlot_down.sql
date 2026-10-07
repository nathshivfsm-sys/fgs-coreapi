START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261007180052_RenameLeadSourceToSourceDropLeadStatusAndAddGloTimeSlot') THEN
    DROP TABLE IF EXISTS glo."GloTimeSlot";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261007180052_RenameLeadSourceToSourceDropLeadStatusAndAddGloTimeSlot') THEN
    ALTER TABLE glo."GloSource" RENAME TO "GloLeadSource";
    ALTER TABLE glo."GloLeadSource" RENAME CONSTRAINT "PK_GloSource" TO "PK_GloLeadSource";
    ALTER INDEX glo."UX_GloSource_SourceCode" RENAME TO "UX_GloLeadSource_SourceCode";

    ALTER TABLE setup."FgsSource" RENAME TO "FgsLeadSource";
    ALTER TABLE setup."FgsLeadSource" RENAME CONSTRAINT "PK_FgsSource" TO "PK_FgsLeadSource";
    ALTER TABLE setup."FgsLeadSource" RENAME CONSTRAINT "FK_FgsSource_FgsTenantCompanyCache_TenantId_CompanyId" TO "FK_FgsLeadSource_FgsTenantCompanyCache_TenantId_CompanyId";
    ALTER INDEX setup."UX_FgsSource_TenantId_CompanyId_SourceCode" RENAME TO "UX_FgsLeadSource_TenantId_CompanyId_SourceCode";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261007180052_RenameLeadSourceToSourceDropLeadStatusAndAddGloTimeSlot') THEN
    DELETE FROM setup."__EFMigrationsHistory"
    WHERE "MigrationId" = '20261007180052_RenameLeadSourceToSourceDropLeadStatusAndAddGloTimeSlot';
    END IF;
END $EF$;

COMMIT;
