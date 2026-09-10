# FGS DEV — Clone EC2 Host (Interactive SSM Session)

**Audience:** operators standing up the same FGS **dev** stack on a **new EC2** using **interactive SSM Session Manager** only.  
**Region:** `us-east-1`  
**Repo:** `https://github.com/nathshivfsm-sys/fgs-coreapi.git`  
**Branch:** `dev`  
**Compose path on host:** `/opt/fgs`  
**ECR:** `fgs/dockers` · **Channel:** `dev`

Related guides:

| Doc | When to use |
| --- | --- |
| [FGS_EC2_SSM_MANUAL_DEPLOY_RUNBOOK.md](FGS_EC2_SSM_MANUAL_DEPLOY_RUNBOOK.md) | Full SSM runbook (IAM, send-command, troubleshooting) |
| [EC2_FULL_SETUP_AND_CD.md](EC2_FULL_SETUP_AND_CD.md) | First-time EC2 + CD |
| [GITHUB_ACTIONS_CD_EC2.md](GITHUB_ACTIONS_CD_EC2.md) | GitHub Actions → SSM deploy |
| `deployment/aws/ec2/README.md` | EC2 vs local compose; deploy order |

**Placeholders:** `OLD_INSTANCE_ID`, `NEW_INSTANCE_ID`, `api-dev.fieldwhizey.com` (or your subdomain), emails, SPA origin.  
**Do not** commit `/opt/fgs/.env`, `setup-appsettings.json`, or TLS private keys.

---

## 0. Prerequisites (new EC2)

Before connecting:

- [ ] Same IAM instance profile as the working host (`AmazonSSMManagedInstanceCore` + ECR pull + KMS/S3 as required)
- [ ] Security group: inbound **80** and **443** (and from ALB if used); outbound **443**
- [ ] SSM shows instance **Online**
- [ ] RDS reachable from the new instance (same VPC/SG rules as old host)
- [ ] ECR channel tags exist (`setup-dev`, `user-dev`, …)

---

## 1. Connect (interactive session)

From your laptop:

```bash
# Old host (read config / certs)
aws ssm start-session --target OLD_INSTANCE_ID --region us-east-1

# New host (bootstrap + deploy)
aws ssm start-session --target NEW_INSTANCE_ID --region us-east-1
```

All following commands run **inside** the SSM session on the named host.

---

## 2. Old EC2 — copy configuration

```bash
sudo cat /opt/fgs/.env
sudo cat /opt/fgs/config/setup-appsettings.json
sudo ls -la /opt/fgs/certs/
sudo openssl x509 -in /opt/fgs/certs/tls.crt -noout -subject -dates -ext subjectAltName
```

Copy off-host securely (password manager / secure paste), not into git:

| Path | Purpose |
| --- | --- |
| `/opt/fgs/.env` | RabbitMQ boot, public URL, Entra URLs, ECR image tags |
| `/opt/fgs/config/setup-appsettings.json` | `ConnectionStrings:FgsSetup` only |
| `/opt/fgs/certs/tls.crt` | TLS certificate (or full chain) |
| `/opt/fgs/certs/tls.key` | TLS private key |

Other secrets stay in RDS `glo.GloCredential` (shared if the new host uses the same database).

Optional — see what is running:

```bash
cd /opt/fgs
docker compose -f docker-compose.ec2.yml ps
```

---

## 3. DNS for the subdomain

In Route 53 (or your DNS provider), create a record for the API host, for example:

- **Name:** `api-dev.fieldwhizey.com`
- **Type:** A (or Alias) → new EC2 public IP, **or** ALB if the ALB fronts the instance

Verify:

```bash
dig +short api-dev.fieldwhizey.com
# or
nslookup api-dev.fieldwhizey.com
```

Ensure the security group allows **80** (Let’s Encrypt HTTP-01) and **443** (HTTPS).

---

## 4. TLS certificate for the subdomain

Nginx on this stack terminates TLS using host mounts:

- `/opt/fgs/certs/tls.crt`
- `/opt/fgs/certs/tls.key`

`NGINX_SERVER_NAME` in `/opt/fgs/.env` must match the certificate SAN / CN (default `api-dev.fieldwhizey.com`).

> **ACM note:** ACM certificates for an **ALB** do not export a private key. This EC2 compose stack terminates TLS **on nginx**, so you need files under `/opt/fgs/certs/` (wildcard copy, Let’s Encrypt, or another CA).

### 4.1 Option A — reuse existing wildcard `*.fieldwhizey.com`

