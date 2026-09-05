# Messaging

Broker: **RabbitMQ** (`Fgs.Messaging`).

## Exchanges (`IntegrationEventExchanges`)

`fgs.user`, `tenant.events`, `setup.events`, `audit.events`, `inventory.events` (+ DLX variants in service RabbitMq config).

## Known queues (appsettings)

| Queue | Exchange | Routing key |
|-------|----------|-------------|
| `Fgs.Setup.tenant.provision` | `tenant.events` | `tenant.provision.requested` |
| `fgs.user.notifications` | `fgs.user` | `user.CompanySignupInviteEmail` |
| `Fgs.Audit.credential` | `audit.events` | `audit.credential.requested` |

More routing keys exist in `IntegrationEventRoutingKeys` than have consumers yet.

## Roles

- Producing APIs: `IOutboxWriter` only on the request path; owning APIs run an in-process outbox `BackgroundService` (`AddFgsOutboxPublisher`)
- **ConsumerService**: subscribe → MediatR process commands; Redis idempotency

No Kafka. No inbox table.
