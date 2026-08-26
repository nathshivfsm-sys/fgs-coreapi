data "aws_caller_identity" "current" {}

data "tls_certificate" "github" {
  count = var.create_github_oidc_provider ? 1 : 0
  url   = "https://token.actions.githubusercontent.com"
}

resource "aws_iam_openid_connect_provider" "github" {
  count           = var.create_github_oidc_provider ? 1 : 0
  url             = "https://token.actions.githubusercontent.com"
  client_id_list  = ["sts.amazonaws.com"]
  thumbprint_list = [data.tls_certificate.github[0].certificates[0].sha1_fingerprint]
}

# -----------------------------------------------------------------------------
# GitHub Actions (OIDC) — ECR push + SSM SendCommand CD to EC2
# -----------------------------------------------------------------------------

resource "aws_iam_role" "github_actions" {
  name = "${local.name_prefix}-github-actions"

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [{
      Effect = "Allow"
      Principal = {
        Federated = local.github_oidc_arn
      }
      Action = "sts:AssumeRoleWithWebIdentity"
      Condition = {
        StringEquals = {
          "token.actions.githubusercontent.com:aud" = "sts.amazonaws.com"
        }
        StringLike = {
          "token.actions.githubusercontent.com:sub" = "repo:${var.github_org}/${var.github_repo}:*"
        }
      }
    }]
  })
}

resource "aws_iam_role_policy" "github_actions_cicd" {
  name = "ecr-push-and-ec2-ssm-deploy"
  role = aws_iam_role.github_actions.id

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Sid      = "EcrAuth"
        Effect   = "Allow"
        Action   = ["ecr:GetAuthorizationToken"]
        Resource = "*"
      },
      {
        Sid    = "EcrPushPull"
        Effect = "Allow"
        Action = [
          "ecr:BatchCheckLayerAvailability",
          "ecr:GetDownloadUrlForLayer",
          "ecr:BatchGetImage",
          "ecr:PutImage",
          "ecr:InitiateLayerUpload",
          "ecr:UploadLayerPart",
          "ecr:CompleteLayerUpload",
          "ecr:DescribeRepositories",
          "ecr:DescribeImages",
        ]
        Resource = [aws_ecr_repository.app.arn]
      },
      {
        Sid    = "EcsDeploy"
        Effect = "Allow"
        Action = [
          "ecs:UpdateService",
          "ecs:DescribeServices",
          "ecs:DescribeClusters",
          "ecs:DescribeTaskDefinition",
          "ecs:ListTasks",
          "ecs:DescribeTasks",
        ]
        Resource = "*"
      },
      {
        Sid    = "Ec2DeployViaSsm"
        Effect = "Allow"
        Action = ["ssm:SendCommand"]
        Resource = [
          "arn:aws:ssm:${var.aws_region}::document/AWS-RunShellScript",
          "arn:aws:ec2:${var.aws_region}:${data.aws_caller_identity.current.account_id}:instance/*",
        ]
      },
      {
        Sid    = "SsmCommandResults"
        Effect = "Allow"
        Action = [
          "ssm:GetCommandInvocation",
          "ssm:ListCommands",
          "ssm:ListCommandInvocations",
          "ssm:DescribeInstanceInformation",
        ]
        Resource = "*"
      },
    ]
  })
}

# Optional IAM user for access-key CI/CD (same permissions as OIDC role).
# Prefer OIDC; set create_github_actions_user=true only if keys are required.
resource "aws_iam_user" "github_actions" {
  count = var.create_github_actions_user ? 1 : 0
  name  = "${local.name_prefix}-github-actions-user"
}

resource "aws_iam_user_policy" "github_actions_user_cicd" {
  count = var.create_github_actions_user ? 1 : 0
  name  = "ecr-push-and-ec2-ssm-deploy"
  user  = aws_iam_user.github_actions[0].name

  policy = aws_iam_role_policy.github_actions_cicd.policy
}

# -----------------------------------------------------------------------------
# EC2 instance role — SSM agent + ECR pull (docker compose on host)
# Attach instance profile to the FGS EC2 instance.
# -----------------------------------------------------------------------------

resource "aws_iam_role" "ec2" {
  count = var.create_ec2_iam ? 1 : 0
  name  = "${local.name_prefix}-ec2-role"

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [{
      Effect    = "Allow"
      Principal = { Service = "ec2.amazonaws.com" }
      Action    = "sts:AssumeRole"
    }]
  })
}

