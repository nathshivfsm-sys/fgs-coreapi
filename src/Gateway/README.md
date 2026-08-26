# FGS NGINX API Gateway

Production-ready NGINX reverse proxy setup for the local .NET 10 microservices stack. NGINX is the only public entry point; service containers are reachable only on the shared Docker network.

## Local Docker Desktop vs EC2

Use **this folder** for day-to-day development on Docker Desktop. Do **not** run `deployment/aws/ec2/docker-compose.ec2.yml` on your laptop.

| | **Local (this README)** | **EC2 AWS dev** |
| --- | --- | --- |
| Directory | `src/Gateway/` | `deployment/aws/ec2/` → `/opt/fgs/` on instance |
| Command | `docker compose up --build` | `deploy-service.sh` after CI push |
| Compose | `docker-compose.yml` | `docker-compose.ec2.yml` |
| Project name | `fgs-local` | `fgs-ec2` |
| Images | Build from `docker/*.Dockerfile` | Pull from ECR |

EC2 guide: [deployment/aws/ec2/README.md](../../deployment/aws/ec2/README.md)

## Folder Structure

```text
src/Gateway/
  Dockerfile
  Dockerfile.prod
  README.md
  cache_params.conf
  docker-compose.yml
  nginx.conf
  nginx.prod.conf
  proxy_params.conf
  cache/
  certs/
  conf.d/
    site.conf
    site.prod.conf
    includes/
      api-v1-routes.conf
      api-v1-routes.prod.conf
      rate-limit.inc
      swagger-index.html
      swagger-routes.conf
      upstreams.conf
      upstreams.prod.conf
  docker/
    user-service.Dockerfile
    bff-service.Dockerfile
    notification-service.Dockerfile
    setup-service.Dockerfile
    file-service.Dockerfile
    audit-service.Dockerfile
    publisher-service.Dockerfile
    consumer-service.Dockerfile
    inventory-service.Dockerfile
    asset-service.Dockerfile
    # scaffold-only Dockerfiles (not in docker-compose.yml yet):
    crm-service.Dockerfile
    scheduling-service.Dockerfile
    billing-service.Dockerfile
    reporting-service.Dockerfile
    integration-service.Dockerfile
    service-agreement-service.Dockerfile
    communication-service.Dockerfile
  logs/
  scripts/
    Compare-ApiRoutes.ps1
    generate-local-cert.ps1
    generate-local-cert.sh
```

RabbitMQ runs in this Compose file for **PublisherService** and **ConsumerService** (host ports `5672` / `15672`). Publisher and Consumer are containerized on the private Docker network; they are not exposed through NGINX. PostgreSQL is expected on the host or reachable via connection strings in mounted `appsettings.Development.json`.

## Routes

NGINX listens on `https://developer.fsm.com` (host ports `80` / `443`). Keep `conf.d/includes/api-v1-routes*.conf` and `api-v1-service-prefix-routes.conf` in sync with `[FgsVersionedRoute]` templates under each `*/Controllers` folder (`scripts/Compare-ApiRoutes.ps1`).

Map the hostname locally before first run (Administrator / root):

```text
127.0.0.1  developer.fsm.com
```

Windows: `C:\Windows\System32\drivers\etc\hosts`. Linux/macOS: `/etc/hosts`. Regenerate the self-signed cert after pulling (`.\scripts\generate-local-cert.ps1`) so the SAN includes `developer.fsm.com`.

