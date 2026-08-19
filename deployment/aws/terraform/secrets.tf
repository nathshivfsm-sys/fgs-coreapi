resource "aws_secretsmanager_secret" "setup_db" {
  count                   = var.setup_db_connection_string != "" ? 1 : 0
  name                    = "fgs/${var.environment}/setup/db"
  recovery_window_in_days = var.environment == "prod" ? 7 : 0
}

resource "aws_secretsmanager_secret_version" "setup_db" {
  count         = var.setup_db_connection_string != "" ? 1 : 0
  secret_id     = aws_secretsmanager_secret.setup_db[0].id
  secret_string = var.setup_db_connection_string

  lifecycle {
    ignore_changes = [secret_string]
  }
}

resource "random_password" "rabbitmq" {
  count   = var.create_redis_rabbitmq ? 1 : 0
  length  = 24
  special = false
}

resource "aws_secretsmanager_secret" "rabbitmq" {
  count                   = var.create_redis_rabbitmq ? 1 : 0
  name                    = "fgs/${var.environment}/rabbitmq"
  recovery_window_in_days = var.environment == "prod" ? 7 : 0
}

resource "aws_secretsmanager_secret_version" "rabbitmq" {
  count         = var.create_redis_rabbitmq ? 1 : 0
  secret_id     = aws_secretsmanager_secret.rabbitmq[0].id
  secret_string = random_password.rabbitmq[0].result
}
