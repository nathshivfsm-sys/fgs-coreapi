# Deployment

## Local

- `src/Gateway/docker-compose.yml` (`fgs-local`)
- Entry: NGINX → `https://developer.fsm.com`
- Postgres usually on host; RabbitMQ in compose
- Consumer on Docker network, not NGINX-exposed; owning APIs publish outbox in-process


## AWS

- Primary CD: **EC2** via SSM (`reusable-deploy-ec2.yml`, `deployment/aws/ec2/`)
- Images: ECR (`reusable-build-service.yml`)
- ECS workflow exists (`reusable-deploy-ecs.yml`) but build callers target EC2 today
- Terraform under `deployment/aws/terraform/`
- Observability: Datadog (`docs/observability/DATADOG.md`)

## CI workflows with images

`build-user`, `build-setup`, `build-bff`, `build-file`, `build-audit`, `build-notification`, `build-consumer`, `build-nginx`, `build-redis`, `build-rabbitmq`
