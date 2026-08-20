resource "aws_lb" "gateway" {
  count              = var.enable_alb ? 1 : 0
  name               = "${local.name_prefix}-gw"
  load_balancer_type = "application"
  internal           = false
  security_groups    = [aws_security_group.alb[0].id]
  subnets            = local.public_subnet_ids
}

resource "aws_lb_target_group" "gateway" {
  count       = var.enable_alb ? 1 : 0
  name        = "${local.name_prefix}-gw"
  port        = 80
  protocol    = "HTTP"
  vpc_id      = local.vpc_id
  target_type = "ip"

  health_check {
    enabled             = true
    path                = "/nginx-health"
    matcher             = "200"
    interval            = 30
    timeout             = 5
    healthy_threshold   = 2
    unhealthy_threshold = 3
  }
}

resource "aws_lb_listener" "http_forward" {
  count             = var.enable_alb && var.acm_certificate_arn == "" ? 1 : 0
  load_balancer_arn = aws_lb.gateway[0].arn
  port              = 80
  protocol          = "HTTP"

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.gateway[0].arn
  }
}

resource "aws_lb_listener" "http_redirect" {
  count             = var.enable_alb && var.acm_certificate_arn != "" ? 1 : 0
  load_balancer_arn = aws_lb.gateway[0].arn
  port              = 80
  protocol          = "HTTP"

  default_action {
    type = "redirect"
    redirect {
      port        = "443"
      protocol    = "HTTPS"
      status_code = "HTTP_301"
    }
  }
}

resource "aws_lb_listener" "https" {
  count             = var.enable_alb && var.acm_certificate_arn != "" ? 1 : 0
  load_balancer_arn = aws_lb.gateway[0].arn
  port              = 443
  protocol          = "HTTPS"
  ssl_policy        = "ELBSecurityPolicy-TLS13-1-2-2021-06"
  certificate_arn   = var.acm_certificate_arn

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.gateway[0].arn
  }
}
