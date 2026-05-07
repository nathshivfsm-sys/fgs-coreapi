-- =============================================================================
-- Migration: 001_create_fgs_user_service_schema (DOWN)
-- Reverts UP by dropping schema fgs and all objects inside it (CASCADE).
-- WARNING: Destroys all data in schema fgs. Backup before use in non-dev DBs.
-- citext extension is not dropped (may be shared). Uncomment optional block if safe.
-- =============================================================================

BEGIN;

SET LOCAL lock_timeout = '30s';
SET LOCAL statement_timeout = '10min';

DROP SCHEMA IF EXISTS fgs CASCADE;

COMMIT;

-- Optional (dedicated DB only):
-- BEGIN;
-- DROP EXTENSION IF EXISTS citext;
-- COMMIT;
