# Datadog Observability

Shared implementation lives in `Fgs.Observability`. Every API uses:

```csharp
builder.AddFgsObservability(hostOptions.ServiceName);
app.MapFgsHealthChecks();
```

## Local (Docker Desktop)

1. Set a test API key in each service `appsettings.json` under `Datadog:ApiKey` (or override with env `Datadog__ApiKey`).
2. Set the same key for the Agent: `DD_API_KEY` when starting compose.
3. From `src/Gateway`:

```bash
docker compose up -d datadog-agent
docker compose up -d
```

Apps send:

- **APM / DogStatsD** → `datadog-agent:8126` / `8125` (`Datadog:AgentHost`)
- **Logs** → Datadog Logs API when `Datadog:ApiKey` is set (Serilog sink), plus JSON console

Empty `ApiKey` / `AgentHost` keeps the process healthy; nothing is exported.

## Configuration (`Datadog` section)

| Key | Purpose |
|-----|---------|
| `Enabled` | Master switch |
| `ApiKey` | Logs intake (never commit real keys) |
| `Site` | e.g. `datadoghq.com` |
| `AgentHost` | APM/DogStatsD host (`datadog-agent` in compose) |
| `AgentPort` / `DogStatsDPort` | 8126 / 8125 |
| `Env` / `Version` | Unified `DD_ENV` / `DD_VERSION` |
| `EnableApm` / `EnableRuntimeMetrics` | Tracer toggles |

`DD_SERVICE` is the unique `serviceName` passed to `AddFgsObservability`.

## Log facets

Every structured log may include: `Service`, `ServiceName`, `Environment`, `Version`, `TraceId`, `SpanId`, `CorrelationId`, `TenantId`, `CompanyId`, `UserId`, `RequestPath`, `StatusCode`, `Duration`.

Sensitive fields (passwords, tokens, Authorization, connection strings, etc.) are redacted by policy.

## Health

- `/health/live` — no dependencies
- `/health/ready` — postgres / redis / rabbit when registered
- `/health` — aggregate

## Metrics (DogStatsD)

Examples: `cache.hit`, `cache.miss`, `cache.error`, `rabbitmq.publish`, `rabbitmq.consume`, `rabbitmq.consume_failure`, `outbox.pending`, `outbox.published`, `outbox.failed`, `outbox.retry`.

Use `IFgsMetrics` for business counters; avoid high-cardinality tags (UserId, entity ids, RequestId).

## Recommended monitors

| Monitor | Query idea |
|---------|------------|
| High 5xx | `sum:trace.aspnet_core.request.errors{env:prod}.as_rate()` / hits |
| High P95/P99 | APM latency percentile on `service:*` |
| Service unavailable | Monitor `/health/ready` synthetic or APM no-data |
| Database failures | APM error rate on `postgres` / `npgsql` spans |
| Redis failures | `sum:cache.error{*}.as_count()` or APM Redis errors |
| RabbitMQ depth | Agent RabbitMQ integration `rabbitmq.queue.messages` |
| Outbox backlog | `avg:outbox.pending{*}` / `outbox.failed` |
| ECS unhealthy | `aws.ecs.service.running` vs desired |
| High CPU/memory | `container.cpu` / `container.memory` |

## AWS ECS/Fargate (future)

See [deployment/aws/datadog/README.md](../../deployment/aws/datadog/README.md). Store `DD_API_KEY` in Secrets Manager; inject as task secrets. Prefer Agent sidecar or FireLens → Datadog; override `Datadog__*` via task environment.
