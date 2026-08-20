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
