# GitHub Actions CD → Amazon EC2

Deploys the shared ECR image (`fgs/dockers`) to a Docker host on EC2 after CI pushes a new channel tag.

**Scope:** `dev` branch only for now. Test/main and qa/prod environments can be added later.

| Git branch | Image channel | Deploy target | Approval | Example tags |
| --- | --- | --- | --- | --- |
| `dev` | `dev` | Repo var `EC2_INSTANCE_ID` | **None** (auto) | `setup-dev`, `user-dev`, `nginx-dev` |

Flow:

```text
Merge PR to dev → Build + push ECR → Deploy job (SSM → EC2) starts immediately
```

PR builds do **not** push or deploy.

---

## 1. Create the EC2 instance

1. **Launch instance** (Amazon Linux 2023 or Ubuntu 22.04+, `t3.medium` or larger recommended).
2. **VPC / subnet** — same VPC as RDS/ALB if you use them.
3. **Security group**
   - Inbound **80** from ALB security group (or your office IP for testing).
   - No SSH required if you use SSM Session Manager.
4. **IAM instance profile** — attach a role with at least:
   - `AmazonSSMManagedInstanceCore` (SSM agent)
   - ECR read (`ecr:GetAuthorizationToken`, `ecr:BatchGetImage`, `ecr:GetDownloadUrlForLayer`, `ecr:BatchCheckLayerAvailability`)
   - `ecr:DescribeRepositories` on your `fgs/dockers` repo

Example inline policy for the EC2 role:

```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Action": "ecr:GetAuthorizationToken",
      "Resource": "*"
    },
    {
      "Effect": "Allow",
      "Action": [
        "ecr:BatchCheckLayerAvailability",
        "ecr:GetDownloadUrlForLayer",
        "ecr:BatchGetImage",
        "ecr:DescribeRepositories"
      ],
      "Resource": "arn:aws:ecr:us-east-1:ACCOUNT_ID:repository/fgs/dockers"
    }
  ]
}
```

5. Note the **instance ID** (`i-xxxxxxxx`) — you will set it in GitHub.

---

## 2. Bootstrap the EC2 host (one time)

Copy the files from `deployment/aws/ec2/` to the instance and run bootstrap:

```bash
# On your workstation (replace with your instance id / use SSM port-forward if needed)
INSTANCE_ID=i-0123456789abcdef0
aws ssm start-session --target "$INSTANCE_ID"

# On the EC2 instance (as root)
sudo mkdir -p /opt/fgs
# Copy deploy-service.sh, docker-compose.ec2.yml, nginx-http-only-entrypoint.sh, bootstrap-ec2.sh
sudo bash /path/to/bootstrap-ec2.sh
```

Or clone the repo and run:

```bash
cd /tmp
git clone <your-repo-url> fgs
sudo bash fgs/deployment/aws/ec2/bootstrap-ec2.sh
```

Then edit config:

```bash
sudo nano /opt/fgs/config/setup-appsettings.json   # FgsSetup RDS only
sudo nano /opt/fgs/.env                            # RABBITMQ_PASSWORD (broker boot — must match GloCredential)
```

Ensure **`glo.GloCredential`** includes `Global:DATABASE:FgsUser`, `Global:REDIS`, `Global:RABBITMQ`, etc. User-service does **not** use a connection string file on EC2.

### RabbitMQ credentials (GloCredential vs `.env`)

| Where | Purpose |
| --- | --- |
| **`glo.GloCredential` `Global:RABBITMQ`** | Setup reads **Username** / **Password** (or **ConnectionUri**) at startup |
| **`/opt/fgs/.env` `RABBITMQ_USER` / `RABBITMQ_PASSWORD`** | RabbitMQ **container** boot only (`RABBITMQ_DEFAULT_*`) |

The password in `.env` must **match** the password stored in `GloCredential` so the broker and Setup agree.

Setup **does not** get `RabbitMq__UserName` / `RabbitMq__Password` from compose (env would override the credential table). Compose still sets `RabbitMq__HostName=rabbitmq` unless you use `Global:RABBITMQ:ConnectionUri` (e.g. `amqp://fgs:pass@rabbitmq:5672/`).

User-service loads RabbitMQ (and other providers) from Setup via credential distribution — no RabbitMQ env on that container.

**First deploy** (pull all three images and start the stack):

```bash
cd /opt/fgs
sudo ./deploy-service.sh setup-service dev
sudo ./deploy-service.sh user-service dev
sudo ./deploy-service.sh nginx dev
```

Verify:

```bash
curl -s http://localhost/nginx-health
curl -s http://localhost/api/v1/setup/health   # if routed
docker compose -f docker-compose.ec2.yml ps
```

---

## 3. GitHub configuration

### Variables (repository)

**Settings** → **Secrets and variables** → **Actions** → **Variables**

| Variable | Value |
| --- | --- |
| `AWS_REGION` | `us-east-1` |
| `ECR_REPO` | `fgs/dockers` |
| `EC2_INSTANCE_ID` | Your EC2 instance ID `i-xxxxxxxx` |
| `AWS_ROLE_TO_ASSUME` | IAM role ARN (OIDC only; omit if using access keys) |
| `FGS_COMPOSE_DIR` | Optional — `/opt/fgs` (default) |

