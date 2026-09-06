#!/usr/bin/env bash
# One-time EC2 host setup for FGS docker-compose deployment.
# Run as root on Amazon Linux 2023 / Ubuntu 22.04+.
#
# Prerequisites:
#   - EC2 instance profile with AmazonSSMManagedInstanceCore + ECR read
#   - Security group: inbound 80/443 from your clients (or ALB if used as TCP/passthrough)
#
# Usage:
#   curl -fsSL <raw-url>/bootstrap-ec2.sh | sudo bash
#   # or copy deployment/aws/ec2/* to the instance and run locally

set -euo pipefail

FGS_DIR="${FGS_COMPOSE_DIR:-/opt/fgs}"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

echo "==> Installing Docker"
if command -v apt-get >/dev/null 2>&1; then
  apt-get update -y
  apt-get install -y ca-certificates curl gnupg awscli
  install -m 0755 -d /etc/apt/keyrings
  curl -fsSL https://download.docker.com/linux/ubuntu/gpg | gpg --dearmor -o /etc/apt/keyrings/docker.gpg
  chmod a+r /etc/apt/keyrings/docker.gpg
  echo "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/ubuntu $(. /etc/os-release && echo "$VERSION_CODENAME") stable" \
    > /etc/apt/sources.list.d/docker.list
  apt-get update -y
  apt-get install -y docker-ce docker-ce-cli containerd.io docker-compose-plugin
elif command -v dnf >/dev/null 2>&1; then
  dnf install -y docker aws-cli
  systemctl enable --now docker
  mkdir -p /usr/local/lib/docker/cli-plugins
  curl -SL "https://github.com/docker/compose/releases/download/v2.32.4/docker-compose-linux-$(uname -m)" \
    -o /usr/local/lib/docker/cli-plugins/docker-compose
  chmod +x /usr/local/lib/docker/cli-plugins/docker-compose
else
  echo "Unsupported OS — install Docker and docker compose manually." >&2
  exit 1
fi

systemctl enable --now docker 2>/dev/null || true

echo "==> Creating $FGS_DIR"
mkdir -p "$FGS_DIR/config" "$FGS_DIR/certs"
install -m 0755 "$SCRIPT_DIR/deploy-service.sh" "$FGS_DIR/deploy-service.sh"
# EC2 nginx entrypoint: TLS on nginx for api-dev.fieldwhizey.com (+ Swagger for setup/user).
install -m 0755 "$SCRIPT_DIR/nginx-https-entrypoint.sh" "$FGS_DIR/nginx-https-entrypoint.sh"
# Keep HTTP-only entrypoint available for ALB-terminated setups.
if [ -f "$SCRIPT_DIR/nginx-http-only-entrypoint.sh" ]; then
  install -m 0755 "$SCRIPT_DIR/nginx-http-only-entrypoint.sh" "$FGS_DIR/nginx-http-only-entrypoint.sh"
fi
install -m 0644 "$SCRIPT_DIR/docker-compose.ec2.yml" "$FGS_DIR/docker-compose.ec2.yml"

if [ ! -f "$FGS_DIR/config/setup-appsettings.json" ]; then
  cat > "$FGS_DIR/config/setup-appsettings.json" << 'JSON'
{
  "ConnectionStrings": {
    "FgsSetup": "REPLACE_WITH_YOUR_RDS_CONNECTION_STRING"
  }
}
JSON
  echo "Wrote placeholder $FGS_DIR/config/setup-appsettings.json — FgsSetup only; other credentials live in glo.GloCredential."
fi

if [ ! -f "$FGS_DIR/.env" ]; then
  cat > "$FGS_DIR/.env" << 'ENV'
FGS_CONFIG_DIR=/opt/fgs/config
ASPNETCORE_ENVIRONMENT=Development
# RabbitMQ container boot only — must match glo.GloCredential Global:RABBITMQ Username/Password.
RABBITMQ_USER=fgs
RABBITMQ_PASSWORD=CHANGE_ME_STRONG_PASSWORD
CREDENTIAL_DISTRIBUTION_KEY=fgs-internal-credential-distribution-key
FGS_CHANNEL=dev
DD_ENV=dev
DD_SITE=datadoghq.com
ENV
  echo "Wrote $FGS_DIR/.env — set RABBITMQ_PASSWORD to match GloCredential RABBITMQ before first deploy."
fi

echo ""
echo "Bootstrap complete."
echo "Next steps:"
echo "  1. Edit $FGS_DIR/config/setup-appsettings.json — FgsSetup RDS connection string only"
echo "  2. Ensure glo.GloCredential has Global:DATABASE (FgsUser, FgsAudit, FgsNotification, FgsSetup, …),"
echo "     Global:REDIS, Global:RABBITMQ, Global:SENDGRID, Global:ENTRA_EXTERNAL_ID, Global:DATADOG, etc."
echo "  3. Ensure RABBITMQ_USER/PASSWORD in $FGS_DIR/.env matches GloCredential RABBITMQ (broker boot)"
echo "  4. Edit $FGS_DIR/.env (ASPNETCORE_ENVIRONMENT if needed)"
echo "  5. Place TLS files at $FGS_DIR/certs/tls.crt and $FGS_DIR/certs/tls.key (wildcard *.fieldwhizey.com)"
echo "  6. Set GitHub repository variable EC2_INSTANCE_ID to this instance ID: $(curl -s http://169.254.169.254/latest/meta-data/instance-id 2>/dev/null || echo '<instance-id>')"
echo "  7. Merge to dev — CI pushes ECR image and CD runs deploy-service.sh via SSM"
echo "  8. First full stack (after images in ECR):"
echo "     redis → rabbitmq → setup → audit → user → bff → notification → file → consumer → nginx"
echo "     sudo $FGS_DIR/deploy-service.sh redis dev"
echo "     sudo $FGS_DIR/deploy-service.sh rabbitmq dev"
echo "     sudo $FGS_DIR/deploy-service.sh setup-service dev"
echo "     sudo $FGS_DIR/deploy-service.sh audit-service dev"
echo "     sudo $FGS_DIR/deploy-service.sh user-service dev"
echo "     sudo $FGS_DIR/deploy-service.sh bff-service dev"
echo "     sudo $FGS_DIR/deploy-service.sh notification-service dev"
echo "     sudo $FGS_DIR/deploy-service.sh file-service dev"
echo "     sudo $FGS_DIR/deploy-service.sh consumer-service dev"
echo "     sudo $FGS_DIR/deploy-service.sh nginx dev"
