START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260905055631_AddFgsTenantMenuCatalogColumns') THEN
    ALTER TABLE identity."FgsTenantMenu" ADD "Description" character varying(255);
    COMMENT ON COLUMN identity."FgsTenantMenu"."Description" IS 'Description of the menu item and its purpose.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260905055631_AddFgsTenantMenuCatalogColumns') THEN
    ALTER TABLE identity."FgsTenantMenu" ADD "Icon" character varying(100);
    COMMENT ON COLUMN identity."FgsTenantMenu"."Icon" IS 'UI icon identifier associated with the menu item.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260905055631_AddFgsTenantMenuCatalogColumns') THEN
    ALTER TABLE identity."FgsTenantMenu" ADD "MenuCode" character varying(50);
    COMMENT ON COLUMN identity."FgsTenantMenu"."MenuCode" IS 'Unique system-defined code identifying the menu item (copied from global catalog).';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260905055631_AddFgsTenantMenuCatalogColumns') THEN
    ALTER TABLE identity."FgsTenantMenu" ADD "MenuType" character varying(20);
    COMMENT ON COLUMN identity."FgsTenantMenu"."MenuType" IS 'Defines the type of menu item, such as a menu group or navigable page.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260905055631_AddFgsTenantMenuCatalogColumns') THEN
    ALTER TABLE identity."FgsTenantMenu" ADD "Name" character varying(100);
    COMMENT ON COLUMN identity."FgsTenantMenu"."Name" IS 'Display name of the menu item shown to users.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260905055631_AddFgsTenantMenuCatalogColumns') THEN
    ALTER TABLE identity."FgsTenantMenu" ADD "ParentMenuId" integer;
    COMMENT ON COLUMN identity."FgsTenantMenu"."ParentMenuId" IS 'Global parent menu id when this item is nested; NULL for top-level menus.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260905055631_AddFgsTenantMenuCatalogColumns') THEN
    ALTER TABLE identity."FgsTenantMenu" ADD "Route" character varying(255);
    COMMENT ON COLUMN identity."FgsTenantMenu"."Route" IS 'Application route used to navigate to the menu item when applicable.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260905055631_AddFgsTenantMenuCatalogColumns') THEN
    UPDATE identity."FgsTenantMenu"
    SET
        "MenuCode" = COALESCE(NULLIF(trim("MenuCode"), ''), 'MENU_' || "MenuId"::text),
        "Name" = COALESCE(NULLIF(trim("Name"), ''), 'Menu ' || "MenuId"::text),
        "MenuType" = COALESCE(NULLIF(trim("MenuType"), ''), 'PAGE')
    WHERE "MenuCode" IS NULL
       OR trim("MenuCode") = ''
       OR "Name" IS NULL
       OR trim("Name") = ''
       OR "MenuType" IS NULL
       OR trim("MenuType") = '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260905055631_AddFgsTenantMenuCatalogColumns') THEN
    ALTER TABLE identity."FgsTenantMenu" ALTER COLUMN "MenuCode" SET NOT NULL;
    ALTER TABLE identity."FgsTenantMenu" ALTER COLUMN "Name" SET NOT NULL;
    ALTER TABLE identity."FgsTenantMenu" ALTER COLUMN "MenuType" SET NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260905055631_AddFgsTenantMenuCatalogColumns') THEN
    CREATE UNIQUE INDEX "IX_FgsTenantMenu_TenantId_CompanyId_MenuCode" ON identity."FgsTenantMenu" ("TenantId", "CompanyId", "MenuCode");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260905055631_AddFgsTenantMenuCatalogColumns') THEN
    ALTER TABLE identity."FgsTenantMenu" ADD CONSTRAINT "CK_FgsTenantMenu_MenuCode_NotEmpty" CHECK (length(trim("MenuCode")) > 0);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260905055631_AddFgsTenantMenuCatalogColumns') THEN
    ALTER TABLE identity."FgsTenantMenu" ADD CONSTRAINT "CK_FgsTenantMenu_MenuType_NotEmpty" CHECK (length(trim("MenuType")) > 0);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260905055631_AddFgsTenantMenuCatalogColumns') THEN
    ALTER TABLE identity."FgsTenantMenu" ADD CONSTRAINT "CK_FgsTenantMenu_Name_NotEmpty" CHECK (length(trim("Name")) > 0);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260905055631_AddFgsTenantMenuCatalogColumns') THEN
    INSERT INTO identity."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260905055631_AddFgsTenantMenuCatalogColumns', '10.0.8');
    END IF;
END $EF$;
COMMIT;