No GitHub **Environment** is required for CD; deploy reads `EC2_INSTANCE_ID` from repository variables.

### Secrets (if using access keys instead of OIDC)

| Secret | Value |
| --- | --- |
| `AWS_ACCESS_KEY_ID` | IAM user access key |
| `AWS_SECRET_ACCESS_KEY` | IAM user secret |

---

## 4. IAM for GitHub Actions (SSM deploy)

The CI/CD role needs ECR push **and** SSM send-command permissions.

Terraform (`deployment/aws/terraform/iam.tf`) creates:

| Resource | Permissions |
| --- | --- |
| `fgs-<env>-github-actions` (OIDC role) | ECR push + SSM SendCommand (CD) |
| `fgs-<env>-ec2-role` + instance profile | `AmazonSSMManagedInstanceCore` + ECR pull |
| `fgs-<env>-ssm-session-operator` (policy) | StartSession / DescribeSessions for operators |

If you manage IAM manually, add:

```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Sid": "Ec2DeployViaSsm",
      "Effect": "Allow",
      "Action": "ssm:SendCommand",
      "Resource": [
        "arn:aws:ssm:us-east-1::document/AWS-RunShellScript",
        "arn:aws:ec2:us-east-1:ACCOUNT_ID:instance/*"
      ]
    },
    {
      "Sid": "SsmCommandResults",
      "Effect": "Allow",
      "Action": [
        "ssm:GetCommandInvocation",
        "ssm:ListCommands",
        "ssm:ListCommandInvocations"
      ],
      "Resource": "*"
    }
  ]
}
```

Replace `ACCOUNT_ID` with your AWS account ID. Do **not** add Session Manager (`StartSession`, etc.) on the GitHub CD user — that is only for interactive shells on your laptop.

---

## 5. Workflows

| Workflow | Build trigger | Deploy target |
| --- | --- | --- |
| `build-setup.yml` | `Fgs.Setup.API.csproj` version bump | `setup-service` on EC2 |
| `build-user.yml` | `Fgs.User.API.csproj` version bump | `user-service` on EC2 |
| `build-nginx.yml` | `src/Gateway/VERSION` bump | `nginx` on EC2 |

Reusable deploy: `.github/workflows/reusable-deploy-ec2.yml`

It runs via SSM:

```bash
sudo /opt/fgs/deploy-service.sh <compose-service> <channel> fgs/dockers us-east-1
```

---

## 6. ALB (optional)

Point an Application Load Balancer at the EC2 instance:

| Setting | Value |
| --- | --- |
| Target type | Instance |
| Port | 80 |
| Health check path | `/nginx-health` |
| Health check matcher | 200 |

Nginx on EC2 listens on **port 80 only** (TLS terminated at ALB). The entrypoint script matches the ECS `gateway_start` pattern.

---

## 7. Troubleshooting

| Symptom | Fix |
| --- | --- |
| `Set GitHub variable EC2_INSTANCE_ID` | Add repository variable `EC2_INSTANCE_ID` (Settings → Actions → Variables). |
| SSM `AccessDenied` on SendCommand | Add SSM permissions to the GitHub Actions IAM role; confirm EC2 has SSM agent + `AmazonSSMManagedInstanceCore`. |
| SSM command `Failed` | In AWS Console → Systems Manager → Run Command → view stdout/stderr. Often missing `/opt/fgs/deploy-service.sh` or bad `.env`. |
| `Cannot perform StartSession` | Instance not registered with SSM — check instance profile and outbound HTTPS (443) to SSM endpoints. |
| ECR pull denied on EC2 | EC2 instance role needs ECR read on `fgs/dockers`. |
| Setup/User unhealthy | Check `docker logs`; verify `config/*-appsettings.json` connection strings. |
| nginx 502 | Upstream not healthy — wait for setup/user health checks; `docker compose ps`. |

### Manual SSM test (same as pipeline)

```bash
aws ssm send-command \
  --instance-ids i-0123456789abcdef0 \
  --document-name AWS-RunShellScript \
  --parameters 'commands=["sudo /opt/fgs/deploy-service.sh user-service dev fgs/dockers us-east-1"]' \
  --query Command.CommandId --output text
```

---

## 8. ECS vs EC2

- **ECS** deploy workflow is still available: `.github/workflows/reusable-deploy-ecs.yml`
- Build workflows now default to **EC2** deploy. To switch back to ECS, change the `deploy` job in `build-*.yml` to call `reusable-deploy-ecs.yml` instead.

---

## Files

| Path | Purpose |
| --- | --- |
| `deployment/aws/ec2/docker-compose.ec2.yml` | Stack: redis, rabbitmq, setup, user, nginx |
| `deployment/aws/ec2/deploy-service.sh` | Pull one ECR image + recreate container |
| `deployment/aws/ec2/bootstrap-ec2.sh` | One-time Docker + `/opt/fgs` setup |
| `deployment/aws/ec2/nginx-http-only-entrypoint.sh` | Nginx :80 for ALB |
| `.github/workflows/reusable-deploy-ec2.yml` | SSM-based CD |
