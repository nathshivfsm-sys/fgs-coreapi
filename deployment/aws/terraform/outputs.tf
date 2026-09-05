output "cluster_name" {
  value = aws_ecs_cluster.this.name
}

output "service_connect_namespace" {
  value = aws_service_discovery_http_namespace.this.name
}

output "alb_dns_name" {
  value       = var.enable_alb ? aws_lb.gateway[0].dns_name : null
  description = "Set when enable_alb is true (dev default)."
}

output "alb_url" {
  value = var.enable_alb ? (var.acm_certificate_arn != "" ? "https://${aws_lb.gateway[0].dns_name}" : "http://${aws_lb.gateway[0].dns_name}") : "http://<nginx-task-public-ip>/nginx-health"
}

output "url_hint" {
  value = var.enable_alb ? "Open alb_url + /nginx-health" : "ECS → fgs-dev → gateway → task → Public IP → http://IP/nginx-health"
}

output "redis_endpoint" {
  value       = var.create_redis_rabbitmq ? "redis:6379" : var.redis_connection_string
  description = "Set Redis__ConnectionString on Setup and User."
}

output "rabbitmq_hostname" {
  value       = var.create_redis_rabbitmq ? "rabbitmq" : var.rabbitmq_host
  description = "Set RabbitMq__HostName on apps. Password is in Secrets Manager, not this output."
}

output "rabbitmq_secret_arn" {
  value       = var.create_redis_rabbitmq ? aws_secretsmanager_secret.rabbitmq[0].arn : null
  description = "Plaintext password for RABBITMQ_DEFAULT_PASS / RabbitMq__Password."
}

output "ecr_repository_url" {
  value       = aws_ecr_repository.app.repository_url
  description = "Shared ECR repo. Tags: setup-<channel>, user-<channel>, nginx-<channel>."
}

output "github_actions_role_arn" {
  value       = aws_iam_role.github_actions.arn
  description = "Set GitHub Actions variable AWS_ROLE_TO_ASSUME to this ARN."
}

output "github_actions_vars" {
  description = "GitHub repository variables to set after apply."
  value = {
    AWS_REGION         = var.aws_region
    AWS_ROLE_TO_ASSUME = aws_iam_role.github_actions.arn
    ECR_REPO           = aws_ecr_repository.app.name
  }
}

output "ec2_instance_role_name" {
  value       = var.create_ec2_iam ? aws_iam_role.ec2[0].name : null
  description = "Attach this IAM role (via instance profile) to the FGS EC2 host."
}

output "ec2_instance_profile_name" {
  value       = var.create_ec2_iam ? aws_iam_instance_profile.ec2[0].name : null
  description = "EC2 instance profile name (SSM + ECR pull)."
}

output "ec2_instance_profile_arn" {
  value       = var.create_ec2_iam ? aws_iam_instance_profile.ec2[0].arn : null
  description = "EC2 instance profile ARN."
}

output "ssm_session_operator_policy_arn" {
  value       = var.create_ssm_session_operator_policy ? aws_iam_policy.ssm_session_operator[0].arn : null
  description = "Attach to IAM users who run aws ssm start-session / Session Manager console."
}

output "github_actions_user_name" {
  value       = var.create_github_actions_user ? aws_iam_user.github_actions[0].name : null
  description = "IAM user for access-key CI/CD when create_github_actions_user is true."
}
