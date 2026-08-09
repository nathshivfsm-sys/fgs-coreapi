# Datadog on AWS ECS/Fargate

This folder scaffolds production wiring. Do **not** put API keys in source, Dockerfiles, or committed task JSON.

## Secrets

1. Store the Datadog API key in AWS Secrets Manager (e.g. `fgs/datadog/api-key`).
2. Grant the task execution role `secretsmanager:GetSecretValue` (see existing IAM docs under `deployment/aws`).
3. Map the secret into the task as `DD_API_KEY` and/or `Datadog__ApiKey`.

## Patterns

### Agent sidecar (recommended for APM + DogStatsD)

- Application container: set `Datadog__AgentHost=localhost` (or sidecar hostname), `Datadog__Env`, `Datadog__Version`.
- Sidecar: official Datadog Agent image with `DD_API_KEY` from Secrets Manager, `DD_APM_NON_LOCAL_TRAFFIC=true`, `DD_DOGSTATSD_NON_LOCAL_TRAFFIC=true`.

### FireLens / Fluent Bit (logs)

- Route container stdout JSON logs to Datadog via FireLens.
- Keep Serilog JSON console enabled; you may disable the Serilog Datadog sink in prod if FireLens owns log shipping (set empty `Datadog__ApiKey` and rely on Agent/FireLens).

## Sample fragments

See `ecs-task-definition.fragment.json` for environment/secret shape. Merge into real service task definitions per environment.
