# Messaging workers

- **Consumer:** dedicated worker — handlers for tenant provision, invite email, credential audit
- **Outbox publish:** in-process in owning services via `AddFgsOutboxPublisher` (User, Setup, Inventory today); not a separate Publisher service
- **Idempotency:** Redis (Consumer)
- **Health-only HTTP** on Consumer; owning APIs also expose `/health`
- **Skills:** `implement-outbox`, `implement-consumer`
- **See:** [../messaging.md](../messaging.md), [../outbox.md](../outbox.md)
