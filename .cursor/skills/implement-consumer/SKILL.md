---
name: implement-consumer
description: Add a RabbitMQ consumer handler in ConsumerService with Redis idempotency. Use when consuming integration events or adding queues/bindings.
---

# Implement consumer

Clone `ProcessTenantProvisionRequestedCommandHandler` and wiring in `Fgs.Consumer.Infrastructure/DependencyInjection.cs`.

## Steps

1. Add routing key/exchange in `Fgs.Contracts` if missing.
2. Application command under `Fgs.Consumer.Application/Features/{Area}/Commands/Process{Event}/` — handler returns `Task` (not `ApiResponse`).
3. Call other services via existing Refit clients (`ISetupClient`, `INotificationDispatchClient`, `IAuditClient`). Do not open their databases.
4. Register subscription (queue, exchange, routing key, DLQ) in Consumer DI + `appsettings.json`.
5. Idempotency is framework Redis — do not add an inbox table.
6. Tests for the process command handler.

## Verify

- [ ] Consumer-only (not in the producing API)
- [ ] Refit/contracts only for S2S
- [ ] Queue names follow existing `Fgs.{Service}.*` style
- [ ] Unit tests passing
