DO $EF$ BEGIN IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'svc') THEN CREATE SCHEMA svc; END IF; END $EF$;

COMMENT ON SCHEMA svc IS $comment$Service Agreement Domain

Stores recurring maintenance agreements, membership plans, preventive maintenance contracts and service contracts.

Responsible for:
- Service Agreements
- Covered Assets
- Visit Scheduling
- Billing Scheduling
- Renewals

Typical lifecycle:

Lead
-> Opportunity
-> Service Agreement
-> Scheduled Visits
-> Work Orders
-> Billing
-> Renewal

CRM owns the sales process.
SVC owns the contract lifecycle after the sale$comment$;

CREATE TABLE IF NOT EXISTS svc."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM svc."__EFMigrationsHistory" WHERE "MigrationId" = '20260611152606_InitialSchema') THEN
    INSERT INTO svc."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260611152606_InitialSchema', '10.0.8');
    END IF;
END $EF$;
COMMIT;
