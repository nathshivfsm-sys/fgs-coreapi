-- =============================================================================
-- Migration: 20260523182019_AddGloUnitOfMeasureGloTagFgsTagGloTitleOfCourtesy
-- Script:   20260523182019_AddGloUnitOfMeasureGloTagFgsTagGloTitleOfCourtesy_down.sql
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260523182019_AddGloUnitOfMeasureGloTagFgsTagGloTitleOfCourtesy') THEN
    DROP TABLE IF EXISTS dbo."FgsTag";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260523182019_AddGloUnitOfMeasureGloTagFgsTagGloTitleOfCourtesy') THEN
    DROP TABLE IF EXISTS dbo."GloTag";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260523182019_AddGloUnitOfMeasureGloTagFgsTagGloTitleOfCourtesy') THEN
    DROP TABLE IF EXISTS dbo."GloTitleOfCourtesy";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260523182019_AddGloUnitOfMeasureGloTagFgsTagGloTitleOfCourtesy') THEN
    DROP TABLE IF EXISTS dbo."GloUnitOfMeasure";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260523182019_AddGloUnitOfMeasureGloTagFgsTagGloTitleOfCourtesy') THEN
    DELETE FROM dbo."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260523182019_AddGloUnitOfMeasureGloTagFgsTagGloTitleOfCourtesy';
    END IF;
END $EF$;

COMMIT;
