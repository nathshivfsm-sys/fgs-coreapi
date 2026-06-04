-- =============================================================================
-- Seed: Glo credential provider and resolution type cache tables (Setup Service)
-- Run manually after migrations and Initial_Migration_Seed.sql (glo reference data).
-- Not part of EF migration Up / Down scripts.
--
-- Idempotent: each insert skips rows that already exist (matched by natural key).
-- =============================================================================

START TRANSACTION;

INSERT INTO setup."GloCredentialProviderTypeCache"
(
    "ProviderTypeId",
    "ProviderCode",
    "ProviderName",
    "ConfigurationSchema",
    "IsActive",
    "UpdatedOn"
)
SELECT
    src."Id",
    src."ProviderCode",
    src."ProviderName",
    src."ConfigurationSchema",
    src."IsActive",
    timezone('utc', now())
FROM glo."GloCredentialProviderType" src
WHERE NOT EXISTS (
    SELECT 1
    FROM setup."GloCredentialProviderTypeCache" c
    WHERE c."ProviderTypeId" = src."Id");

INSERT INTO setup."GloResolutionTypeCache"
(
    "ResolutionTypeId",
    "ResolutionTypeCode",
    "ResolutionTypeName",
    "IsActive",
    "UpdatedOn"
)
SELECT
    src."Id",
    src."ResolutionTypeCode",
    src."ResolutionTypeName",
    src."IsActive",
    timezone('utc', now())
FROM glo."GloResolutionType" src
WHERE NOT EXISTS (
    SELECT 1
    FROM setup."GloResolutionTypeCache" c
    WHERE c."ResolutionTypeId" = src."Id");

COMMIT;
