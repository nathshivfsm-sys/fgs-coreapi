# Outbox

Within one unit of work:

1. Persist business changes
2. `IOutboxWriter.EnqueueAsync(...)`
3. `SaveChanges`

**PublisherService** runs `AddFgsOutboxProcessor` (`OutboxPollingBackgroundService`) and reads configured `OutboxSources` (cross-schema read — intentional publisher exception).

## Writers present

User (`TenantOutboxMessage`), Setup (`GloOutboxMessage` / `SetupOutboxMessage`), Inventory (`InventoryOutboxMessage`). CRM has outbox entity; wire writer only if following Inventory/User pattern.

Do not call `IRabbitMqPublisher` from API request threads for domain events.
