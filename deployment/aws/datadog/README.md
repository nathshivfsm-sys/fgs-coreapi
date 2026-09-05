# Datadog on AWS ECS/Fargate

This folder scaffolds production wiring. Do **not** put API keys in source, Dockerfiles, or committed task JSON.

## Logs (current slice)

The Datadog API key lives in **Setup** `glo.GloCredential` (provider `DATADOG`, Global scope). Apps map `Global:DATADOG:*` → `Datadog:ApiKey` / `Datadog:Site`. Setup reads its own table at startup; other APIs get it via credential distribution (Redis snapshot at `redis:6379` on the dev ECS stack).

Do **not** set `Datadog__ApiKey` or `DD_API_KEY` on the ECS task for this path — an environment value overrides the credential.

Keep `Datadog__Enabled=true`, `DD_LLMOBS_ENABLED=false`. Traces/metrics stay off until an agent sidecar is added.

## Secrets (agent / FireLens only)

When you add a Datadog Agent sidecar or FireLens log routing, that **sidecar** still needs `DD_API_KEY` from Secrets Manager. The application container should keep using the Setup credential for the Serilog sink.

1. Store the Datadog API key in AWS Secrets Manager (e.g. `fgs/datadog/api-key`) for the **agent/FireLens** only.
2. Grant the task execution role `secretsmanager:GetSecretValue`.
3. Map the secret into the **sidecar**, not into `Datadog__ApiKey` on the app.

## Patterns

### Agent sidecar (recommended for OTLP traces + metrics)

- Application container: set `Observability__OtlpEndpoint=http://localhost:4317` (or sidecar hostname), `Observability__Env`, `Observability__Version`. Keep `DD_LLMOBS_ENABLED=false`.
- Sidecar: official Datadog Agent image with `DD_API_KEY` from Secrets Manager, OTLP receivers enabled, `DD_APM_NON_LOCAL_TRAFFIC=true`, and **`DD_LLMOBS_ENABLED=false`** (Datadog LLM Observability / AI off).

### FireLens / Fluent Bit (logs)

- Route container stdout JSON logs to Datadog via FireLens.
- Keep Serilog JSON console enabled. If FireLens owns log shipping, you can leave the Serilog Datadog sink driven by the Setup credential or turn it off so logs are not duplicated.

## Sample fragments

See `ecs-task-definition.fragment.json` for **sidecar** environment/secret shape. Merge into real service task definitions per environment. App log ApiKey remains the Setup DATADOG credential.

See also [docs/observability/DATADOG.md](../../../docs/observability/DATADOG.md).
