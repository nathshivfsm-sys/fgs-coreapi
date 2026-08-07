-- Template: least-privilege role for Publisher outbox polling.
-- Do NOT apply automatically. Review and run manually per environment.
-- Grants SELECT + UPDATE only on outbox tables (claim/retry status updates).

-- CREATE ROLE fgs_outbox_reader LOGIN PASSWORD '...';

-- GRANT USAGE ON SCHEMA tenant TO fgs_outbox_reader;
-- GRANT USAGE ON SCHEMA glo TO fgs_outbox_reader;
-- GRANT USAGE ON SCHEMA setup TO fgs_outbox_reader;
-- GRANT USAGE ON SCHEMA crm TO fgs_outbox_reader;
-- GRANT USAGE ON SCHEMA inventory TO fgs_outbox_reader;

-- GRANT SELECT, UPDATE ON TABLE tenant."TenantOutboxMessage" TO fgs_outbox_reader;
-- GRANT SELECT, UPDATE ON TABLE glo."GloOutboxMessage" TO fgs_outbox_reader;
-- GRANT SELECT, UPDATE ON TABLE setup."SetupOutboxMessage" TO fgs_outbox_reader;
-- GRANT SELECT, UPDATE ON TABLE crm."CrmOutboxMessage" TO fgs_outbox_reader;
-- GRANT SELECT, UPDATE ON TABLE inventory."InventoryOutboxMessage" TO fgs_outbox_reader;

-- Then configure Publisher OutboxSources[*].OutboxConnectionStringName to a
-- connection that authenticates as fgs_outbox_reader (falls back to ConnectionStringName).
