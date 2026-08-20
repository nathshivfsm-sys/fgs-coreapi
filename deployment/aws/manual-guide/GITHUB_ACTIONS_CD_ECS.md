# GitHub Actions CD → Amazon ECS

Deploys the shared ECR image (`fgs/dockers`) to ECS after CI pushes a new channel tag.

| Git branch | Image channel | GitHub Environment | Approval | Default ECS cluster |
| --- | --- | --- | --- | --- |
| `dev` | `dev` (`setup-dev`, …) | `dev` | **None** (auto) | `fgs-dev` |
| `test` | `test` | `qa` | **Required** | `fgs-test` |
| `main` | `prod` | `prod` | **Required** | `fgs-prod` |

Flow:

```text
Merge PR → Build + push ECR → Deploy job (Environment)
  dev  → starts immediately
  qa / prod → waits for reviewer approval in GitHub
```

PR builds do **not** push or deploy. Manual **Run workflow** deploys only if you also set **push_to_ecr**.

---

## 1. Create GitHub Environments

Repo → **Settings** → **Environments**:

### `dev`

1. **New environment** → name `dev`.
2. Do **not** add required reviewers.
3. Optional: add environment variable `ECS_CLUSTER` = `fgs-dev` if you need to override the default.

### `qa`

1. **New environment** → name `qa`.
2. **Deployment protection rules** → **Required reviewers** → add 1+ people or a team.
3. Optional: `ECS_CLUSTER` = `fgs-test` (if your test stack cluster is not `fgs-test`).

### `prod`

1. **New environment** → name `prod`.
2. **Required reviewers** → add approvers (can differ from qa).
3. Optional: `ECS_CLUSTER` = `fgs-prod`.

Repo (or environment) variables still needed:

| Variable | Value |
| --- | --- |
| `AWS_REGION` | `us-east-1` |
| `AWS_ROLE_TO_ASSUME` | IAM role ARN used by CI/CD |
| `ECR_REPO` | `fgs/dockers` |

---

## 2. IAM (ECS deploy)

The GitHub OIDC role (`fgs-dev-github-actions` or shared deploy role) needs ECR push **and**:

```json
{
  "Effect": "Allow",
  "Action": [
    "ecs:UpdateService",
    "ecs:DescribeServices",
    "ecs:DescribeClusters",
    "ecs:DescribeTaskDefinition",
    "ecs:ListTasks",
    "ecs:DescribeTasks"
  ],
  "Resource": "*"
}
```

Terraform already adds this on the GitHub Actions role when you apply `deployment/aws/terraform`.

If you use **separate** AWS accounts/roles for qa/prod, set `AWS_ROLE_TO_ASSUME` as an **environment** variable on `qa` / `prod` (overrides the repo variable).

---

## 3. ECS task definitions

Services must already point at the **mutable channel tags**, for example:

```text
…/fgs/dockers:setup-dev
…/fgs/dockers:user-dev
…/fgs/dockers:nginx-dev
```

CD runs `aws ecs update-service --force-new-deployment`, which starts new tasks that pull the latest image for that tag. Cluster/service names:

| Service | ECS service name |
| --- | --- |
| Setup | `setup` |
| User | `user` |
| nginx | `gateway` |

---

## 4. Workflows

| File | Role |
| --- | --- |
| `reusable-build-service.yml` | Build, test, push to ECR |
| `reusable-deploy-ecs.yml` | Force ECS deploy (uses Environment) |
| `build-setup.yml` / `build-user.yml` / `build-nginx.yml` | Call build, then deploy when `image_pushed` |

---

## 5. How to approve qa / prod

1. Merge to `test` or `main` (with a version bump so CI builds).
2. Open **Actions** → the running workflow.
3. The **deploy** job shows **Waiting for review** / **Review deployments**.
4. Approver clicks **Approve and deploy**.

`dev` never waits.

---

## Troubleshooting

| Symptom | Likely cause |
| --- | --- |
| Deploy skipped | No image push (PR only, or version not bumped) |
| Waiting forever on qa/prod | No reviewers configured, or no one approved |
| Service not ACTIVE | Cluster/service missing; check `ECS_CLUSTER` and service name |
| AccessDenied on UpdateService | OIDC role missing ECS actions |
| Old code still running | Task definition not using the channel tag (`setup-dev`, etc.) |
