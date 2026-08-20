resource "aws_ecs_task_definition" "redis" {
  count                    = var.create_redis_rabbitmq ? 1 : 0
  family                   = "${local.name_prefix}-redis"
  requires_compatibilities = ["FARGATE"]
  network_mode             = "awsvpc"
  cpu                      = var.redis_cpu
  memory                   = var.redis_memory
  execution_role_arn       = aws_iam_role.ecs_execution.arn

  runtime_platform {
    operating_system_family = "LINUX"
    cpu_architecture        = "X86_64"
  }

  container_definitions = jsonencode([{
    name      = "redis"
    image     = var.redis_image
    essential = true
    portMappings = [{
      name          = "redis-tcp"
      containerPort = 6379
      hostPort      = 6379
      protocol      = "tcp"
    }]
    healthCheck = {
      command     = ["CMD-SHELL", "redis-cli ping || exit 1"]
      interval    = 15
      timeout     = 5
      retries     = 5
      startPeriod = 10
    }
    logConfiguration = {
      logDriver = "awslogs"
      options = {
        awslogs-group         = aws_cloudwatch_log_group.redis[0].name
        awslogs-region        = var.aws_region
        awslogs-stream-prefix = "redis"
      }
    }
  }])
}

resource "aws_ecs_service" "redis" {
  count                              = var.create_redis_rabbitmq ? 1 : 0
  name                               = "redis"
  cluster                            = aws_ecs_cluster.this.id
  task_definition                    = aws_ecs_task_definition.redis[0].arn
  desired_count                      = var.desired_count
  platform_version                   = "LATEST"
  deployment_minimum_healthy_percent = 0
  deployment_maximum_percent         = 200

  capacity_provider_strategy {
    capacity_provider = var.use_fargate_spot ? "FARGATE_SPOT" : "FARGATE"
    weight            = 1
    base              = 0
  }

  network_configuration {
    subnets          = local.public_subnet_ids
    security_groups  = [aws_security_group.redis[0].id]
    assign_public_ip = true
  }

  service_connect_configuration {
    enabled   = true
    namespace = aws_service_discovery_http_namespace.this.arn

    service {
      port_name      = "redis-tcp"
      discovery_name = "redis"
      client_alias {
        dns_name = "redis"
        port     = 6379
      }
    }
  }

  depends_on = [aws_ecs_cluster_capacity_providers.this]

  lifecycle {
    ignore_changes = [desired_count]
  }
}

resource "aws_ecs_task_definition" "rabbitmq" {
  count                    = var.create_redis_rabbitmq ? 1 : 0
  family                   = "${local.name_prefix}-rabbitmq"
  requires_compatibilities = ["FARGATE"]
  network_mode             = "awsvpc"
  cpu                      = var.rabbitmq_cpu
  memory                   = var.rabbitmq_memory
  execution_role_arn       = aws_iam_role.ecs_execution.arn

  runtime_platform {
    operating_system_family = "LINUX"
    cpu_architecture        = "X86_64"
  }

  container_definitions = jsonencode([{
    name      = "rabbitmq"
    image     = var.rabbitmq_image
    essential = true
    portMappings = [
      {
        name          = "amqp"
        containerPort = 5672
        hostPort      = 5672
        protocol      = "tcp"
      },
      {
        name          = "management"
        containerPort = 15672
        hostPort      = 15672
        protocol      = "tcp"
      }
    ]
    environment = [
      { name = "RABBITMQ_DEFAULT_USER", value = var.rabbitmq_username }
    ]
    secrets = [{
      name      = "RABBITMQ_DEFAULT_PASS"
      valueFrom = aws_secretsmanager_secret.rabbitmq[0].arn
    }]
    healthCheck = {
      command     = ["CMD-SHELL", "rabbitmq-diagnostics -q ping || exit 1"]
      interval    = 30
      timeout     = 10
      retries     = 6
      startPeriod = 60
    }
    logConfiguration = {
      logDriver = "awslogs"
      options = {
        awslogs-group         = aws_cloudwatch_log_group.rabbitmq[0].name
        awslogs-region        = var.aws_region
        awslogs-stream-prefix = "rabbitmq"
      }
    }
  }])
}

resource "aws_ecs_service" "rabbitmq" {
  count                              = var.create_redis_rabbitmq ? 1 : 0
  name                               = "rabbitmq"
  cluster                            = aws_ecs_cluster.this.id
  task_definition                    = aws_ecs_task_definition.rabbitmq[0].arn
  desired_count                      = var.desired_count
  platform_version                   = "LATEST"
  deployment_minimum_healthy_percent = 0
  deployment_maximum_percent         = 200

  capacity_provider_strategy {
    capacity_provider = var.use_fargate_spot ? "FARGATE_SPOT" : "FARGATE"
    weight            = 1
    base              = 0
  }

  network_configuration {
    subnets          = local.public_subnet_ids
    security_groups  = [aws_security_group.rabbitmq[0].id]
    assign_public_ip = true
  }

  service_connect_configuration {
    enabled   = true
    namespace = aws_service_discovery_http_namespace.this.arn

    service {
      port_name      = "amqp"
      discovery_name = "rabbitmq"
      client_alias {
        dns_name = "rabbitmq"
        port     = 5672
      }
    }
  }

  depends_on = [
    aws_ecs_cluster_capacity_providers.this,
    aws_iam_role_policy.ecs_execution_secrets,
  ]

  lifecycle {
    ignore_changes = [desired_count]
  }
}
