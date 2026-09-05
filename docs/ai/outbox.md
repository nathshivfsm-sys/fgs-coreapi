# Outbox

Within one unit of work:

1. Persist business changes
2. `IOutboxWriter.EnqueueAsync(...)`
3. `SaveChanges`

Owning APIs run a service-owned outbox worker via `AddFgsOutboxPublisher` (registers `OutboxPollingBackgroundService` for that service’s own table(s) only). Default transport is RabbitMQ through `IIntegrationEventPublisher` (`RabbitMqIntegrationEventPublisher`); swap with `OutboxPublisherBuilder.UsePublisher<T>()` (e.g. SQS) without changing writers.

## Workers present

| Service | Outbox table(s) | Worker |
|---------|-----------------|--------|
| User | `TenantOutboxMessage` | `AddFgsOutboxPublisher` in API |
| Setup | `GloOutboxMessage`, `SetupOutboxMessage` | `AddFgsOutboxPublisher` in API |
| Inventory | `InventoryOutboxMessage` | `AddFgsOutboxPublisher` in API |
| CRM | outbox entity exists | **no** worker yet |

Same-transaction writers (`IOutboxWriter` + `SaveChanges`) are unchanged. Do not call the broker publisher from API request threads for domain events — enqueue to the outbox instead.
