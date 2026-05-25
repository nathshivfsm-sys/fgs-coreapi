-- =============================================================================
-- Migration: 20260522154248_AddGloTradeSkillLeadSourceZoneCategory
-- Script:   20260522154248_AddGloTradeSkillLeadSourceZoneCategory_down.sql
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260522154248_AddGloTradeSkillLeadSourceZoneCategory') THEN
    DROP TABLE IF EXISTS dbo."FgsLeadSource";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260522154248_AddGloTradeSkillLeadSourceZoneCategory') THEN
    DROP TABLE IF EXISTS dbo."GloCategorySubCategory";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260522154248_AddGloTradeSkillLeadSourceZoneCategory') THEN
    DROP TABLE IF EXISTS dbo."GloSkill";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260522154248_AddGloTradeSkillLeadSourceZoneCategory') THEN
    DROP TABLE IF EXISTS dbo."GloCategory";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260522154248_AddGloTradeSkillLeadSourceZoneCategory') THEN
    DROP TABLE IF EXISTS dbo."GloSubCategory";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260522154248_AddGloTradeSkillLeadSourceZoneCategory') THEN
    DROP TABLE IF EXISTS dbo."GloTrade";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260522154248_AddGloTradeSkillLeadSourceZoneCategory') THEN
    DROP TABLE IF EXISTS dbo."GloZone";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260522154248_AddGloTradeSkillLeadSourceZoneCategory') THEN
    DROP TABLE IF EXISTS dbo."GloLeadSource";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260522154248_AddGloTradeSkillLeadSourceZoneCategory') THEN
    DELETE FROM dbo."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260522154248_AddGloTradeSkillLeadSourceZoneCategory';
    END IF;
END $EF$;

COMMIT;
