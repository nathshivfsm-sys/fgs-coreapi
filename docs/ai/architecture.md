# Architecture

Multi-tenant FSM backend on **.NET 10**, microservices, Clean Architecture per service, CQRS (MediatR), outbox → **RabbitMQ**.

```text
Client → NGINX (Gateway) → BFF (orchestration) and/or owning APIs
                         → Application (Features) → Infrastructure
                         → Outbox → service worker → RabbitMQ → Consumer
```


## Layers (typical service)

| Project | Role |
|---------|------|
| `*.API` | Controllers, `Program.cs`, health |
| `*.Application` | Commands/queries, validators, abstractions |
| `*.Domain` | Entities |
| `*.Infrastructure` | EF, Dapper, Refit, messaging adapters |
| `*.Tests` | xUnit |

BFF has no Domain. Consumer is a dedicated worker with health HTTP; owning APIs host an in-process outbox `BackgroundService`.


## Shared libraries

| Library | Role |
|---------|------|
| `Fgs.Kernel` | Entity bases, tenant markers |
| `Fgs.Foundation` | Host, controllers, MediatR behaviors, Redis cache, Refit helpers, idempotency |
| `Fgs.Contracts` | `ApiResponse<T>`, Refit clients, integration event names |
| `Fgs.Persistence` | `IUnitOfWork`, `IRepository<>`, Npgsql |
| `Fgs.MultiTenancy` | Headers, middleware, `FgsTenantFilteredDbContext` |
| `Fgs.Security` | Entra JWT, permissions, active-user |
| `Fgs.Messaging` | RabbitMQ, outbox poller, consumer framework |
| `Fgs.Credentials` | Remote credential snapshot |
| `Fgs.Observability` | Serilog + Datadog/OTel |

## Not in repo

Kafka/MSK, YARP, OpenSearch, platform-issued JWT, DB inbox tables, WebApplicationFactory integration suite.
