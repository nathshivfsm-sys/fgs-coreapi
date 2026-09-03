# Messaging workers

- **Publisher:** polls outbox tables → RabbitMQ (`OutboxSources` in appsettings)
- **Consumer:** handlers for tenant provision, invite email, credential audit
- **Idempotency:** Redis
- **Health-only HTTP** on both
- **Skills:** `implement-outbox`, `implement-consumer`
- **See:** [../messaging.md](../messaging.md), [../outbox.md](../outbox.md)
