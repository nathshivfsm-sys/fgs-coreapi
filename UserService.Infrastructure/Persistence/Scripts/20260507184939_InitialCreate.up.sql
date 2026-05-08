-- =============================================================================
-- Migration: 001_create_fgs_user_service_schema (UP)
-- Description: Baseline PostgreSQL schema for UserService (UserServiceDbContext).
--              Synchronized with `dotnet ef dbcontext script` (EF Core 10 / Npgsql).
-- Prerequisites: PostgreSQL 14+ recommended; role with CREATE on database.
-- Idempotency:   Extension + schema use IF NOT EXISTS; tables/indexes are plain
--                 CREATE (run once per environment, or DOWN first).
-- =============================================================================

-- -----------------------------------------------------------------------------
-- Extensions (outside main DDL transaction where hosts restrict extension installs)
-- -----------------------------------------------------------------------------
CREATE EXTENSION IF NOT EXISTS citext;

-- -----------------------------------------------------------------------------
-- Schema
-- -----------------------------------------------------------------------------
DO $schema$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_namespace WHERE nspname = 'fgs') THEN
        CREATE SCHEMA fgs;
    END IF;
END $schema$;

COMMENT ON SCHEMA fgs IS 'UserService: tenants, tenant_company, users, invites, auth identities.';

-- -----------------------------------------------------------------------------
-- Tables & indexes (transactional all-or-nothing)
-- -----------------------------------------------------------------------------
BEGIN;

SET LOCAL lock_timeout = '30s';
SET LOCAL statement_timeout = '10min';

CREATE TABLE fgs.tenant (
    id uuid NOT NULL,
    name text NOT NULL,
    status text NOT NULL,
    created_at timestamp with time zone NOT NULL,
    updated_at timestamp with time zone NOT NULL,
    CONSTRAINT "PK_tenant" PRIMARY KEY (id)
);

CREATE TABLE fgs.tenant_company (
    tenant_id uuid NOT NULL,
    company_id smallint NOT NULL,
    name text NOT NULL,
    created_at timestamp with time zone NOT NULL,
    updated_at timestamp with time zone NOT NULL,
    CONSTRAINT "PK_tenant_company" PRIMARY KEY (tenant_id, company_id),
    CONSTRAINT "FK_tenant_company_tenant_tenant_id" FOREIGN KEY (tenant_id) REFERENCES fgs.tenant (id) ON DELETE RESTRICT
);

CREATE TABLE fgs.users (
    id uuid NOT NULL,
    tenant_id uuid NOT NULL,
    email citext NOT NULL,
    display_name text,
    status text NOT NULL,
    company_id smallint NOT NULL,
    role text NOT NULL,
    created_at timestamp with time zone NOT NULL,
    updated_at timestamp with time zone NOT NULL,
    CONSTRAINT "PK_users" PRIMARY KEY (id),
    CONSTRAINT "FK_users_tenant_company_tenant_id_company_id" FOREIGN KEY (tenant_id, company_id) REFERENCES fgs.tenant_company (tenant_id, company_id) ON DELETE RESTRICT,
    CONSTRAINT "FK_users_tenant_tenant_id" FOREIGN KEY (tenant_id) REFERENCES fgs.tenant (id) ON DELETE RESTRICT
);

CREATE TABLE fgs.auth_identity (
    id uuid NOT NULL,
    user_id uuid NOT NULL,
    issuer text NOT NULL,
    object_id text NOT NULL,
    subject text,
    email_snapshot citext,
    linked_at timestamp with time zone NOT NULL,
    CONSTRAINT "PK_auth_identity" PRIMARY KEY (id),
    CONSTRAINT "FK_auth_identity_users_user_id" FOREIGN KEY (user_id) REFERENCES fgs.users (id) ON DELETE CASCADE
);

CREATE TABLE fgs.invite (
    id uuid NOT NULL,
    tenant_id uuid NOT NULL,
    user_id uuid NOT NULL,
    invited_email citext NOT NULL,
    company_id smallint NOT NULL,
    token_hash bytea NOT NULL,
    status text NOT NULL,
    expires_at timestamp with time zone NOT NULL,
    accepted_at timestamp with time zone,
    revoked_at timestamp with time zone,
    created_at timestamp with time zone NOT NULL,
    CONSTRAINT "PK_invite" PRIMARY KEY (id),
    CONSTRAINT "FK_invite_tenant_company_tenant_id_company_id" FOREIGN KEY (tenant_id, company_id) REFERENCES fgs.tenant_company (tenant_id, company_id) ON DELETE RESTRICT,
    CONSTRAINT "FK_invite_tenant_tenant_id" FOREIGN KEY (tenant_id) REFERENCES fgs.tenant (id) ON DELETE RESTRICT,
    CONSTRAINT "FK_invite_users_user_id" FOREIGN KEY (user_id) REFERENCES fgs.users (id) ON DELETE CASCADE
);

CREATE UNIQUE INDEX "IX_auth_identity_issuer_object_id" ON fgs.auth_identity (issuer, object_id);

CREATE INDEX ix_auth_identity_user ON fgs.auth_identity (user_id);

CREATE INDEX ix_invite_pending ON fgs.invite (tenant_id) WHERE status = 'pending';

CREATE INDEX "IX_invite_tenant_id_company_id" ON fgs.invite (tenant_id, company_id);

CREATE INDEX ix_invite_token_hash ON fgs.invite (token_hash);

CREATE INDEX ix_invite_user ON fgs.invite (user_id);

CREATE INDEX ix_tenant_company_tenant ON fgs.tenant_company (tenant_id);

CREATE INDEX ix_users_tenant ON fgs.users (tenant_id);

CREATE INDEX "IX_users_tenant_id_company_id" ON fgs.users (tenant_id, company_id);

CREATE UNIQUE INDEX "IX_users_tenant_id_email" ON fgs.users (tenant_id, email);

CREATE TABLE IF NOT EXISTS fgs.__ef_migrations_history (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___ef_migrations_history" PRIMARY KEY ("MigrationId")
);

INSERT INTO fgs.__ef_migrations_history ("MigrationId", "ProductVersion")
VALUES ('20260507184939_InitialCreate', '10.0.0');

COMMIT;
