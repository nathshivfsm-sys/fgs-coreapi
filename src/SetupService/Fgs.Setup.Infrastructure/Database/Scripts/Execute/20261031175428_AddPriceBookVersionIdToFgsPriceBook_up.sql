START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261031175428_AddPriceBookVersionIdToFgsPriceBook') THEN
    ALTER TABLE setup."FgsPriceBook" ADD "PriceBookVersionId" bigint;
    COMMENT ON COLUMN setup."FgsPriceBook"."PriceBookVersionId" IS 'Identifier of the Price Book version associated with this price book record. The referenced version contains the version number and effective date range.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261031175428_AddPriceBookVersionIdToFgsPriceBook') THEN
    CREATE INDEX "IX_FgsPriceBook_PriceBookVersionId" ON setup."FgsPriceBook" ("PriceBookVersionId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261031175428_AddPriceBookVersionIdToFgsPriceBook') THEN
    ALTER TABLE setup."FgsPriceBook" ADD CONSTRAINT "FK_FgsPriceBook_FgsSetupPriceBookVersion_PriceBookVersionId" FOREIGN KEY ("PriceBookVersionId") REFERENCES setup."FgsSetupPriceBookVersion" ("Id") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261031175428_AddPriceBookVersionIdToFgsPriceBook') THEN
    INSERT INTO setup."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20261031175428_AddPriceBookVersionIdToFgsPriceBook', '10.0.8');
    END IF;
END $EF$;
COMMIT;