| Public route | Upstream | Notes |
| --- | --- | --- |
| `/{service-name}/api/v1/...` | per service | **EC2 / production only** — e.g. `/setup-service/api/v1/billingcategory/lookup` (`api-v1-service-prefix-routes.conf`) |
| `/api/v1/auth/*` | `user-service:5001` | Entra token, refresh, me, callback |
| `/api/v1/bff/*` | `bff-service:5003` | Signup orchestration + GraphQL (`/api/v1/bff/graphql`) |
| `/api/v1/login/*` | `user-service:5001` | |
| `/api/v1/invite/*` | `user-service:5001` | |
| `/api/v1/signup/*` | `user-service:5001` | |
| `/api/v1/dashboard` | `user-service:5001` | |
| `/api/v1/(role\|permission\|dataaccess\|…\|apiwebhooksubscription\|company)` | `user-service:5001` | Identity catalog / API management / company CRUD |
| `/api/v1/internal/users/*` | `user-service:5001` | Internal auth-profile |
| `/api/v1/notification/*` | `notification-service:5002` | e.g. `POST …/notification/dispatch` |
| `/api/v1/credential/*` | `setup-service:5004` | Credential admin |
| `/api/v1/communication-template/*` | `setup-service:5004` | Templates for Notification |
| `/api/v1/tenantprovisioning` | S2S only (Consumer→Setup) | Public gateway returns 403; not proxied |
| `/api/v1/tenant/{id}/companies/{id}/businesstype` | S2S only (BFF→Setup) | Public gateway returns 403; not proxied |
| `/api/v1/company/*` | `user-service:5001` | Company CRUD |
| `/api/v1/tenant/{id}/bucket` | `file-service:5005` | Bucket provisioning |
| `/api/v1/tenant/*` (other) | `user-service:5001` | Tenant CRUD |
| `/api/v1/attachment/*` | `file-service:5005` | Attachments |
| `/api/v1/credentialaudit/*` | `audit-service:5008` | Credential audit writes |
| `/api/v1/(inventory-location\|truck-stock-template\|vendor)/*` | `inventory-service:5012` | Inventory catalog |
| `/api/v1/(assettype\|…\|assetwarranty)/*` | `asset-service:5015` | Asset catalog |
| `/api/v1/(billingcategory\|…\|zone)/*` | `setup-service:5004` | Setup catalog (includes universal pricing matrix) |

**Service-prefixed URLs (EC2 / production nginx only)** — local Docker Desktop keeps flat `/api/v1/...`:

| Example | Service |
| --- | --- |
| `/setup-service/api/v1/billingcategory/lookup` | Setup |
| `/user-service/api/v1/user/lookup` | User |
| `/bff-service/api/v1/bff/signup` | BFF |

Docker Compose service names match the URL prefix. Config: `conf.d/includes/api-v1-service-prefix-routes.conf` (included from `api-v1-routes.prod.conf` only).

**Local dev** uses flat routes only, e.g. `https://developer.fsm.com/api/v1/billingcategory/lookup`.

Publisher and Consumer are on the private Docker network only (no public API routes).

### Swagger (OpenAPI UI) — local only

Swagger is exposed through the local gateway at **`https://developer.fsm.com/swagger/`**. Production (`site.prod.conf` / `Dockerfile.prod`) does **not** include Swagger routes.

| Swagger URL | Service |
| --- | --- |
| `https://developer.fsm.com/swagger/user/` | User |
| `https://developer.fsm.com/swagger/bff/` | BFF |
| `https://developer.fsm.com/swagger/notification/` | Notification |
| `https://developer.fsm.com/swagger/setup/` | Setup |
| `https://developer.fsm.com/swagger/file/` | File |
| `https://developer.fsm.com/swagger/audit/` | Audit |
| `https://developer.fsm.com/swagger/inventory/` | Inventory |
| `https://developer.fsm.com/swagger/asset/` | Asset |
| `https://developer.fsm.com/swagger/publisher/` | Publisher |
| `https://developer.fsm.com/swagger/consumer/` | Consumer |

When adding a service to local Compose, set `Swagger__RoutePrefix: swagger/{service}` on that container and add a matching block in `conf.d/includes/swagger-routes.conf`.

### Database-backed services (local dev)

Each service uses its own connection string (`FgsUser`, `FgsSetup`, `FgsFile`, etc.). PostgreSQL init script: [`scripts/init-postgres.sql`](scripts/init-postgres.sql). Ownership map: [`docs/architecture/DATABASE_OWNERSHIP_MIGRATION.md`](../../docs/architecture/DATABASE_OWNERSHIP_MIGRATION.md).

Generate EF SQL scripts: [`scripts/generate-migration-sql.ps1`](../../scripts/generate-migration-sql.ps1).

OAuth and invitation URLs are exposed through the gateway (register the same values in Microsoft Entra):

