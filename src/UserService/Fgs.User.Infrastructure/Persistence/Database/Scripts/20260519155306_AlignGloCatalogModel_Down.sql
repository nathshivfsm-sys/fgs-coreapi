-- =============================================================================
-- Revert: 20260519155306_AlignGloCatalogModel
-- Removes the EF migration history row only. Catalog constraints and defaults
-- are not dropped here; use 20260519120000_GloCatalogConstraints_Down.sql to
-- revert constraint changes if required.
-- =============================================================================

START TRANSACTION;

DELETE FROM dbo."__EFMigrationsHistory"
WHERE "MigrationId" = '20260519155306_AlignGloCatalogModel';

COMMIT;
