-- =============================================================================
-- 20260508163550_UpdateTenantCompanyAndAuditable (DOWN) — PostgreSQL / fgs
-- Reverts the UP script (restores tenant / tenant_company layout from InitialCreate).
-- Mirrors UserService.Infrastructure/Persistence/Migrations/
--   20260508163550_UpdateTenantCompanyAndAuditable.cs Down().
--
-- WARNING: Destructive for Company-only columns and bigint company_id semantics.
--          Review users/invite.company_id vs composite (tenant_id, company_id) if
--          you had multiple companies per tenant.
--
-- Note: EF's raw reverse script incorrectly re-added tenant.created_at after renaming
--       created_on → created_at; this file omits that duplicate (status only is added).
-- =============================================================================

BEGIN;

ALTER TABLE fgs."Company" DROP CONSTRAINT "FK_Company_Tenant_tenant_id";

ALTER TABLE fgs.invite DROP CONSTRAINT "FK_invite_Company_company_id";

ALTER TABLE fgs.invite DROP CONSTRAINT "FK_invite_Tenant_tenant_id";

ALTER TABLE fgs.users DROP CONSTRAINT "FK_users_Company_company_id";

ALTER TABLE fgs.users DROP CONSTRAINT "FK_users_Tenant_tenant_id";

DROP INDEX fgs."IX_users_company_id";

ALTER TABLE fgs."Tenant" DROP CONSTRAINT "PK_Tenant";

DROP INDEX fgs."IX_invite_company_id";

ALTER TABLE fgs."Company" DROP CONSTRAINT "PK_Company";

DROP INDEX fgs."IX_Company_company_guid";

ALTER TABLE fgs."Tenant" DROP COLUMN created_by;

ALTER TABLE fgs."Tenant" DROP COLUMN default_currency;

ALTER TABLE fgs."Tenant" DROP COLUMN default_language_id;

ALTER TABLE fgs."Tenant" DROP COLUMN email;

ALTER TABLE fgs."Tenant" DROP COLUMN is_active;

ALTER TABLE fgs."Tenant" DROP COLUMN legal_name;

ALTER TABLE fgs."Tenant" DROP COLUMN phone_number;

ALTER TABLE fgs."Tenant" DROP COLUMN primary_location_id;

ALTER TABLE fgs."Tenant" DROP COLUMN subscription_plan_id;

ALTER TABLE fgs."Tenant" DROP COLUMN time_zone;

ALTER TABLE fgs."Tenant" DROP COLUMN updated_by;

ALTER TABLE fgs."Tenant" DROP COLUMN website;

ALTER TABLE fgs."Company" DROP COLUMN id;

ALTER TABLE fgs."Company" DROP COLUMN business_type_id;

ALTER TABLE fgs."Company" DROP COLUMN code;

ALTER TABLE fgs."Company" DROP COLUMN compact_logo_url;

ALTER TABLE fgs."Company" DROP COLUMN company_guid;

ALTER TABLE fgs."Company" DROP COLUMN company_number;

ALTER TABLE fgs."Company" DROP COLUMN created_by;

ALTER TABLE fgs."Company" DROP COLUMN email;

ALTER TABLE fgs."Company" DROP COLUMN favicon_url;

ALTER TABLE fgs."Company" DROP COLUMN full_logo_url;

ALTER TABLE fgs."Company" DROP COLUMN icon_logo_url;

ALTER TABLE fgs."Company" DROP COLUMN is_active;

ALTER TABLE fgs."Company" DROP COLUMN legal_name;

ALTER TABLE fgs."Company" DROP COLUMN phone_number;

ALTER TABLE fgs."Company" DROP COLUMN primary_location_id;

ALTER TABLE fgs."Company" DROP COLUMN tax_id;

ALTER TABLE fgs."Company" DROP COLUMN updated_by;

ALTER TABLE fgs."Company" DROP COLUMN website;

ALTER TABLE fgs."Tenant" RENAME TO tenant;

ALTER TABLE fgs."Company" RENAME TO tenant_company;

ALTER TABLE fgs.tenant RENAME COLUMN created_on TO created_at;

ALTER TABLE fgs.tenant RENAME COLUMN updated_on TO updated_at;

ALTER TABLE fgs.tenant_company RENAME COLUMN created_on TO created_at;

ALTER TABLE fgs.tenant_company RENAME COLUMN updated_on TO updated_at;

ALTER INDEX fgs.ix_company_tenant RENAME TO ix_tenant_company_tenant;

ALTER TABLE fgs.users ALTER COLUMN company_id TYPE smallint;

ALTER TABLE fgs.tenant ALTER COLUMN name TYPE text;

ALTER TABLE fgs.tenant ADD status text NOT NULL DEFAULT '';

ALTER TABLE fgs.invite ALTER COLUMN company_id TYPE smallint;

ALTER TABLE fgs.tenant_company ALTER COLUMN name TYPE text;

ALTER TABLE fgs.tenant_company ADD company_id smallint NOT NULL DEFAULT 0;

ALTER TABLE fgs.tenant ADD CONSTRAINT "PK_tenant" PRIMARY KEY (id);

ALTER TABLE fgs.tenant_company ADD CONSTRAINT "PK_tenant_company" PRIMARY KEY (tenant_id, company_id);

CREATE INDEX "IX_users_tenant_id_company_id" ON fgs.users (tenant_id, company_id);

CREATE INDEX "IX_invite_tenant_id_company_id" ON fgs.invite (tenant_id, company_id);

ALTER TABLE fgs.invite ADD CONSTRAINT "FK_invite_tenant_company_tenant_id_company_id" FOREIGN KEY (tenant_id, company_id) REFERENCES fgs.tenant_company (tenant_id, company_id) ON DELETE RESTRICT;

ALTER TABLE fgs.invite ADD CONSTRAINT "FK_invite_tenant_tenant_id" FOREIGN KEY (tenant_id) REFERENCES fgs.tenant (id) ON DELETE RESTRICT;

ALTER TABLE fgs.tenant_company ADD CONSTRAINT "FK_tenant_company_tenant_tenant_id" FOREIGN KEY (tenant_id) REFERENCES fgs.tenant (id) ON DELETE RESTRICT;

ALTER TABLE fgs.users ADD CONSTRAINT "FK_users_tenant_company_tenant_id_company_id" FOREIGN KEY (tenant_id, company_id) REFERENCES fgs.tenant_company (tenant_id, company_id) ON DELETE RESTRICT;

ALTER TABLE fgs.users ADD CONSTRAINT "FK_users_tenant_tenant_id" FOREIGN KEY (tenant_id) REFERENCES fgs.tenant (id) ON DELETE RESTRICT;

DELETE FROM fgs.__ef_migrations_history
WHERE "MigrationId" = '20260508163550_UpdateTenantCompanyAndAuditable';

COMMIT;
