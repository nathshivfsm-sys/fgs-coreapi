# FGS NGINX API Gateway

Production-ready NGINX reverse proxy setup for the local .NET 10 microservices stack. NGINX is the only public entry point; service containers are reachable only on the shared Docker network.

## Folder Structure

```text
src/Gateway/
  Dockerfile
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
  docker/
    user-service.Dockerfile
    notification-service.Dockerfile
    setup-service.Dockerfile
    file-service.Dockerfile
    audit-service.Dockerfile
    publisher-service.Dockerfile
    consumer-service.Dockerfile
    crm-service.Dockerfile
    scheduling-service.Dockerfile
    billing-service.Dockerfile
    inventory-service.Dockerfile
    reporting-service.Dockerfile
    integration-service.Dockerfile
    asset-service.Dockerfile
    service-agreement-service.Dockerfile
    communication-service.Dockerfile
  logs/
  scripts/
    generate-local-cert.ps1
    generate-local-cert.sh
```

RabbitMQ runs in this Compose file for **PublisherService** and **ConsumerService** (host ports `5672` / `15672`). Publisher and Consumer are containerized on the private Docker network; they are not exposed through NGINX. PostgreSQL is expected on the host or reachable via connection strings in mounted `appsettings.Development.json`.

## Routes

NGINX listens on `https://localhost:8443` locally.

| Public route | Upstream service | Forwarded path |
| --- | --- | --- |
| `/api/v1/auth/{path}` | `user-service:5001` | `/api/v1/auth/{path}` |
| `/api/v1/invite/{path}` | `user-service:5001` | `/api/v1/invite/{path}` |
| `/api/v1/signup/{path}` | `user-service:5001` | `/api/v1/signup/{path}` |
| `/api/v1/dashboard` | `user-service:5001` | `/api/v1/dashboard` |
| `/api/v1/users` | `user-service:5001` | `/api/v1/` |
| `/api/v1/users/{path}` | `user-service:5001` | `/api/v1/{path}` |
| `/api/v1/notifications` | `notification-service:5002` | `/api/v1/` |
| `/api/v1/notifications/{path}` | `notification-service:5002` | `/api/v1/{path}` |
| `/api/v1/credentials/{path}` | `setup-service:5004` | KMS-backed credential admin |
| `/api/v1/communication-templates/{path}` | `setup-service:5004` | Template reads for Notification |
| `/api/v1/tenants/{tenantId}/companies/{path}` | `user-service:5001` | Tenant company management (details, list) |
| `/api/v1/tenants/{tenantId}/bucket` | `file-service:5005` | S3 bucket and folder provisioning |
| `/api/v1/tenants/{path}` (other) | `file-service:5005` | Fallback tenant storage routes |
| `/api/v1/files/{path}` | `file-service:5005` | File upload, download, and metadata |
| `/api/v1/crm/{path}` | `crm-service:5009` | `/api/v1/{path}` |
| `/api/v1/scheduling/{path}` | `scheduling-service:5010` | `/api/v1/{path}` |
| `/api/v1/billing/{path}` | `billing-service:5011` | `/api/v1/{path}` |
| `/api/v1/inventory/{path}` | `inventory-service:5012` | `/api/v1/{path}` |
| `/api/v1/reporting/{path}` | `reporting-service:5013` | `/api/v1/{path}` |
| `/api/v1/integration/{path}` | `integration-service:5014` | `/api/v1/{path}` |
| `/api/v1/asset/{path}` | `asset-service:5015` | `/api/v1/{path}` |
| `/api/v1/service-agreements/{path}` | `service-agreement-service:5016` | `/api/v1/{path}` |
| `/api/v1/communication/{path}` | `communication-service:5017` | `/api/v1/{path}` |

### Swagger (OpenAPI UI)

Swagger is exposed through the gateway at **`https://localhost:8443/swagger/`**, which lists every service. Each service UI lives under a path prefix that matches its container configuration (`Swagger__RoutePrefix` in `docker-compose.yml`).

| Swagger URL | Service |
| --- | --- |
| `https://localhost:8443/swagger/user/` | User |
| `https://localhost:8443/swagger/notification/` | Notification |
| `https://localhost:8443/swagger/setup/` | Setup |
| `https://localhost:8443/swagger/file/` | File |
| `https://localhost:8443/swagger/audit/` | Audit |
| `https://localhost:8443/swagger/inventory/` | Inventory |
| `https://localhost:8443/swagger/publisher/` | Publisher |
| `https://localhost:8443/swagger/consumer/` | Consumer |
| `https://localhost:8443/swagger/crm/` | CRM (502 until container is running) |
| `https://localhost:8443/swagger/scheduling/` | Scheduling |
| `https://localhost:8443/swagger/billing/` | Billing |
| `https://localhost:8443/swagger/reporting/` | Reporting |
| `https://localhost:8443/swagger/integration/` | Integration |
| `https://localhost:8443/swagger/asset/` | Asset |
| `https://localhost:8443/swagger/service-agreement/` | Service Agreement |
| `https://localhost:8443/swagger/communication/` | Communication |

NGINX proxies `/swagger/{service}/` to the matching upstream with the same path. Each API sets `Swagger:RoutePrefix` (via `Swagger__RoutePrefix`) so Swagger UI and `swagger.json` URLs align with the gateway. Swagger is enabled in Development by default; set `Swagger:Enabled` to `true` in other environments if needed.

When adding a service to local Compose, set `Swagger__RoutePrefix: swagger/{service}` on that container and add a matching block in `conf.d/includes/swagger-routes.conf`.

### Database-backed services (local dev)

