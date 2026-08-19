variable "aws_region" {
  type        = string
  description = "AWS region for this stack."
  default     = "us-east-1"
}

variable "environment" {
  type        = string
  description = "Short env name. Used in resource names and Service Connect namespace fgs-<env> (dev → fgs-dev)."
  default     = "dev"
}

variable "github_org" {
  type        = string
  description = "GitHub org or user that owns fgs-coreapi (OIDC)."
  default     = "nathshivfsm-sys"
}

variable "github_repo" {
  type        = string
  description = "GitHub repository name (no org prefix)."
  default     = "fgs-coreapi"
}

variable "create_github_oidc_provider" {
  type        = bool
  description = "Create the GitHub OIDC provider. Set false if token.actions.githubusercontent.com already exists in the account."
  default     = true
}

variable "github_oidc_provider_arn" {
  type        = string
  description = "Existing GitHub OIDC provider ARN when create_github_oidc_provider is false."
  default     = ""
}

variable "image_tag" {
  type        = string
  description = "ECR image tag to run (build pipeline channel: dev, test, or prod)."
  default     = "dev"
}

variable "desired_count" {
  type        = number
  description = "Desired Fargate tasks per service."
  default     = 1
}

variable "create_vpc" {
  type        = bool
  description = "If true, create a small public VPC. If false, use vpc_id and subnet_ids."
  default     = true
}

variable "vpc_id" {
  type        = string
  description = "Existing VPC id when create_vpc is false."
  default     = ""
}

variable "public_subnet_ids" {
  type        = list(string)
  description = "Existing public subnet ids (min 2 AZs) when create_vpc is false."
  default     = []
}

variable "enable_alb" {
  type        = bool
  description = "Create an internet-facing ALB so the environment has a stable URL."
  default     = true
}

variable "use_fargate_spot" {
  type        = bool
  description = "Fargate Spot is cheaper but can interrupt tasks. Default false (On-Demand)."
  default     = false
}

variable "enable_container_insights" {
  type        = bool
  description = "Container Insights adds CloudWatch cost. Leave false for this stack unless you need it."
  default     = false
}

variable "log_retention_days" {
  type        = number
  description = "CloudWatch log retention."
  default     = 14
}

variable "dev_ingress_cidrs" {
  type        = list(string)
  description = "CIDRs allowed to hit nginx :80 when enable_alb is false."
  default     = ["0.0.0.0/0"]
}

variable "setup_cpu" {
  type        = string
  default     = "512"
}

variable "setup_memory" {
  type        = string
  default     = "1024"
}

variable "gateway_cpu" {
  type        = string
  default     = "256"
}

variable "gateway_memory" {
  type        = string
  default     = "512"
}

variable "acm_certificate_arn" {
  type        = string
  description = "Optional ACM cert ARN for HTTPS on the ALB. Empty = HTTP listener only."
  default     = ""
}

variable "aspnetcore_environment" {
  type        = string
  description = "ASPNETCORE_ENVIRONMENT for Setup (not baked into the image)."
  default     = "Development"
}

variable "create_redis_rabbitmq" {
  type        = bool
  description = "Create Redis and RabbitMQ as Fargate services in the Service Connect namespace (redis:6379, rabbitmq:5672). Set false to use existing hosted endpoints instead."
  default     = true
}

variable "redis_image" {
  type        = string
  description = "Redis image. Default is Amazon ECR Public (avoids Docker Hub rate limits)."
  default     = "public.ecr.aws/docker/library/redis:7-alpine"
}

variable "rabbitmq_image" {
  type        = string
  description = "RabbitMQ image (management tag includes rabbitmq-diagnostics for health checks)."
  default     = "public.ecr.aws/docker/library/rabbitmq:4-management-alpine"
}

variable "redis_cpu" {
  type    = string
  default = "256"
}

variable "redis_memory" {
  type    = string
  default = "512"
}

variable "rabbitmq_cpu" {
  type    = string
  default = "512"
}

variable "rabbitmq_memory" {
  type    = string
  default = "1024"
}

variable "rabbitmq_username" {
  type        = string
  description = "RabbitMQ default user when create_redis_rabbitmq is true."
  default     = "fgs"
}

variable "redis_connection_string" {
  type        = string
  description = "Used only when create_redis_rabbitmq is false. Example: mycache.cache.amazonaws.com:6379."
  default     = ""
  sensitive   = true
}

variable "rabbitmq_host" {
  type        = string
  description = "Used only when create_redis_rabbitmq is false."
  default     = ""
}

variable "rabbitmq_connection_uri" {
  type        = string
  description = "Optional amqps:// URI when using Amazon MQ (create_redis_rabbitmq = false)."
  default     = ""
  sensitive   = true
}

variable "setup_db_connection_string" {
  type        = string
  description = "Optional ConnectionStrings__FgsSetup for first boot. Prefer Secrets Manager / Setup vault later."
  default     = ""
  sensitive   = true
}

variable "gateway_http_behind_alb" {
  type        = bool
  description = "Rewrite nginx at start so APIs are served on :80 (ALB terminates TLS). Unused upstreams stub to 127.0.0.1 so nginx can boot before other services exist."
  default     = true
}

variable "kms_key_arn" {
  type        = string
  description = "Optional CMK ARN Setup uses for credential vault. Empty = leave app default."
  default     = ""
}

variable "create_ecs_services" {
  type        = bool
  description = "Create Setup + nginx Fargate services. Set false for ECR/IAM/VPC-only first apply, then push images and set true. Redis/RabbitMQ use create_redis_rabbitmq."
  default     = false
}
