#!/usr/bin/env bash
# Pull one service image from ECR and recreate the container.
# Usage: deploy-service.sh <compose-service> <channel> [ecr-repo] [aws-region]
# Example: deploy-service.sh setup-service dev fgs/dockers us-east-1

set -euo pipefail

COMPOSE_SERVICE="${1:?compose service (setup-service, user-service, nginx)}"
CHANNEL="${2:?channel (dev, test, prod)}"
ECR_REPO="${3:-fgs/dockers}"
AWS_REGION="${4:-us-east-1}"

COMPOSE_DIR="${FGS_COMPOSE_DIR:-/opt/fgs}"
COMPOSE_FILE="${FGS_COMPOSE_FILE:-docker-compose.ec2.yml}"

cd "$COMPOSE_DIR"

if [ ! -f "$COMPOSE_FILE" ]; then
  echo "Missing $COMPOSE_DIR/$COMPOSE_FILE — run bootstrap-ec2.sh first." >&2
  exit 1
fi

REGISTRY=$(aws ecr describe-repositories \
  --repository-names "$ECR_REPO" \
  --region "$AWS_REGION" \
  --query 'repositories[0].repositoryUri' \
  --output text | sed 's|/.*||')

echo "Logging in to ECR registry $REGISTRY"
aws ecr get-login-password --region "$AWS_REGION" \
  | docker login --username AWS --password-stdin "$REGISTRY"

ENV_FILE="$COMPOSE_DIR/.env"
touch "$ENV_FILE"

upsert_env() {
  local key="$1"
  local val="$2"
  if grep -q "^${key}=" "$ENV_FILE" 2>/dev/null; then
    sed -i "s|^${key}=.*|${key}=${val}|" "$ENV_FILE"
  else
    echo "${key}=${val}" >> "$ENV_FILE"
  fi
}

upsert_env FGS_CHANNEL "$CHANNEL"
upsert_env FGS_ECR_REGISTRY "$REGISTRY"
upsert_env FGS_ECR_REPO "$ECR_REPO"
upsert_env FGS_SETUP_IMAGE "${REGISTRY}/${ECR_REPO}:setup-${CHANNEL}"
upsert_env FGS_USER_IMAGE "${REGISTRY}/${ECR_REPO}:user-${CHANNEL}"
upsert_env FGS_NGINX_IMAGE "${REGISTRY}/${ECR_REPO}:nginx-${CHANNEL}"

echo "Deploying ${COMPOSE_SERVICE} (channel ${CHANNEL})"
docker compose -f "$COMPOSE_FILE" pull "$COMPOSE_SERVICE"
docker compose -f "$COMPOSE_FILE" up -d --no-deps "$COMPOSE_SERVICE"
docker compose -f "$COMPOSE_FILE" ps "$COMPOSE_SERVICE"

echo "Done: ${COMPOSE_SERVICE} updated to channel ${CHANNEL}"
