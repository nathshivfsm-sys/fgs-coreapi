data "aws_availability_zones" "available" {
  state = "available"
}

locals {
  name_prefix = "fgs-${var.environment}"
  namespace   = "fgs-${var.environment}"

  ecr_lifecycle_keep_60 = jsonencode({
    rules = [{
      rulePriority = 1
      description  = "Keep last 60 images (shared repo for setup/user/nginx)"
      selection = {
        tagStatus   = "any"
        countType   = "imageCountMoreThan"
        countNumber = 60
      }
      action = { type = "expire" }
    }]
  })

  vpc_id            = var.create_vpc ? aws_vpc.this[0].id : var.vpc_id
  public_subnet_ids = var.create_vpc ? aws_subnet.public[*].id : var.public_subnet_ids
  vpc_cidr          = var.create_vpc ? aws_vpc.this[0].cidr_block : data.aws_vpc.existing[0].cidr_block

  github_oidc_arn = var.create_github_oidc_provider ? aws_iam_openid_connect_provider.github[0].arn : var.github_oidc_provider_arn

  # Shared ECR repo; service is in the tag (setup-dev, nginx-dev, …).
  setup_image   = "${aws_ecr_repository.app.repository_url}:setup-${var.image_tag}"
  gateway_image = "${aws_ecr_repository.app.repository_url}:nginx-${var.image_tag}"

  redis_connection = var.create_redis_rabbitmq ? "redis:6379" : var.redis_connection_string

  execution_secret_arns = concat(
    aws_secretsmanager_secret.setup_db[*].arn,
    aws_secretsmanager_secret.rabbitmq[*].arn,
  )

  setup_environment = concat(
    [
      { name = "ASPNETCORE_ENVIRONMENT", value = var.aspnetcore_environment },
      { name = "ASPNETCORE_URLS", value = "http://+:5004" },
      { name = "Datadog__Enabled", value = "true" },
      { name = "Datadog__Site", value = "datadoghq.com" },
      { name = "Datadog__Env", value = var.environment },
      { name = "Observability__Enabled", value = "false" },
      { name = "DD_LLMOBS_ENABLED", value = "false" },
      { name = "AwsCredentials__Region", value = var.aws_region },
      { name = "AwsCredentials__EnableLocalProfileFallback", value = "false" },
    ],
    var.kms_key_arn != "" ? [{ name = "AwsCredentials__KmsKeyArn", value = var.kms_key_arn }] : [],
    local.redis_connection != "" ? [
      { name = "Redis__ConnectionString", value = local.redis_connection },
      { name = "Redis__Enabled", value = "true" },
    ] : [],
    var.create_redis_rabbitmq ? [
      { name = "RabbitMq__HostName", value = "rabbitmq" },
      { name = "RabbitMq__Port", value = "5672" },
      { name = "RabbitMq__UserName", value = var.rabbitmq_username },
      { name = "RabbitMq__SslEnabled", value = "false" },
    ] : concat(
      var.rabbitmq_host != "" ? [{ name = "RabbitMq__HostName", value = var.rabbitmq_host }] : [],
      var.rabbitmq_connection_uri != "" ? [{ name = "RabbitMq__ConnectionUri", value = var.rabbitmq_connection_uri }] : [],
    ),
  )

  # Datadog ApiKey comes from Setup glo.GloCredential (Global DATADOG), not ECS secrets.
  # Do not set Datadog__ApiKey / DD_API_KEY here — an env value would override the table.
  setup_secrets = concat(
    var.setup_db_connection_string != "" ? [{
      name      = "ConnectionStrings__FgsSetup"
      valueFrom = aws_secretsmanager_secret.setup_db[0].arn
    }] : [],
    var.create_redis_rabbitmq ? [{
      name      = "RabbitMq__Password"
      valueFrom = aws_secretsmanager_secret.rabbitmq[0].arn
    }] : [],
  )

  # Dockerfile.prod listens 443 with certs and redirects :80 → https.
  # ALB terminates TLS and forwards HTTP, so rewrite site.conf at start.
  # Stub unused upstreams so nginx can start before User/BFF/etc. exist.
  gateway_start = <<-EOT
    set -eu
    cat > /etc/nginx/conf.d/includes/upstreams.prod.conf << 'UP'
    upstream setup_service {
      least_conn;
      server setup-service:5004 max_fails=3 fail_timeout=10s;
      keepalive 32;
    }
    upstream user_service { server 127.0.0.1:9; }
    upstream notification_service { server 127.0.0.1:9; }
    upstream bff_service { server 127.0.0.1:9; }
    upstream file_service { server 127.0.0.1:9; }
    upstream audit_service { server 127.0.0.1:9; }
    upstream inventory_service { server 127.0.0.1:9; }
    upstream asset_service { server 127.0.0.1:9; }
    upstream crm_service { server 127.0.0.1:9; }
    upstream scheduling_service { server 127.0.0.1:9; }
    upstream billing_service { server 127.0.0.1:9; }
    upstream service_agreement_service { server 127.0.0.1:9; }
    UP
    cat > /etc/nginx/conf.d/site.conf << 'SITE'
    include /etc/nginx/conf.d/includes/upstreams.prod.conf;
    server {
      listen 80 default_server;
      server_name _;
      location = /nginx-health {
        access_log off;
        return 200 "healthy\n";
        add_header Content-Type text/plain;
      }
      include /etc/nginx/conf.d/includes/api-v1-routes.prod.conf;
    }
    SITE
    nginx -g 'daemon off;'
  EOT
}
