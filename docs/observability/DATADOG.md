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

**Testing split:** in `src/Gateway/docker-compose.yml`, Datadog/OTLP is **on** only for `setup-service` and `user-service`; all other APIs use `x-datadog-env-off` (`Datadog__Enabled` / `Observability__Enabled` false, empty ApiKey/OTLP).

1. Ensure Setup seed includes `DATADOG` (`Initial_Migration_Seed.sql`), or run `tools/seed_datadog_dev_credential.py` for DEV.
2. Create a **Global** Datadog credential in **dev** only (via Credential API / Postman), e.g.:

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
3. Set agent key for the Datadog Agent container: `DD_API_KEY` (same value as the credential ApiKey).
4. From `src/Gateway`:

```bash
docker compose up -d datadog-agent
docker compose up -d
```

Apps send:

- **Traces / metrics (OTLP)** → `http://datadog-agent:4317` (`Observability:OtlpEndpoint`, or derived from `Datadog:AgentHost`) — from appsettings/compose
- **Logs** → Datadog Logs API when `Datadog:ApiKey` is supplied by the **credential snapshot** (Serilog), plus JSON console

Empty `ApiKey` / empty `OtlpEndpoint` (and no `AgentHost`) keeps the process healthy; nothing is exported.

**LLM Observability / Datadog AI is disabled:** `DD_LLMOBS_ENABLED=false` on the agent and all app containers.

### Config split (dev)

| Source | Keys |
|--------|------|
| **Credential table (Global DATADOG)** | `ApiKey`, optional `Site` |
| **appsettings / compose** | `Observability:*`, `Datadog:Enabled`, `AgentHost`, toggles, `DD_LLMOBS_ENABLED` |

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