If the old host already has a valid wildcard cert, paste the same files on the new host:

```bash
sudo mkdir -p /opt/fgs/certs
sudo nano /opt/fgs/certs/tls.crt
sudo nano /opt/fgs/certs/tls.key
sudo chmod 644 /opt/fgs/certs/tls.crt
sudo chmod 600 /opt/fgs/certs/tls.key
sudo openssl x509 -in /opt/fgs/certs/tls.crt -noout -subject -dates -ext subjectAltName
```

### 4.2 Option B — Let’s Encrypt (new cert for the subdomain)

Install certbot.

**Amazon Linux:**

```bash
sudo dnf install -y certbot
```

**Ubuntu:**

```bash
sudo apt update
sudo apt install -y certbot
```

If nginx is already bound to 80/443, stop it first:

```bash
cd /opt/fgs
sudo docker compose -f docker-compose.ec2.yml stop nginx
```

Issue the certificate (HTTP-01):

```bash
sudo certbot certonly --standalone \
  -d api-dev.fieldwhizey.com \
  --agree-tos \
  -m YOUR_EMAIL@example.com \
  --non-interactive
```

Install into FGS paths:

```bash
sudo mkdir -p /opt/fgs/certs
sudo cp /etc/letsencrypt/live/api-dev.fieldwhizey.com/fullchain.pem /opt/fgs/certs/tls.crt
sudo cp /etc/letsencrypt/live/api-dev.fieldwhizey.com/privkey.pem /opt/fgs/certs/tls.key
sudo chmod 644 /opt/fgs/certs/tls.crt
sudo chmod 600 /opt/fgs/certs/tls.key
sudo openssl x509 -in /opt/fgs/certs/tls.crt -noout -subject -dates -ext subjectAltName
```

Renew later, then re-copy and recreate nginx:

```bash
sudo certbot renew
sudo cp /etc/letsencrypt/live/api-dev.fieldwhizey.com/fullchain.pem /opt/fgs/certs/tls.crt
sudo cp /etc/letsencrypt/live/api-dev.fieldwhizey.com/privkey.pem /opt/fgs/certs/tls.key
cd /opt/fgs
sudo docker compose -f docker-compose.ec2.yml up -d --no-deps --force-recreate nginx
```

### 4.3 Option C — self-signed (dev only; browsers warn)

```bash
sudo mkdir -p /opt/fgs/certs
sudo openssl req -x509 -nodes -newkey rsa:2048 -days 365 \
  -keyout /opt/fgs/certs/tls.key \
  -out /opt/fgs/certs/tls.crt \
  -subj "/CN=api-dev.fieldwhizey.com" \
  -addext "subjectAltName=DNS:api-dev.fieldwhizey.com"
sudo chmod 600 /opt/fgs/certs/tls.key
```

---

## 5. New EC2 — pull deploy files from git `dev`

Bootstrap and day-2 script updates come from the **dev** branch of `fgs-coreapi`.  
Files under `deployment/aws/ec2/` are installed into `/opt/fgs/`.

### 5.1 First time — clone + bootstrap

```bash
# Amazon Linux
sudo yum install -y git
# Ubuntu: sudo apt update && sudo apt install -y git

cd /tmp
sudo rm -rf fgs
git clone --branch dev --single-branch https://github.com/nathshivfsm-sys/fgs-coreapi.git fgs
cd fgs
sudo bash deployment/aws/ec2/bootstrap-ec2.sh
```

Private repo (pick one):

```bash
git clone --branch dev --single-branch https://<GITHUB_PAT>@github.com/nathshivfsm-sys/fgs-coreapi.git fgs
# or
git clone --branch dev --single-branch git@github.com:nathshivfsm-sys/fgs-coreapi.git fgs
```

`bootstrap-ec2.sh` installs Docker (if needed) and copies:

| Repo file | Installed to |
| --- | --- |
| `deploy-service.sh` | `/opt/fgs/deploy-service.sh` |
| `docker-compose.ec2.yml` | `/opt/fgs/docker-compose.ec2.yml` |
| `nginx-https-entrypoint.sh` | `/opt/fgs/nginx-https-entrypoint.sh` |
| `nginx-http-only-entrypoint.sh` | `/opt/fgs/nginx-http-only-entrypoint.sh` |

It also creates placeholder `/opt/fgs/.env` and `/opt/fgs/config/setup-appsettings.json` if missing — **overwrite** those with values from the old host (section 6).

### 5.2 Later — refresh scripts from `dev` (no full bootstrap)

