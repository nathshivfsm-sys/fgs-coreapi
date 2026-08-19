resource "aws_cloudwatch_log_group" "setup" {
  name              = "/ecs/${local.name_prefix}/setup"
  retention_in_days = var.log_retention_days
}

resource "aws_cloudwatch_log_group" "gateway" {
  name              = "/ecs/${local.name_prefix}/gateway"
  retention_in_days = var.log_retention_days
}

resource "aws_cloudwatch_log_group" "redis" {
  count             = var.create_redis_rabbitmq ? 1 : 0
  name              = "/ecs/${local.name_prefix}/redis"
  retention_in_days = var.log_retention_days
}

resource "aws_cloudwatch_log_group" "rabbitmq" {
  count             = var.create_redis_rabbitmq ? 1 : 0
  name              = "/ecs/${local.name_prefix}/rabbitmq"
  retention_in_days = var.log_retention_days
}