| Setting | Local gateway value |
| --- | --- |
| `EntraExternalId:RedirectUri` | `https://developer.fsm.com/api/v1/auth/entra/callback` (signup/invite API callback) |
| `EntraExternalId:LoginRedirectUri` | SPA login callback (e.g. `https://localhost:3000/auth/callback`) |
| `Invitation:InviteBaseUrl` | `https://developer.fsm.com/api/v1/invite/start` |

Both upstreams use `least_conn`, keepalive connections, passive health checks with `max_fails` and `fail_timeout`, and Docker health checks against each service's `/health` endpoint.

## Response compression

Compression is enabled at **two layers** (each compresses only when the response is still uncompressed):

| Layer | Where | Formats |
| --- | --- | --- |
| **Services** | `Fgs.Foundation` via `AddFgsApiHost` / `UseFgsApiHost` | Brotli (`br`) and Gzip |
| **Gateway** | `nginx.conf` (local) and `nginx.prod.conf` (production) | Gzip |

- Direct service access (local ports, inter-service debugging) benefits from ASP.NET response compression.
- Public clients through `https://developer.fsm.com` also get gateway gzip for JSON and related MIME types.
- When an upstream already sends `Content-Encoding: gzip` or `br`, nginx passes it through without re-compressing.

Optional per-service override in `appsettings.json`:

```json
"ResponseCompression": {
  "Enabled": true,
  "EnableForHttps": true
}
```

Set `UseResponseCompression = false` on `FgsApiHostOptions` to opt out for a specific service.

### Inter-service Refit URLs (Docker)

Inter-service Refit clients use **direct container DNS and ports** on the `fgs-private` network — not the NGINX gateway. NGINX path rewrites break several internal routes (for example `/api/v1/notifications/dispatch` and `/api/v1/tenants/*`).

| Caller | Refit client | Base URL |
| --- | --- | --- |
| All services with remote auth | `IFgsClaimsClient` | `http://user-service:5001` |
| Setup | `IUserTenantClient` | `http://user-service:5001` |
| Setup | `IFileTenantClient` | `http://file-service:5005` |
| User, Notification, Consumer, Publisher, File | `ISetupClient` | `http://setup-service:5004` |
| Setup | `IAuditClient` | `http://audit-service:5008` |
| Consumer | `INotificationDispatchClient` | `http://notification-service:5002` |
| Publisher | `IFgsClaimsClient` | `http://user-service:5001` |
| Domain scaffolds | Direct container DNS | `http://crm-service:5009`, `http://scheduling-service:5010`, `http://billing-service:5011`, `http://inventory-service:5012`, `http://reporting-service:5013`, `http://integration-service:5014`, `http://asset-service:5015`, `http://service-agreement-service:5016`, `http://communication-service:5017` |

Public-facing URLs (OAuth, invites, dashboard) use the NGINX gateway at `https://developer.fsm.com`.

## Run Locally (Docker Desktop)

From `C:\SourceCode\FGS\src\Gateway` only — not from `deployment/aws/ec2/`.

```powershell
.\scripts\generate-local-cert.ps1
$env:DOCKER_BUILDKIT = "1"
docker compose up --build
```

If `dotnet restore` fails inside Docker with NuGet SSL errors (`unexpected EOF from transport stream`), retry the build — Dockerfiles use a shared restore script with retries and a persistent NuGet cache mount. You can also pre-restore on the host (`dotnet restore` from `src/`) before building so packages are already in the BuildKit cache.

Linux/macOS/WSL:

```sh
./scripts/generate-local-cert.sh
docker compose up --build
```

Test the gateway:

```powershell
curl.exe -k https://developer.fsm.com/nginx-health
curl.exe -k https://developer.fsm.com/api/v1/auth/me
curl.exe -k https://developer.fsm.com/api/v1/notification/dispatch
curl.exe -k https://developer.fsm.com/api/v1/credential/
curl.exe -k https://developer.fsm.com/api/v1/attachment/
curl.exe -k https://developer.fsm.com/api/v1/vendor/
curl.exe -k https://developer.fsm.com/api/v1/asset/
curl.exe -k https://developer.fsm.com/swagger/
```

Container health checks use each service's `/health` endpoint (see Dockerfiles). Public API routes match controller `[FgsVersionedRoute]` templates (singular path segments such as `/api/v1/credential`, `/api/v1/tenant`, `/api/v1/attachment`, `/api/v1/notification`).