```bash
cd /tmp/fgs
git fetch origin
git checkout dev
git pull --ff-only origin dev

sudo install -m 0755 deployment/aws/ec2/deploy-service.sh /opt/fgs/deploy-service.sh
sudo install -m 0755 deployment/aws/ec2/nginx-https-entrypoint.sh /opt/fgs/nginx-https-entrypoint.sh
sudo install -m 0755 deployment/aws/ec2/nginx-http-only-entrypoint.sh /opt/fgs/nginx-http-only-entrypoint.sh
sudo install -m 0644 deployment/aws/ec2/docker-compose.ec2.yml /opt/fgs/docker-compose.ec2.yml
```

---

## 6. New EC2 — restore secrets and subdomain configuration

```bash
sudo mkdir -p /opt/fgs/config /opt/fgs/certs

sudo nano /opt/fgs/.env
sudo nano /opt/fgs/config/setup-appsettings.json
# If not already done in section 4:
sudo nano /opt/fgs/certs/tls.crt
sudo nano /opt/fgs/certs/tls.key
sudo chmod 600 /opt/fgs/certs/tls.key
```

### 6.1 Required `/opt/fgs/.env` keys for the subdomain

```env
FGS_CONFIG_DIR=/opt/fgs/config
FGS_ECR_REGISTRY=ACCOUNT_ID.dkr.ecr.us-east-1.amazonaws.com
FGS_ECR_REPO=fgs/dockers
FGS_CHANNEL=dev

ASPNETCORE_ENVIRONMENT=Development

RABBITMQ_USER=fgs
RABBITMQ_PASSWORD=SAME_AS_OLD_HOST_AND_GloCredential

CREDENTIAL_DISTRIBUTION_KEY=fgs-internal-credential-distribution-key

NGINX_SERVER_NAME=api-dev.fieldwhizey.com
FGS_PUBLIC_BASE_URL=https://api-dev.fieldwhizey.com
FGS_PUBLIC_SERVICE_PATH=user-service

FGS_UI_AUTH_CALLBACK_URL=https://api-dev.fieldwhizey.com/user-service/api/v1/auth/entra/callback
FGS_UI_POST_LOGIN_REDIRECT_URL=https://YOUR_SPA_ORIGIN

DD_ENV=dev
DD_SITE=datadoghq.com
```

`deploy-service.sh` upserts `FGS_*_IMAGE` lines to channel tags when you deploy.

### 6.2 Setup RDS bootstrap

`/opt/fgs/config/setup-appsettings.json` — **FgsSetup connection string only** (same as old host if sharing RDS):

```json
{
  "ConnectionStrings": {
    "FgsSetup": "Host=YOUR_RDS_HOST;Port=5432;Database=fgs_dev_db;Username=YOUR_USER;Password=YOUR_PASSWORD"
  }
}
```

### 6.3 RabbitMQ password alignment

| Location | Purpose |
| --- | --- |
| `/opt/fgs/.env` `RABBITMQ_USER` / `RABBITMQ_PASSWORD` | RabbitMQ **container** boot |
| `glo.GloCredential` `Global:RABBITMQ` | Apps via Setup credential distribution |

Passwords **must match**. Redis/RabbitMQ Docker volumes are **local** to each instance — a new host starts empty brokers; apps rehydrate credentials from Setup/RDS after deploy.

### 6.4 Entra External ID (outside EC2)

App registration → Authentication → **Web** redirect URI — add exactly:

```text
https://api-dev.fieldwhizey.com/user-service/api/v1/auth/entra/callback
```

---

## 7. Deploy all services

```bash
cd /opt/fgs

sudo ./deploy-service.sh redis dev
sudo ./deploy-service.sh rabbitmq dev
sudo ./deploy-service.sh setup-service dev
sudo ./deploy-service.sh audit-service dev
sudo ./deploy-service.sh user-service dev
sudo ./deploy-service.sh bff-service dev
sudo ./deploy-service.sh notification-service dev
sudo ./deploy-service.sh file-service dev
sudo ./deploy-service.sh inventory-service dev
sudo ./deploy-service.sh asset-service dev
sudo ./deploy-service.sh consumer-service dev
sudo ./deploy-service.sh nginx dev
```

Explicit repo/region (same as CD):

```bash
sudo ./deploy-service.sh setup-service dev fgs/dockers us-east-1
```

After **cert** or **hostname-only** `.env` changes (images already current):

```bash
cd /opt/fgs
sudo docker compose -f docker-compose.ec2.yml up -d --no-deps --force-recreate nginx
sudo docker compose -f docker-compose.ec2.yml up -d --no-deps --force-recreate user-service
```

