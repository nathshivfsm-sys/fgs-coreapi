resource "aws_service_discovery_http_namespace" "this" {
  name        = local.namespace
  description = "ECS Service Connect for ${local.name_prefix}"
}

resource "aws_ecs_cluster" "this" {
  name = local.name_prefix

  setting {
    name  = "containerInsights"
    value = var.enable_container_insights ? "enabled" : "disabled"
  }

  service_connect_defaults {
    namespace = aws_service_discovery_http_namespace.this.arn
  }
}

resource "aws_ecs_cluster_capacity_providers" "this" {
  cluster_name = aws_ecs_cluster.this.name

  capacity_providers = ["FARGATE", "FARGATE_SPOT"]

  default_capacity_provider_strategy {
    capacity_provider = var.use_fargate_spot ? "FARGATE_SPOT" : "FARGATE"
    weight            = 1
    base              = var.use_fargate_spot ? 0 : 1
  }
}

resource "aws_ecs_task_definition" "setup" {
  family                   = "${local.name_prefix}-setup"
  requires_compatibilities = ["FARGATE"]
  network_mode             = "awsvpc"
  cpu                      = var.setup_cpu
  memory                   = var.setup_memory
  execution_role_arn       = aws_iam_role.ecs_execution.arn
  task_role_arn            = aws_iam_role.ecs_task.arn

  runtime_platform {
    operating_system_family = "LINUX"
    cpu_architecture        = "X86_64"
  }

  container_definitions = jsonencode([
    {
      name      = "setup"
      image     = local.setup_image
      essential = true
      portMappings = [{
        name          = "setup-http"
        containerPort = 5004
        hostPort      = 5004
        protocol      = "tcp"
        appProtocol   = "http"
      }]
      environment = local.setup_environment
      secrets     = local.setup_secrets
      healthCheck = {
        command     = ["CMD-SHELL", "curl -fsS http://localhost:5004/health || exit 1"]
        interval    = 30
        timeout     = 5
        retries     = 3
        startPeriod = 60
      }
      logConfiguration = {
        logDriver = "awslogs"
        options = {
          awslogs-group         = aws_cloudwatch_log_group.setup.name
          awslogs-region        = var.aws_region
          awslogs-stream-prefix = "setup"
        }
      }
    }
  ])
}

resource "aws_ecs_service" "setup" {
  count                              = var.create_ecs_services ? 1 : 0
  name                               = "setup"
  cluster                            = aws_ecs_cluster.this.id
  task_definition                    = aws_ecs_task_definition.setup.arn
  desired_count                      = var.desired_count
  platform_version                   = "LATEST"
  enable_execute_command             = false
  deployment_minimum_healthy_percent = 0
  deployment_maximum_percent         = 200

  capacity_provider_strategy {
    capacity_provider = var.use_fargate_spot ? "FARGATE_SPOT" : "FARGATE"
    weight            = 1
    base              = 0
  }

  network_configuration {
    subnets          = local.public_subnet_ids
    security_groups  = [aws_security_group.setup.id]
    assign_public_ip = true
  }

  service_connect_configuration {
    enabled   = true
    namespace = aws_service_discovery_http_namespace.this.arn

    service {
      port_name      = "setup-http"
      discovery_name = "setup-service"
      client_alias {
        dns_name = "setup-service"
        port     = 5004
      }
    }
  }

  lifecycle {
    ignore_changes = [desired_count]
  }

  depends_on = [
    aws_ecs_cluster_capacity_providers.this,
    aws_ecs_service.redis,
    aws_ecs_service.rabbitmq,
  ]
}

resource "aws_ecs_task_definition" "gateway" {
  family                   = "${local.name_prefix}-gateway"
  requires_compatibilities = ["FARGATE"]
  network_mode             = "awsvpc"
  cpu                      = var.gateway_cpu
  memory                   = var.gateway_memory
  execution_role_arn       = aws_iam_role.ecs_execution.arn
  task_role_arn            = aws_iam_role.ecs_task.arn

  runtime_platform {
    operating_system_family = "LINUX"
    cpu_architecture        = "X86_64"
  }

  container_definitions = jsonencode([
    merge(
      {
        name      = "gateway"
        image     = local.gateway_image
        essential = true
        portMappings = [{
          name          = "nginx-http"
          containerPort = 80
          hostPort      = 80
          protocol      = "tcp"
          appProtocol   = "http"
        }]
        healthCheck = {
          command     = ["CMD-SHELL", "curl -fsS http://localhost/nginx-health || exit 1"]
          interval    = 30
          timeout     = 5
          retries     = 3
          startPeriod = 20
        }
        logConfiguration = {
          logDriver = "awslogs"
          options = {
            awslogs-group         = aws_cloudwatch_log_group.gateway.name
            awslogs-region        = var.aws_region
            awslogs-stream-prefix = "gateway"
          }
        }
      },
      var.gateway_http_behind_alb ? { command = ["sh", "-c", local.gateway_start] } : {}
    )
  ])
}

resource "aws_ecs_service" "gateway" {
  count                              = var.create_ecs_services ? 1 : 0
  name                               = "gateway"
  cluster                            = aws_ecs_cluster.this.id
  task_definition                    = aws_ecs_task_definition.gateway.arn
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
    security_groups  = [aws_security_group.gateway.id]
    assign_public_ip = true
  }

  dynamic "load_balancer" {
    for_each = var.enable_alb ? [1] : []
    content {
      target_group_arn = aws_lb_target_group.gateway[0].arn
      container_name   = "gateway"
      container_port   = 80
    }
  }

  service_connect_configuration {
    enabled   = true
    namespace = aws_service_discovery_http_namespace.this.arn
  }

  depends_on = [
    aws_lb_listener.http_forward,
    aws_lb_listener.http_redirect,
    aws_lb_listener.https,
    aws_ecs_cluster_capacity_providers.this,
    aws_ecs_service.setup,
  ]

  lifecycle {
    ignore_changes = [desired_count]
  }
}
