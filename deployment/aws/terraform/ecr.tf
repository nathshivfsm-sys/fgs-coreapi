resource "aws_ecr_repository" "setup" {
  name                 = "fgs-setup-service"
  image_tag_mutability = "MUTABLE"
  force_delete         = var.environment != "prod"

  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_repository" "user" {
  name                 = "fgs-user-service"
  image_tag_mutability = "MUTABLE"
  force_delete         = var.environment != "prod"

  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_repository" "gateway" {
  name                 = "fgs-gateway"
  image_tag_mutability = "MUTABLE"
  force_delete         = var.environment != "prod"

  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_lifecycle_policy" "setup" {
  repository = aws_ecr_repository.setup.name
  policy     = local.ecr_lifecycle_keep_10
}

resource "aws_ecr_lifecycle_policy" "user" {
  repository = aws_ecr_repository.user.name
  policy     = local.ecr_lifecycle_keep_10
}

resource "aws_ecr_lifecycle_policy" "gateway" {
  repository = aws_ecr_repository.gateway.name
  policy     = local.ecr_lifecycle_keep_10
}