---

## 8. Verify

```bash
cd /opt/fgs
docker compose -f docker-compose.ec2.yml ps

# Cert files and SAN
sudo ls -la /opt/fgs/certs/
sudo openssl x509 -in /opt/fgs/certs/tls.crt -noout -subject -dates -ext subjectAltName
sudo docker compose -f docker-compose.ec2.yml exec nginx printenv NGINX_SERVER_NAME

# Health
curl -s -o /dev/null -w "%{http_code}\n" http://localhost/nginx-health
curl -k -s -o /dev/null -w "%{http_code}\n" https://localhost/nginx-health
docker exec $(docker ps -qf name=redis) redis-cli ping
docker exec $(docker ps -qf name=setup-service) curl -fsS http://localhost:5004/health
docker exec $(docker ps -qf name=user-service) curl -fsS http://localhost:5001/health

# Public HTTPS
curl -s -o /dev/null -w "%{http_code}\n" https://api-dev.fieldwhizey.com/nginx-health
echo | openssl s_client -servername api-dev.fieldwhizey.com -connect api-dev.fieldwhizey.com:443 2>/dev/null \
  | openssl x509 -noout -subject -dates
```

Expected: `nginx-health` → **200**, redis → **PONG**, Setup/User `/health` OK.

---

## 9. Cut over

1. Point ALB target group to `NEW_INSTANCE_ID` (port **80**, health path **`/nginx-health`**) if ALB is used, **or** update DNS A record to the new public IP.
2. Update GitHub Actions variable **`EC2_INSTANCE_ID`** to `NEW_INSTANCE_ID` if CD should target this host.
3. Confirm Entra redirect URI matches the live subdomain.
4. Decommission or stop the old instance only after verification.

---

## 10. Quick-reference checklist

### AWS / DNS

- [ ] New EC2: instance profile + SSM **Online**
- [ ] SG: 80/443 inbound; 443 outbound
- [ ] DNS A/Alias for subdomain → new host or ALB
- [ ] ECR `*-dev` tags present

### Git / host files

- [ ] `git clone --branch dev` of `fgs-coreapi` (or `git pull` refresh)
- [ ] `bootstrap-ec2.sh` completed once
- [ ] `/opt/fgs/deploy-service.sh`, `docker-compose.ec2.yml`, nginx entrypoints present

### Secrets / TLS

- [ ] `/opt/fgs/.env` restored; `NGINX_SERVER_NAME` + `FGS_PUBLIC_BASE_URL` + Entra URLs set
- [ ] `/opt/fgs/config/setup-appsettings.json` — FgsSetup only
- [ ] `/opt/fgs/certs/tls.crt` + `tls.key` installed; SAN covers subdomain
- [ ] RabbitMQ password matches `GloCredential`
- [ ] Entra Web redirect URI registered

### Deploy / verify

- [ ] Full deploy order (section 7)
- [ ] `docker compose … ps` healthy
- [ ] `https://<subdomain>/nginx-health` → 200
- [ ] GitHub `EC2_INSTANCE_ID` updated (if using CD)

---

## 11. Command cheat sheet

```bash
# Session
aws ssm start-session --target INSTANCE_ID --region us-east-1

# Read config (old host)
sudo cat /opt/fgs/.env
sudo cat /opt/fgs/config/setup-appsettings.json

# Clone + bootstrap (new host)
cd /tmp && git clone --branch dev --single-branch https://github.com/nathshivfsm-sys/fgs-coreapi.git fgs
cd fgs && sudo bash deployment/aws/ec2/bootstrap-ec2.sh

# Refresh scripts from dev
cd /tmp/fgs && git pull --ff-only origin dev
sudo install -m 0755 deployment/aws/ec2/deploy-service.sh /opt/fgs/deploy-service.sh
sudo install -m 0644 deployment/aws/ec2/docker-compose.ec2.yml /opt/fgs/docker-compose.ec2.yml

# Deploy one / all
sudo /opt/fgs/deploy-service.sh <service> dev fgs/dockers us-east-1

# Status
cd /opt/fgs && docker compose -f docker-compose.ec2.yml ps
curl -s -o /dev/null -w "%{http_code}\n" https://api-dev.fieldwhizey.com/nginx-health
```

**Compose services:**  
`redis`, `rabbitmq`, `setup-service`, `audit-service`, `user-service`, `bff-service`, `notification-service`, `file-service`, `inventory-service`, `asset-service`, `consumer-service`, `nginx`
