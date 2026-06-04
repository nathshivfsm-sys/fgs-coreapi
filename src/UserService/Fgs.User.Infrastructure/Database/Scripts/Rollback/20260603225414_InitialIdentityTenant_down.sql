-- Rollback: 20260603225414_InitialIdentityTenant
DROP TABLE IF EXISTS identity."FgsUserRole" CASCADE;
DROP TABLE IF EXISTS identity."FgsInvitation" CASCADE;
DROP TABLE IF EXISTS identity."FgsUser" CASCADE;
DROP TABLE IF EXISTS tenant."FgsTenantServiceSetup" CASCADE;
DROP TABLE IF EXISTS tenant."FgsTenantCompany" CASCADE;
DROP TABLE IF EXISTS tenant."FgsTenant" CASCADE;
DROP TABLE IF EXISTS identity."FgsRole" CASCADE;
DROP TABLE IF EXISTS tenant."FgsLocation" CASCADE;
DELETE FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260603225414_InitialIdentityTenant';