The local Compose file starts services in credential-safe order:

1. `rabbitmq` — host ports `5672` / `15672`
2. `setup-service` (`5004`) — credential authority; bootstraps `ConnectionStrings:FgsSetup` from mounted appsettings (no dependency on other FGS APIs)
3. `audit-service` (`5008`) starts after Setup; other credential consumers wait for Setup **and** Audit healthy
4. Messaging workers: `publisher-service` (`5006`), `consumer-service` (`5007`)
5. `nginx` — host ports `80` / `443` (waits for gateway upstream health checks)

### Credential bootstrap environment variables

Consuming services load secrets from Setup Service at startup (`GET /api/v1/credentials/resolved`). Configure these bootstrap values (non-secret) in appsettings or Docker env:

| Variable / setting | Purpose |
| --- | --- |
| `SetupService__BaseUrl` | Setup Service URL for `ISetupClient` |
| `CredentialDistribution__InternalServiceKey` | S2S key for `/credentials/resolved` |
| `CredentialConsumer__ServiceName` | Service identity for access audit |
| `CredentialConsumer__RequiredProviders__0` | Provider filter (e.g. `DATABASE`, `SENDGRID`) |
| `AuditService__Enabled` | On `setup-service`, set to `false` in Compose so Setup does not call Audit before Audit is listening (local bootstrap race). Set `true` in `appsettings.Development.json` when testing audit integration outside Compose. |
| `FGS_SETUP_DB` | Setup Service DB bootstrap (Setup only) |
| `FGS_USER_DB`, `FGS_FILE_DB`, etc. | Optional DB fallback during credential migration |
| `KMS_KEY_ARN` | KMS key ARN for Setup Service bootstrap only (File/User load `AwsCredentials:KmsKeyArn` from Setup) |

See [tools/credential-migration/README.md](../../tools/credential-migration/README.md) for migrating legacy appsettings secrets into `GloCredential`.

### Application configuration (Postgres, Entra)

Each API container mounts the **same** files you edit for local `dotnet run`:

| Service | Mounted files |
| --- | --- |
| User | `src/UserService/Fgs.User.API/appsettings.json` + `appsettings.Development.json` |
| Platform | `src/NotificationService/Fgs.Notification.API/appsettings.json` + `appsettings.Development.json` |
| Setup | `src/SetupService/Fgs.Setup.API/appsettings.json` + `appsettings.Development.json` |
| Audit | `src/AuditService/Fgs.Audit.API/appsettings.json` + `appsettings.Development.json` |
| File | `src/FileService/Fgs.File.API/appsettings.json` + `appsettings.Development.json` |
| Publisher | `src/PublisherService/Fgs.Publisher.API/appsettings.json` + `appsettings.Development.json` |
| Consumer | `src/ConsumerService/Fgs.Consumer.API/appsettings.json` + `appsettings.Development.json` |
| Crm | `src/CrmService/Fgs.Crm.API/appsettings.json` + `appsettings.Development.json` |
| Scheduling | `src/SchedulingService/Fgs.Scheduling.API/appsettings.json` + `appsettings.Development.json` |
| Billing | `src/BillingService/Fgs.Billing.API/appsettings.json` + `appsettings.Development.json` |
| Inventory | `src/InventoryService/Fgs.Inventory.API/appsettings.json` + `appsettings.Development.json` |
| Reporting | `src/ReportingService/Fgs.Reporting.API/appsettings.json` + `appsettings.Development.json` |
| Integration | `src/IntegrationService/Fgs.Integration.API/appsettings.json` + `appsettings.Development.json` |
| Asset | `src/AssetService/Fgs.Asset.API/appsettings.json` + `appsettings.Development.json` |
| Service Agreement | `src/ServiceAgreementService/Fgs.ServiceAgreement.API/appsettings.json` + `appsettings.Development.json` |
| Communication | `src/CommunicationService/Fgs.Communication.API/appsettings.json` + `appsettings.Development.json` |

Containers do **not** bake `ASPNETCORE_ENVIRONMENT` into the image. Compose sets `ASPNETCORE_ENVIRONMENT` (default `Development`; override with the env var). Mount Setup `appsettings.json` for local bootstrap only — on AWS use Secrets Manager / task env. Redis is set via Compose (`Redis__ConnectionString=redis:6379`) so the container reaches the Compose Redis service.

