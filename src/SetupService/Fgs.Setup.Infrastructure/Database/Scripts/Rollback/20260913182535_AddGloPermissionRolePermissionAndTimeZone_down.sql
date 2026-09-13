-- Rollback for 20260913182535_AddGloPermissionRolePermissionAndTimeZone
DROP TABLE IF EXISTS glo."GloRolePermission";
DROP TABLE IF EXISTS glo."GloTimeZone";
DROP TABLE IF EXISTS glo."GloPermission";
DELETE FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260913182535_AddGloPermissionRolePermissionAndTimeZone';
