START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260802062516_AddPostalCodeGeoTripChargeDropTimeCardOption') THEN
    DROP TABLE IF EXISTS glo."GloTimeCardOption";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260802062516_AddPostalCodeGeoTripChargeDropTimeCardOption') THEN
    COMMENT ON COLUMN setup."FgsSetupPricingMatrix"."IsLaborTierStructure" IS 'Indicates whether labor pricing in this pricing matrix is based on labor tiers. When false, standard labor pricing rules are applied. When true, labor charges are calculated using the configured labor tier structure.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260802062516_AddPostalCodeGeoTripChargeDropTimeCardOption') THEN
    ALTER TABLE setup."FgsSetupPostalCode" ADD "City" character varying(100) NOT NULL DEFAULT '';
    COMMENT ON COLUMN setup."FgsSetupPostalCode"."City" IS 'Primary city or municipality associated with the postal code.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260802062516_AddPostalCodeGeoTripChargeDropTimeCardOption') THEN
    ALTER TABLE setup."FgsSetupPostalCode" ADD "CountryCode" character varying(2) NOT NULL DEFAULT '';
    COMMENT ON COLUMN setup."FgsSetupPostalCode"."CountryCode" IS 'ISO 3166-1 alpha-2 country code associated with the postal code (for example, US, CA, MX).';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260802062516_AddPostalCodeGeoTripChargeDropTimeCardOption') THEN
    ALTER TABLE setup."FgsSetupPostalCode" ADD "StateProvinceCode" character varying(10) NOT NULL DEFAULT '';
    COMMENT ON COLUMN setup."FgsSetupPostalCode"."StateProvinceCode" IS 'State, province, or territory code associated with the postal code (for example, TX, ON, BC).';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260802062516_AddPostalCodeGeoTripChargeDropTimeCardOption') THEN
    ALTER TABLE setup."FgsSetupPostalCode" ADD "TripChargeAmount" numeric(12,2) NOT NULL DEFAULT 0.0;
    COMMENT ON COLUMN setup."FgsSetupPostalCode"."TripChargeAmount" IS 'Default trip charge applied when providing service to this postal code. Used by dispatching, estimating, and pricing calculations.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260802062516_AddPostalCodeGeoTripChargeDropTimeCardOption') THEN
    INSERT INTO setup."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260802062516_AddPostalCodeGeoTripChargeDropTimeCardOption', '10.0.8');
    END IF;
END $EF$;
COMMIT;

