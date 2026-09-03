---
name: implement-outbox
description: Enqueue domain/integration events in the same transaction as business data using IOutboxWriter. Use when publishing events, adding outbox rows, or wiring Publisher sources.
---

# Implement outbox

Read [docs/ai/outbox.md](../../../docs/ai/outbox.md). Clone `Fgs.User.Infrastructure/Messaging/OutboxWriter.cs`.

## Steps

1. Confirm the service already has an outbox entity + `IOutboxWriter`. If not, copy User/Setup/Inventory pattern — do not publish RabbitMQ from the API process.
2. In the write path, after mutating aggregates: `IOutboxWriter.EnqueueAsync` with `IntegrationEvent*` type, payload, `correlationId`, `TenantId`, `CompanyId`, exchange, routing key.
3. `SaveChanges` once (entity + outbox).
4. Event names/keys go in `Fgs.Contracts` (`IntegrationEventExchanges`, `IntegrationEventRoutingKeys`).
5. If new table: add to Publisher `OutboxSources` in `Fgs.Publisher.API/appsettings.json`.
6. Add Consumer handler separately (`implement-consumer`).

## Verify

- [ ] Same transaction as business write
- [ ] Tenant/company on the outbox row
- [ ] Contract keys not hardcoded only in one service