Each service uses its own connection string (`FgsUser`, `FgsSetup`, `FgsFile`, etc.). PostgreSQL init script: [`scripts/init-postgres.sql`](scripts/init-postgres.sql). Ownership map: [`docs/architecture/DATABASE_OWNERSHIP_MIGRATION.md`](../../docs/architecture/DATABASE_OWNERSHIP_MIGRATION.md).

Generate EF SQL scripts: [`scripts/generate-migration-sql.ps1`](../../scripts/generate-migration-sql.ps1).

OAuth and invitation URLs are exposed through the gateway (register the same values in Microsoft Entra):

| Setting | Local gateway value |
| --- | --- |
| `EntraExternalId:RedirectUri` | `https://localhost:8443/api/v1/auth/entra/callback` |
| `Invitation:InviteBaseUrl` | `https://localhost:8443/api/v1/invite/start` |

Both upstreams use `least_conn`, keepalive connections, passive health checks with `max_fails` and `fail_timeout`, and Docker health checks against each service's `/health` endpoint.

## Response compression

Compression is enabled at **two layers** (each compresses only when the response is still uncompressed):

| Layer | Where | Formats |
| --- | --- | --- |
| **Services** | `Fgs.Foundation` via `AddFgsApiHost` / `UseFgsApiHost` | Brotli (`br`) and Gzip |
| **Gateway** | `nginx.conf` (local) and `nginx.prod.conf` (production) | Gzip |

- Direct service access (local ports, inter-service debugging) benefits from ASP.NET response compression.
- Public clients through `https://localhost:8443` also get gateway gzip for JSON and related MIME types.
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

Public-facing URLs (OAuth, invites, dashboard) use the NGINX gateway at `https://localhost:8443`.

## Run Locally

From `C:\SourceCode\FGS\src\Gateway`:

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
curl.exe -k https://localhost:8443/nginx-health
curl.exe -k https://localhost:8443/api/v1/users/health
curl.exe -k https://localhost:8443/api/v1/notifications/health
curl.exe -k https://localhost:8443/api/v1/crm/health
curl.exe -k https://localhost:8443/api/v1/scheduling/health
curl.exe -k https://localhost:8443/api/v1/billing/health
curl.exe -k https://localhost:8443/api/v1/inventory/health
curl.exe -k https://localhost:8443/api/v1/reporting/health
curl.exe -k https://localhost:8443/api/v1/integration/health
curl.exe -k https://localhost:8443/api/v1/asset/health
curl.exe -k https://localhost:8443/api/v1/service-agreements/health
curl.exe -k https://localhost:8443/api/v1/communication/health
```

Container health checks use each service's `/health` endpoint (see Dockerfiles). Setup and File API routes are reachable under `/api/v1/credentials/` and `/api/v1/tenants/` without a path prefix rewrite.

The local Compose file starts services in credential-safe order:

1. `rabbitmq` — host ports `5672` / `15672`
2. `setup-service` (`5004`) — credential authority; bootstraps `ConnectionStrings:FgsSetup` from mounted appsettings (no dependency on other FGS APIs)
3. `audit-service` (`5008`) starts after Setup; other credential consumers wait for Setup **and** Audit healthy
4. Messaging workers: `publisher-service` (`5006`), `consumer-service` (`5007`)
5. `nginx` — host ports `8080` / `8443` (waits for gateway upstream health checks)

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

Containers use `ASPNETCORE_ENVIRONMENT=Development`, so ASP.NET Core **merges** `appsettings.json` then `appsettings.Development.json` (same as Visual Studio / `dotnet run`). There are no duplicate Postgres settings in `docker-compose.yml`.

Change connection strings in those JSON files and restart the service container; no image rebuild is required for config-only changes.

`appsettings.Development.json` overrides `Host` to `host.docker.internal` so containers can reach Postgres on the host. Base `appsettings.json` keeps `localhost` for `dotnet run` on the machine when you use a profile without the Development override, or when `host.docker.internal` resolves on your OS.

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
2. Copy `nginx.prod.conf`, `conf.d/site.prod.conf`, `proxy_params.conf`, and `cache_params.conf` to the VM.
3. Replace `fgs.example.com` in `site.prod.conf` with the real hostname.
4. Mount production certificates at `/etc/nginx/certs/tls.crt` and `/etc/nginx/certs/tls.key`.
5. Run NGINX in Docker or install NGINX directly on the VM.
6. Keep microservices on a private Docker network or private VM subnet.
7. Publish only ports `80` and `443` from NGINX.

For direct VM NGINX:

```sh
sudo nginx -t
sudo systemctl reload nginx
```

## Kubernetes Path Later

For Kubernetes, keep this routing model but move responsibilities as follows:

- Use Kubernetes `Service` objects for `user-service`, `notification-service`, `setup-service`, `file-service`, `publisher-service`, `consumer-service`, `crm-service`, `scheduling-service`, `billing-service`, `inventory-service`, `reporting-service`, `integration-service`, `asset-service`, `service-agreement-service`, and `communication-service`.
- Put TLS certificates in `kubernetes.io/tls` secrets, or use cert-manager.
- Use NGINX Ingress Controller for path routing and prefix rewrite annotations.
- Keep `/api/v1/users`, `/api/v1/notifications`, `/api/v1/credentials`, `/api/v1/communication-templates`, `/api/v1/tenants`, `/api/v1/crm`, `/api/v1/scheduling`, `/api/v1/billing`, `/api/v1/inventory`, `/api/v1/reporting`, `/api/v1/integration`, `/api/v1/asset`, `/api/v1/service-agreements`, and `/api/v1/communication` as the stable external contract.
- Move rate limiting, body size, gzip, timeouts, and security headers into Ingress annotations or a controller ConfigMap.

The production NGINX files remain useful as the reference edge policy when converting to Ingress resources.