## Scale Services Locally

Do not add `container_name`; Docker Compose needs generated names for scaling.

```powershell
docker compose up --build --scale user-service=2 --scale notification-service=2 --scale setup-service=2 --scale file-service=2
```

NGINX resolves the Compose service names and load balances with `least_conn`. If you scale after NGINX has already started, recreate or reload NGINX so it refreshes upstream DNS:

```powershell
docker compose up -d --force-recreate nginx
```

## Security

The gateway applies:

- HTTP to HTTPS redirect.
- HSTS, `X-Frame-Options`, `X-Content-Type-Options`, `X-XSS-Protection`, and `Referrer-Policy`.
- `server_tokens off`.
- `client_max_body_size 10m`.
- Per-client request and connection limits.
- Basic blocking for common probes such as path traversal, `.env`, `.git`, WordPress, phpMyAdmin, and simple SQL/script injection patterns.
- Forwarded `X-Request-ID` and `X-Correlation-ID` headers.

For production, replace local self-signed certificates with ACME, a managed certificate, or your platform certificate secret.

## Caching

`cache_params.conf` enables NGINX proxy caching for `GET` and `HEAD` responses, including OpenAPI/Swagger JSON and static assets returned through the service routes. Requests with `Authorization`, `Cache-Control`, `Pragma`, or `?nocache=1` bypass cache storage and lookup.

Basic invalidation options:

- Send `Cache-Control: no-cache` or append `?nocache=1` for a one-off bypass.
- Remove the Docker cache directory or named cache volume during deployment.
- Use short TTLs for mutable API data.
- Add NGINX Plus, OpenResty, or an external CDN if URL-level purge is required.

## Logs and Observability

Access logs are JSON formatted and include:

- Request ID and correlation ID.
- Upstream address, status, and response time.
- NGINX cache status.
- Request duration, method, URI, status, bytes, and user agent.

Local logs are mounted to `src/Gateway/logs`.

## Promote to a Linux VM

1. Build and publish the service images to your registry.
2. Build the production gateway image (from repo root):

```sh
docker build -f src/Gateway/Dockerfile.prod -t fgs-gateway:prod src/Gateway
```

   Or copy these files to the VM and install NGINX directly:
   - `nginx.prod.conf`
   - `conf.d/site.prod.conf`
   - `conf.d/includes/` (especially `api-v1-routes.prod.conf`, `upstreams.prod.conf`, `rate-limit.inc`)
   - `proxy_params.conf`, `cache_params.conf`
3. Mount production certificates at `/etc/nginx/certs/tls.crt` and `/etc/nginx/certs/tls.key` (`server_name` is `developer.fsm.com` in `site.prod.conf`).
4. Run NGINX in Docker or install NGINX directly on the VM.
5. Keep microservices on a private Docker network or private VM subnet.
6. Publish only ports `80` and `443` from NGINX.

For direct VM NGINX:

```sh
sudo nginx -t
sudo systemctl reload nginx
```

Validate route coverage against controllers before deploy:

```powershell
.\scripts\Compare-ApiRoutes.ps1
```

## Kubernetes Path Later

For Kubernetes, keep this routing model but move responsibilities as follows:

- Use Kubernetes `Service` objects for the live compose services: `user-service`, `bff-service`, `notification-service`, `setup-service`, `file-service`, `audit-service`, `inventory-service`, `asset-service`, `publisher-service`, and `consumer-service`.
- Put TLS certificates in `kubernetes.io/tls` secrets, or use cert-manager.
- Use NGINX Ingress Controller for path routing that mirrors `api-v1-routes.prod.conf` (no path-stripping rewrites).
- Keep the current controller route templates as the stable external contract (`/api/v1/auth`, `/api/v1/notification`, `/api/v1/credential`, `/api/v1/tenant`, `/api/v1/attachment`, catalog controllers, etc.).
- Move rate limiting, body size, gzip, timeouts, and security headers into Ingress annotations or a controller ConfigMap.

The production NGINX files remain useful as the reference edge policy when converting to Ingress resources.