resource "aws_iam_role_policy_attachment" "ec2_ssm" {
  count      = var.create_ec2_iam ? 1 : 0
  role       = aws_iam_role.ec2[0].name
  policy_arn = "arn:aws:iam::aws:policy/AmazonSSMManagedInstanceCore"
}

resource "aws_iam_role_policy" "ec2_ecr_pull" {
  count = var.create_ec2_iam ? 1 : 0
  name  = "ecr-pull"
  role  = aws_iam_role.ec2[0].id

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Sid      = "EcrAuth"
        Effect   = "Allow"
        Action   = ["ecr:GetAuthorizationToken"]
        Resource = "*"
      },
      {
        Sid    = "EcrPull"
        Effect = "Allow"
        Action = [
          "ecr:BatchCheckLayerAvailability",
          "ecr:GetDownloadUrlForLayer",
          "ecr:BatchGetImage",
          "ecr:DescribeRepositories",
        ]
        Resource = [aws_ecr_repository.app.arn]
      },
    ]
  })
}

resource "aws_iam_instance_profile" "ec2" {
  count = var.create_ec2_iam ? 1 : 0
  name  = "${local.name_prefix}-ec2-profile"
  role  = aws_iam_role.ec2[0].name
}

# -----------------------------------------------------------------------------
# Operator Session Manager (interactive shell) — attach to IAM users/roles
# used from laptop/console. Not for GitHub CD (CD uses SendCommand only).
# -----------------------------------------------------------------------------

resource "aws_iam_policy" "ssm_session_operator" {
  count       = var.create_ssm_session_operator_policy ? 1 : 0
  name        = "${local.name_prefix}-ssm-session-operator"
  description = "Allow Start Session / DescribeSessions for FGS EC2 troubleshooting"

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Sid    = "SsmSessionManager"
        Effect = "Allow"
        Action = [
          "ssm:StartSession",
          "ssm:TerminateSession",
          "ssm:ResumeSession",
          "ssm:DescribeSessions",
          "ssm:GetConnectionStatus",
          "ssm:DescribeInstanceInformation",
        ]
        Resource = "*"
      },
      {
        Sid    = "SsmSessionTargets"
        Effect = "Allow"
        Action = ["ssm:StartSession"]
        Resource = [
          "arn:aws:ssm:${var.aws_region}::document/SSM-SessionManagerRunShell",
          "arn:aws:ec2:${var.aws_region}:${data.aws_caller_identity.current.account_id}:instance/*",
        ]
      },
    ]
  })
}

# -----------------------------------------------------------------------------
# ECS roles
# -----------------------------------------------------------------------------

resource "aws_iam_role" "ecs_execution" {
  name = "${local.name_prefix}-ecs-execution"

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [{
      Effect    = "Allow"
      Principal = { Service = "ecs-tasks.amazonaws.com" }
      Action    = "sts:AssumeRole"
    }]
  })
}

resource "aws_iam_role_policy_attachment" "ecs_execution" {
  role       = aws_iam_role.ecs_execution.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"
}

resource "aws_iam_role_policy" "ecs_execution_secrets" {
  count = length(local.execution_secret_arns) > 0 ? 1 : 0
  name  = "secrets"
  role  = aws_iam_role.ecs_execution.id

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [{
      Effect   = "Allow"
      Action   = ["secretsmanager:GetSecretValue"]
      Resource = local.execution_secret_arns
    }]
  })
}

resource "aws_iam_role" "ecs_task" {
  name = "${local.name_prefix}-ecs-task"

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [{
      Effect    = "Allow"
      Principal = { Service = "ecs-tasks.amazonaws.com" }
      Action    = "sts:AssumeRole"
    }]
  })
}

# Setup creates/reads tenant secrets at runtime. Keep this broad for the first
# slice; tighten Resource to fgs/${var.environment}/* once naming is stable.
resource "aws_iam_role_policy" "ecs_task_secrets" {
  name = "app-secrets"
  role = aws_iam_role.ecs_task.id

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [{
      Effect = "Allow"
      Action = [
        "secretsmanager:GetSecretValue",
        "secretsmanager:CreateSecret",
        "secretsmanager:PutSecretValue",
        "secretsmanager:DescribeSecret",
      ]
      Resource = "*"
    }]
  })
}
