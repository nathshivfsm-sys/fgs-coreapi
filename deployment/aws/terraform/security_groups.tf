resource "aws_security_group" "alb" {
  count       = var.enable_alb ? 1 : 0
  name        = "${local.name_prefix}-alb"
  description = "ALB in front of nginx"
  vpc_id      = local.vpc_id

  ingress {
    description = "HTTP"
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    description = "HTTPS"
    from_port   = 443
    to_port     = 443
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

resource "aws_security_group" "gateway" {
  name        = "${local.name_prefix}-gateway"
  description = "Nginx Fargate tasks"
  vpc_id      = local.vpc_id

  dynamic "ingress" {
    for_each = var.enable_alb ? [1] : []
    content {
      description     = "HTTP from ALB"
      from_port       = 80
      to_port         = 80
      protocol        = "tcp"
      security_groups = [aws_security_group.alb[0].id]
    }
  }

  dynamic "ingress" {
    for_each = var.enable_alb ? [] : [1]
    content {
      description = "HTTP to nginx when ALB is disabled"
      from_port   = 80
      to_port     = 80
      protocol    = "tcp"
      cidr_blocks = var.dev_ingress_cidrs
    }
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

resource "aws_security_group" "setup" {
  name        = "${local.name_prefix}-setup"
  description = "Setup API Fargate tasks"
  vpc_id      = local.vpc_id

  ingress {
    description = "Setup HTTP from VPC (Service Connect + nginx)"
    from_port   = 5004
    to_port     = 5004
    protocol    = "tcp"
    cidr_blocks = [local.vpc_cidr]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

resource "aws_security_group" "redis" {
  count       = var.create_redis_rabbitmq ? 1 : 0
  name        = "${local.name_prefix}-redis"
  description = "Redis Fargate tasks"
  vpc_id      = local.vpc_id

  ingress {
    description = "Redis from VPC (Service Connect)"
    from_port   = 6379
    to_port     = 6379
    protocol    = "tcp"
    cidr_blocks = [local.vpc_cidr]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

resource "aws_security_group" "rabbitmq" {
  count       = var.create_redis_rabbitmq ? 1 : 0
  name        = "${local.name_prefix}-rabbitmq"
  description = "RabbitMQ Fargate tasks"
  vpc_id      = local.vpc_id

  ingress {
    description = "AMQP from VPC (Service Connect)"
    from_port   = 5672
    to_port     = 5672
    protocol    = "tcp"
    cidr_blocks = [local.vpc_cidr]
  }

  ingress {
    description = "Management UI from VPC only (not on the ALB)"
    from_port   = 15672
    to_port     = 15672
    protocol    = "tcp"
    cidr_blocks = [local.vpc_cidr]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}
