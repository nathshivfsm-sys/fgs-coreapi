-- Rollback for 20260831175228_AddMenuAndTermsConditionEntities
DROP TABLE IF EXISTS setup."FgsEntityDefaultTermsCondition";
DROP TABLE IF EXISTS glo."GloRoleMenu";
DROP TABLE IF EXISTS glo."GloMenu";
DROP TABLE IF EXISTS setup."FgsTermsCondition";
DELETE FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260831175228_AddMenuAndTermsConditionEntities';
