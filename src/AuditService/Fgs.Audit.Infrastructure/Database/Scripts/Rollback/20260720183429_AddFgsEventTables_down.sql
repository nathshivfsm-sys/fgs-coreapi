-- Rollback for 20260720183429_AddFgsEventTables

START TRANSACTION;

DROP TABLE IF EXISTS audit."FgsEventAttachment";
DROP TABLE IF EXISTS audit."FgsEventDetail";
DROP TABLE IF EXISTS audit."FgsEvent";
DROP TABLE IF EXISTS audit."FgsArchiveCatalog";

DROP TYPE IF EXISTS audit.event_detail_type;
DROP TYPE IF EXISTS audit.event_source;
DROP TYPE IF EXISTS audit.record_type;

DELETE FROM audit."__EFMigrationsHistory"
WHERE "MigrationId" = '20260720183429_AddFgsEventTables';

COMMIT;
