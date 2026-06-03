-- =============================================================================
-- Migration: 20260530114834_AddBillingCategoryAllowToPickAndGloCommunicationTokenSource
-- Script:   20260530114834_AddBillingCategoryAllowToPickAndGloCommunicationTokenSource_up.sql
-- Path:     Persistence/Database/Scripts/Execute
-- Database: PostgreSQL (schema: dbo)
--
-- Adds GloBillingCategory ShowToFieldTech/AllowToPick, FgsBillingCategory AllowToPick,
-- GloCommunicationToken source metadata columns, and expands Fgs billing category unique constraint.
-- Idempotent (dotnet ef migrations script --idempotent).
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260530114834_AddBillingCategoryAllowToPickAndGloCommunicationTokenSource') THEN
    ALTER TABLE dbo."FgsBillingCategory" DROP CONSTRAINT "UQ_FgsBillingCategory_TenantId_CompanyId_BillingCategoryType";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260530114834_AddBillingCategoryAllowToPickAndGloCommunicationTokenSource') THEN
    COMMENT ON TABLE dbo."FgsBillingCategory" IS 'Stores tenant/company specific billing categories used for invoicing, service billing, maintenance plans, and other billing operations. Seeded initially from GloBillingCategory but fully managed by each tenant/company independently.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260530114834_AddBillingCategoryAllowToPickAndGloCommunicationTokenSource') THEN
    ALTER TABLE dbo."GloCommunicationToken" ADD "SourceDatabaseName" text NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260530114834_AddBillingCategoryAllowToPickAndGloCommunicationTokenSource') THEN
    ALTER TABLE dbo."GloCommunicationToken" ADD "SourceSchemaName" text NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260530114834_AddBillingCategoryAllowToPickAndGloCommunicationTokenSource') THEN
    ALTER TABLE dbo."GloBillingCategory" ADD "AllowToPick" boolean NOT NULL DEFAULT TRUE;
    COMMENT ON COLUMN dbo."GloBillingCategory"."AllowToPick" IS 'Determines whether office users are allowed to manually select this billing category during estimate, invoice, or billing entry. Categories such as tax may be system controlled and not manually selectable.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260530114834_AddBillingCategoryAllowToPickAndGloCommunicationTokenSource') THEN
    ALTER TABLE dbo."GloBillingCategory" ADD "ShowToFieldTech" boolean NOT NULL DEFAULT TRUE;
    COMMENT ON COLUMN dbo."GloBillingCategory"."ShowToFieldTech" IS 'Determines whether field technicians can view/select this billing category in mobile and field workflows.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260530114834_AddBillingCategoryAllowToPickAndGloCommunicationTokenSource') THEN
    COMMENT ON COLUMN dbo."FgsBillingCategory"."UpdatedOn" IS 'Date and time the billing category record was last updated.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260530114834_AddBillingCategoryAllowToPickAndGloCommunicationTokenSource') THEN
    COMMENT ON COLUMN dbo."FgsBillingCategory"."UpdatedBy" IS 'User identifier that last updated the billing category record.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260530114834_AddBillingCategoryAllowToPickAndGloCommunicationTokenSource') THEN
    COMMENT ON COLUMN dbo."FgsBillingCategory"."TenantId" IS 'Tenant identifier owning this billing category.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260530114834_AddBillingCategoryAllowToPickAndGloCommunicationTokenSource') THEN
    ALTER TABLE dbo."FgsBillingCategory" ALTER COLUMN "ShowToFieldTech" SET DEFAULT FALSE;
    COMMENT ON COLUMN dbo."FgsBillingCategory"."ShowToFieldTech" IS 'Indicates whether the billing category is visible to field technicians in mobile and field service applications.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260530114834_AddBillingCategoryAllowToPickAndGloCommunicationTokenSource') THEN
    COMMENT ON COLUMN dbo."FgsBillingCategory"."IsSystemDefined" IS 'Indicates whether the billing category was system seeded or manually created by the tenant/company.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260530114834_AddBillingCategoryAllowToPickAndGloCommunicationTokenSource') THEN
    COMMENT ON COLUMN dbo."FgsBillingCategory"."IsActive" IS 'Indicates whether the billing category is active and available for use.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260530114834_AddBillingCategoryAllowToPickAndGloCommunicationTokenSource') THEN
    COMMENT ON COLUMN dbo."FgsBillingCategory"."DisplayOrder" IS 'Controls sorting/display order of billing categories in dropdowns and setup screens.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260530114834_AddBillingCategoryAllowToPickAndGloCommunicationTokenSource') THEN
    COMMENT ON COLUMN dbo."FgsBillingCategory"."Description" IS 'Optional internal description or notes for the billing category.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260530114834_AddBillingCategoryAllowToPickAndGloCommunicationTokenSource') THEN
    COMMENT ON COLUMN dbo."FgsBillingCategory"."CreatedOn" IS 'Date and time the billing category record was created.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260530114834_AddBillingCategoryAllowToPickAndGloCommunicationTokenSource') THEN
    COMMENT ON COLUMN dbo."FgsBillingCategory"."CreatedBy" IS 'User identifier that created the billing category record.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260530114834_AddBillingCategoryAllowToPickAndGloCommunicationTokenSource') THEN
    COMMENT ON COLUMN dbo."FgsBillingCategory"."CompanyId" IS 'Company identifier within the tenant owning this billing category.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260530114834_AddBillingCategoryAllowToPickAndGloCommunicationTokenSource') THEN
    COMMENT ON COLUMN dbo."FgsBillingCategory"."BillingCategoryType" IS 'Short billing category code such as IN, PM, SR, or other tenant-defined values.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260530114834_AddBillingCategoryAllowToPickAndGloCommunicationTokenSource') THEN
    COMMENT ON COLUMN dbo."FgsBillingCategory"."BillingCategoryName" IS 'Display name of the billing category shown throughout the application.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260530114834_AddBillingCategoryAllowToPickAndGloCommunicationTokenSource') THEN
    COMMENT ON COLUMN dbo."FgsBillingCategory"."Id" IS 'Primary key identity of the billing category record.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260530114834_AddBillingCategoryAllowToPickAndGloCommunicationTokenSource') THEN
    ALTER TABLE dbo."FgsBillingCategory" ADD "AllowToPick" boolean NOT NULL DEFAULT TRUE;
    COMMENT ON COLUMN dbo."FgsBillingCategory"."AllowToPick" IS 'Determines whether office users are allowed to manually select this billing category during estimate, invoice, or billing entry. Categories such as tax may be system controlled and not manually selectable.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260530114834_AddBillingCategoryAllowToPickAndGloCommunicationTokenSource') THEN
    ALTER TABLE dbo."FgsBillingCategory" ADD CONSTRAINT "UQ_FgsBillingCategory_TenantId_CompanyId_BillingCategoryType" UNIQUE ("TenantId", "CompanyId", "BillingCategoryType", "BillingCategoryName");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260530114834_AddBillingCategoryAllowToPickAndGloCommunicationTokenSource') THEN
    INSERT INTO dbo."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260530114834_AddBillingCategoryAllowToPickAndGloCommunicationTokenSource', '10.0.8');
    END IF;
END $EF$;
COMMIT;

