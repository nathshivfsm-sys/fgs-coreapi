# FGS **dev** — ECS Setup + nginx + ALB + Redis + RabbitMQ

Terraform for **dev**: **ECR**, **VPC (optional)**, **ECS Fargate On-Demand**, **Service Connect `fgs-dev`**, **Redis**, **RabbitMQ**, **Setup API**, **nginx**, **ALB**, **GitHub OIDC**.

ECS services run **continuously** (desired count 1).

Do **not** create RDS. Datadog `ApiKey` stays in Setup `glo.GloCredential`.

| Piece | Dev behavior |
| --- | --- |
| ECR | one repo `fgs/dockers` (tags `setup-dev`, `user-dev`, `nginx-dev`) |
| Cluster | `fgs-dev` |
| Service Connect | `setup-service:5004`, `redis:6379`, `rabbitmq:5672` |
| Gateway | nginx :80 behind ALB |
| Data plane | Redis + RabbitMQ as Fargate tasks (not ElastiCache / Amazon MQ) |
| Compute | Fargate On-Demand: Setup 512/1024, nginx 256/512, Redis 256/512, RabbitMQ 512/1024 |

HTTPS is optional (ACM). No NAT — tasks use a public IP to pull images.

**Estimated new AWS spend (us-east-1 list, 24/7):** about **$100–120 / month**. Full breakdown: [../manual-guide/MANUAL_DEPLOY_NGINX_SETUP_USER.md](../manual-guide/MANUAL_DEPLOY_NGINX_SETUP_USER.md#estimated-monthly-cost-dev-stack). Does **not** include existing RDS.

## Apply order

1. Copy [terraform.tfvars.example](terraform.tfvars.example) to `terraform.tfvars` (gitignored).
2. Keep `create_ecs_services = false` until Setup/nginx images exist. Redis and RabbitMQ still start when `create_redis_rabbitmq = true` (public images; no ECR build).
3. From this directory:

```bash
terraform init
terraform plan
terraform apply
```

4. Copy `github_actions_role_arn` into GitHub **Actions variables**:
   - `AWS_ROLE_TO_ASSUME` = output ARN
   - `AWS_REGION` = your region
   Workflows push to ECR on `dev` / `test` / `main` by default (set `PUSH_TO_ECR=false` only to skip publish).
5. Confirm the Datadog API key is in Setup `glo.GloCredential` (provider `DATADOG`, Global scope).
6. Build images (branch `dev` → tag `:dev`) and wait for **Build setup** / **Build nginx** (and **Build user** from the console runbook).
7. Set `create_ecs_services = true`, `terraform apply` again.
8. Open `alb_url` + `/nginx-health`.

If GitHub OIDC already exists in the account:

```hcl
create_github_oidc_provider = false
github_oidc_provider_arn    = "arn:aws:iam::ACCOUNT:oidc-provider/token.actions.githubusercontent.com"
```

## Existing VPC

```hcl
create_vpc        = false
vpc_id            = "vpc-..."
public_subnet_ids = ["subnet-a", "subnet-b"]  # two AZs, routes to an IGW
```

Public subnets so Fargate can `assign_public_ip = true` and pull images **without a NAT gateway**.

## Hosted Postgres

Set `setup_db_connection_string` (or `TF_VAR_setup_db_connection_string`) so Setup gets `ConnectionStrings__FgsSetup`.

Security groups on RDS must allow the Setup task security group or the VPC CIDR (`10.80.0.0/16` if this stack created the VPC).

## Redis and RabbitMQ

Default (`create_redis_rabbitmq = true`): Fargate services in namespace `fgs-dev`.

| DNS | Port | App env |
| --- | --- | --- |
| `redis` | 6379 | `Redis__ConnectionString=redis:6379` |
| `rabbitmq` | 5672 | `RabbitMq__HostName=rabbitmq`, `RabbitMq__UserName=fgs`, password from Secrets Manager `fgs/dev/rabbitmq` |

Storage is **ephemeral** (lost when the task is replaced). This is for **dev**, not HA.

To use existing ElastiCache / Amazon MQ instead:

```hcl
create_redis_rabbitmq   = false
redis_connection_string = "your-cache:6379"
rabbitmq_host           = "your-broker"
# or rabbitmq_connection_uri = "amqps://..."
```

## Datadog

Setup loads `Global:DATADOG:ApiKey` from the credential table. Terraform does **not** inject `Datadog__ApiKey` / `DD_API_KEY`.

## HTTPS

Optional. Full console steps: [../manual-guide/MANUAL_DEPLOY_NGINX_SETUP_USER.md](../manual-guide/MANUAL_DEPLOY_NGINX_SETUP_USER.md#a10-https-certificate-acm--alb).

```hcl
acm_certificate_arn = "arn:aws:acm:REGION:ACCOUNT:certificate/..."
```

## Everyday release

1. Bump `<Version>` in Setup (or `src/Gateway/VERSION` for nginx).
2. Merge to `dev` → GitHub Actions pushes ECR `:dev`.
3. Force a new ECS deployment (or apply with `image_tag`).

Redeploying Setup does **not** require an nginx config change while discovery stays `setup-service:5004`.

## Out of scope (later)

- User ECS service in this Terraform (User is in the console runbook)
- Datadog Agent sidecar / FireLens
- Private subnets + NAT
- RDS / ElastiCache / Amazon MQ modules
- Persistent volumes (EFS) for Redis/RabbitMQ
