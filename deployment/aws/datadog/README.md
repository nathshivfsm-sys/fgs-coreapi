# Datadog on AWS ECS/Fargate

This folder scaffolds production wiring. Do **not** put API keys in source, Dockerfiles, or committed task JSON.

Apps use **OpenTelemetry → OTLP** for traces/metrics and **Serilog** (or FireLens) for logs. Datadog Agent sidecar receives OTLP on `4317`/`4318`.

## Secrets

1. Store the Datadog API key in AWS Secrets Manager (e.g. `fgs/datadog/api-key`).
2. Grant the task execution role `secretsmanager:GetSecretValue` (see existing IAM docs under `deployment/aws`).
3. Map the secret into the task as `DD_API_KEY` and/or `Datadog__ApiKey`.

## Patterns

### Agent sidecar (recommended for OTLP traces + metrics)

- Application container: set `Observability__OtlpEndpoint=http://localhost:4317` (or sidecar hostname), `Observability__Env`, `Observability__Version`. Keep `DD_LLMOBS_ENABLED=false`.
- Sidecar: official Datadog Agent image with `DD_API_KEY` from Secrets Manager, OTLP receivers enabled, `DD_APM_NON_LOCAL_TRAFFIC=true`, and **`DD_LLMOBS_ENABLED=false`** (Datadog LLM Observability / AI off).

### FireLens / Fluent Bit (logs)

- Route container stdout JSON logs to Datadog via FireLens.
- Keep Serilog JSON console enabled; you may disable the Serilog Datadog sink in prod if FireLens owns log shipping (set empty `Datadog__ApiKey` and rely on Agent/FireLens).

## Sample fragments

See `ecs-task-definition.fragment.json` for environment/secret shape. Merge into real service task definitions per environment.

See also [docs/observability/DATADOG.md](../../../docs/observability/DATADOG.md).
