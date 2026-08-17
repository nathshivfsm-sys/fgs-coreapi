# Observability (OpenTelemetry + Datadog)

Shared implementation lives in `Fgs.Observability`. Every API uses:

```csharp
builder.AddFgsObservability(hostOptions.ServiceName);
app.MapFgsHealthChecks();
```

## Architecture

- **Traces + metrics:** OpenTelemetry SDK → OTLP → Datadog Agent (local) or another OTLP backend later (AWS ADOT, App Insights exporter).
- **Logs:** Serilog JSON console + optional Serilog Datadog Logs sink (`Datadog:ApiKey`). OpenTelemetry Logs are not used.
- **Business metrics:** `IFgsMetrics` → OpenTelemetry `Meter` (`Fgs`).

```text
APIs (ILogger / IFgsMetrics)
  ├─ Serilog ──► console JSON
  │          └─► Datadog Logs API (when ApiKey set)
  └─ OpenTelemetry ──OTLP──► datadog-agent:4317 ──► Datadog
```

## Local (Docker Desktop)

**Default: logs-only (no agent).** In `src/Gateway/docker-compose.yml`, `setup-service` and `user-service` use `x-datadog-env-on` (Serilog → Datadog Logs API; OTLP/agent off). All other APIs use `x-datadog-env-off`. The `datadog-agent` service is behind Compose profile `datadog-agent` and does not start with a normal `docker compose up`.

1. Ensure Setup seed includes `DATADOG` (`Initial_Migration_Seed.sql`), or run `tools/seed_datadog_dev_credential.py` for DEV.
2. Set `DD_API_KEY` for Serilog at process start (compose `Datadog__ApiKey`), **or** create a **Global** Datadog credential in **dev** (via Credential API / Postman). Prefer `DD_API_KEY` for local logs-only so the sink attaches at startup:

```json
{
  "scope": 1,
  "providerCode": "DATADOG",
  "credentialName": "DatadogDev",
  "payload": "{\"ApiKey\":\"<your-dev-api-key>\",\"Site\":\"datadoghq.com\"}",
  "description": "Datadog API key for local/dev",
  "tenantId": null,
  "companyId": null
}
```

   That maps to `Datadog:ApiKey` / `Datadog:Site` via credential distribution (`Global:DATADOG:*`).
3. From `src/Gateway`:

```bash
# Logs only (default) — set DD_API_KEY in the environment or a local .env
docker compose up -d
```

Apps send:

- **Logs** → Datadog Logs API when `Datadog:ApiKey` is set (`DD_API_KEY` / credential), plus JSON console
- **Traces / metrics** → off by default. Optional agent: `docker compose --profile datadog-agent up -d datadog-agent`, then enable OTLP/`AgentHost` on the services you want.

Empty `ApiKey` / empty `OtlpEndpoint` (and no `AgentHost`) keeps the process healthy; nothing is exported.

**Credential hot-reload (Datadog logs):** updating the Global `DATADOG` credential republishes the Redis snapshot. Consumers always retain `Global:DATADOG:*` (even when `DATADOG` is not in `RequiredProviders`), refresh `DatadogOptions`, and the Serilog Datadog sink rebuilds on the next log when `ApiKey` changes. Keep `DD_SITE` in Compose/env to pin the intake region (e.g. `us5.datadoghq.com`).

**Credential hot-reload (DATABASE connection strings):** DbContexts use scoped options (`AddFgsDbContext`) so each request re-resolves the connection string from the credential holder. Dapper read factories and publisher outbox stores resolve on each open/claim as well.

**LLM Observability / Datadog AI is disabled everywhere:** `AddFgsObservability` forces `DD_LLMOBS_ENABLED=false` and `Datadog:EnableLlmObs=false`. Compose/ECS also set `DD_LLMOBS_ENABLED=false` on app and agent containers.

### Config split (dev)

| Source | Keys |
|--------|------|
| **Credential table (Global DATADOG)** / `DD_API_KEY` | `ApiKey` (hot-reloads into Serilog sink after snapshot pub/sub), optional `Site` |
| **appsettings / compose** | `Observability:*`, `Datadog:Enabled`, `AgentHost`, toggles, `EnableLlmObs` (always false), `DD_LLMOBS_ENABLED`; `DD_SITE` pins intake region (e.g. `us5.datadoghq.com`) |

## Configuration

### `Observability` section (preferred for traces/metrics)

| Key | Purpose |
|-----|---------|
| `Enabled` | Master switch for OTel registration |
| `ServiceName` | Resource service name (also set via `AddFgsObservability`) |
| `Env` / `Version` | Resource attributes |
| `OtlpEndpoint` | OTLP gRPC endpoint, e.g. `http://datadog-agent:4317` |
| `EnableTracing` / `EnableMetrics` / `EnableRuntimeMetrics` | Signal toggles |

### `Datadog` section (logs + legacy alias)

| Key | Purpose |
|-----|---------|
| `Enabled` | Used when `Observability:Enabled` is unset |
| `ApiKey` | Serilog Datadog log intake — **prefer Global `DATADOG` credential** in dev (never commit real keys) |
| `Site` | e.g. `datadoghq.com` |
| `AgentHost` | If `Observability:OtlpEndpoint` is empty → `http://{AgentHost}:4317` |
| `Env` / `Version` / `EnableApm` / `EnableRuntimeMetrics` | Fallbacks when Observability keys are unset |
| `EnableLlmObs` | Always `false`; `AddFgsObservability` also forces `DD_LLMOBS_ENABLED=false` |

## Log facets

Every structured log may include: `Service`, `ServiceName`, `Environment`, `Version`, `TraceId`, `SpanId`, `CorrelationId`, `TenantId`, `CompanyId`, `UserId`, `RequestPath`, `StatusCode`, `Duration`.

`TraceId` / `SpanId` come from `Activity.Current` (OpenTelemetry). Sensitive fields are redacted by policy.

## Health

- `/health/live` — no dependencies
- `/health/ready` — postgres / redis / rabbit when registered
- `/health` — aggregate

## Metrics (`IFgsMetrics`)

Examples: `cache.hit`, `cache.miss`, `cache.error`, `rabbitmq.publish`, `rabbitmq.consume`, `rabbitmq.consume_failure`, `outbox.pending`, `outbox.published`, `outbox.failed`, `outbox.retry`.

Avoid high-cardinality tags (UserId, entity ids, RequestId).

## Switching backends later

| Target | Traces/metrics | Logs |
|--------|----------------|------|
| Datadog (current) | OTLP → Datadog Agent | Serilog Datadog sink or FireLens |
| AWS | OTLP → ADOT / X-Ray / AMP / CloudWatch | FireLens or CloudWatch agent |
| App Insights | OTLP or Azure Monitor exporter | Serilog App Insights sink |

## Recommended monitors

| Monitor | Query idea |
|---------|------------|
| High 5xx | APM / OTel HTTP server error rate |
| High P95/P99 | Latency percentile on `service:*` |
| Service unavailable | `/health/ready` synthetic or APM no-data |
| Cache failures | `cache.error` via OTel metrics |
| Outbox backlog | `outbox.pending` / `outbox.failed` |
| ECS unhealthy | `aws.ecs.service.running` vs desired |

## AWS ECS/Fargate

See [deployment/aws/datadog/README.md](../../deployment/aws/datadog/README.md). Store `DD_API_KEY` in Secrets Manager. Prefer Agent sidecar with OTLP enabled; keep `DD_LLMOBS_ENABLED=false`.
