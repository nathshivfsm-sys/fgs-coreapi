# GitHub Actions CD → Amazon EC2

Deploys the shared ECR image (`fgs/dockers`) to a Docker host on EC2 after CI pushes a new channel tag.

| Git branch | Image channel | GitHub Environment | Approval | Example tags |
| --- | --- | --- | --- | --- |
| `dev` | `dev` | `dev` | **None** (auto) | `setup-dev`, `user-dev`, `nginx-dev` |
| `test` | `test` | `qa` | **Required** | `setup-test`, … |
| `main` | `prod` | `prod` | **Required** | `setup-prod`, … |

Flow:

```text
Merge PR → Build + push ECR → Deploy job (SSM → EC2)
  dev  → starts immediately
  qa / prod → waits for reviewer approval in GitHub
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
sudo nano /opt/fgs/config/setup-appsettings.json   # RDS connection string for Setup
sudo nano /opt/fgs/config/user-appsettings.json    # RDS connection string for User
sudo nano /opt/fgs/.env                            # RABBITMQ_PASSWORD, ASPNETCORE_ENVIRONMENT
```

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

### Environments

Same as ECS CD — create `dev`, `qa`, `prod` with reviewers on `qa` and `prod`.

### Variables

| Variable | Where | Value |
| --- | --- | --- |
| `AWS_REGION` | Repo | `us-east-1` |
| `ECR_REPO` | Repo | `fgs/dockers` |
| `AWS_ROLE_TO_ASSUME` | Repo (OIDC) or use access-key secrets | IAM role ARN |
| `EC2_INSTANCE_ID` | **Per environment** (`dev`, `qa`, `prod`) | `i-xxxxxxxx` for that stack's EC2 |
| `FGS_COMPOSE_DIR` | Optional | `/opt/fgs` (default) |

Set `EC2_INSTANCE_ID` on each GitHub Environment so dev/test/prod can target different instances.

### Secrets (if using access keys instead of OIDC)

| Secret | Value |
| --- | --- |
| `AWS_ACCESS_KEY_ID` | IAM user access key |
| `AWS_SECRET_ACCESS_KEY` | IAM user secret |

---

## 4. IAM for GitHub Actions (SSM deploy)

The CI/CD role needs ECR push **and** SSM send-command permissions.

Terraform (`deployment/aws/terraform/iam.tf`) adds `Ec2DeployViaSsm` when you apply. If you manage IAM manually, add:

```json
{
  "Sid": "Ec2DeployViaSsm",
  "Effect": "Allow",
  "Action": [
    "ssm:SendCommand",
    "ssm:GetCommandInvocation",
    "ssm:ListCommands",
    "ssm:ListCommandInvocations"
  ],
  "Resource": [
    "arn:aws:ssm:us-east-1::document/AWS-RunShellScript",
    "arn:aws:ec2:us-east-1:*:instance/*"
  ]
}
```

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
| `Set GitHub variable EC2_INSTANCE_ID` | Add `EC2_INSTANCE_ID` on the GitHub Environment (`dev`, `qa`, or `prod`). |
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
