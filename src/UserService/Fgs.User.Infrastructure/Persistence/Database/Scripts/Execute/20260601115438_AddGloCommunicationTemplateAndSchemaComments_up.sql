-- =============================================================================
-- Migration: 20260601115438_AddGloCommunicationTemplateAndSchemaComments
-- Script:   20260601115438_AddGloCommunicationTemplateAndSchemaComments_up.sql
-- Path:     Persistence/Database/Scripts/Execute
-- Database: PostgreSQL
--
-- Ensures domain schemas, adds schema comments, creates glo communication
-- template tables, and alters setup.FgsSetupCommunicationTemplate.
-- Seed data: run Persistence/Database/Seed/Initial_Migration_Seed.sql separately.
-- Idempotent (dotnet ef migrations script --idempotent).
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260601115438_AddGloCommunicationTemplateAndSchemaComments') THEN
    CREATE SCHEMA IF NOT EXISTS billing;
    CREATE SCHEMA IF NOT EXISTS crm;
    CREATE SCHEMA IF NOT EXISTS dispatch;
    CREATE SCHEMA IF NOT EXISTS integration;
    CREATE SCHEMA IF NOT EXISTS inventory;
    CREATE SCHEMA IF NOT EXISTS notification;
    CREATE SCHEMA IF NOT EXISTS reporting;
    CREATE SCHEMA IF NOT EXISTS workflow;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260601115438_AddGloCommunicationTemplateAndSchemaComments') THEN
    COMMENT ON SCHEMA audit IS 'Stores audit logs, entity history, security events, and compliance records.';
    COMMENT ON SCHEMA billing IS 'Stores estimates, invoices, payments, taxes, and financial transactions.';
    COMMENT ON SCHEMA crm IS 'Stores leads, customers, contacts, opportunities, and customer-related data.';
    COMMENT ON SCHEMA dispatch IS 'Stores work orders, appointments, scheduling, routing, and service operations.';
    COMMENT ON SCHEMA glo IS 'Stores FSM platform-owned global master and reference data shared across all tenants.';
    COMMENT ON SCHEMA identity IS 'Stores users, roles, permissions, authentication, and security-related data.';
    COMMENT ON SCHEMA integration IS 'Stores external system integrations, credentials, webhooks, and synchronization data.';
    COMMENT ON SCHEMA inventory IS 'Stores inventory items, warehouses, stock transactions, and purchasing data.';
    COMMENT ON SCHEMA notification IS 'Stores notification templates, delivery queues, messages, and communication history.';
    COMMENT ON SCHEMA shared IS 'Stores reusable tenant-owned entities shared across multiple business modules.';
    COMMENT ON SCHEMA tenant IS 'Stores tenant organizational structure, companies, subscriptions, and ownership data.';
    COMMENT ON SCHEMA setup IS 'Stores tenant business configuration, operational settings, pricing, tax, and accounting setup.';
    COMMENT ON SCHEMA reporting IS 'Stores report definitions, dashboards, KPIs, and analytics configuration.';
    COMMENT ON SCHEMA workflow IS 'Stores workflow definitions, automation rules, triggers, and business processes.';
    END IF;
END $EF$;

COMMIT;
