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
    job-service.Dockerfile
    setup-service.Dockerfile
    file-service.Dockerfile
    publisher-service.Dockerfile
    consumer-service.Dockerfile
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
| `/api/v1/jobs` | `job-service:5003` | `/api/v1/` |
| `/api/v1/jobs/{path}` | `job-service:5003` | `/api/v1/{path}` |
| `/api/v1/credentials/{path}` | `setup-service:5004` | KMS-backed credential admin |
| `/api/v1/communication-templates/{path}` | `setup-service:5004` | Template reads for Notification |
| `/api/v1/tenants/{path}` | `file-service:5005` | S3 bucket and folder provisioning |

### Database-backed services (local dev)

Each service uses its own connection string (`FgsUser`, `FgsSetup`, `FgsFile`, etc.). PostgreSQL init script: [`scripts/init-postgres.sql`](scripts/init-postgres.sql). Ownership map: [`docs/architecture/DATABASE_OWNERSHIP_MIGRATION.md`](../../docs/architecture/DATABASE_OWNERSHIP_MIGRATION.md).

Generate EF SQL scripts: [`scripts/generate-migration-sql.ps1`](../../scripts/generate-migration-sql.ps1).

OAuth and invitation URLs are exposed through the gateway (register the same values in Microsoft Entra):

| Setting | Local gateway value |
| --- | --- |
| `EntraExternalId:RedirectUri` | `https://localhost:8443/api/v1/auth/entra/callback` |
| `Invitation:InviteBaseUrl` | `https://localhost:8443/api/v1/invite/start` |

Both upstreams use `least_conn`, keepalive connections, passive health checks with `max_fails` and `fail_timeout`, and Docker health checks against each service's `/health` endpoint.

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

Public-facing URLs (OAuth, invites, dashboard) use the NGINX gateway at `https://localhost:8443`.

## Run Locally

From `C:\SourceCode\FGS\src\Gateway`:

```powershell
.\scripts\generate-local-cert.ps1
docker compose up --build
```

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
curl.exe -k https://localhost:8443/api/v1/jobs/health
```

Container health checks use each service's `/health` endpoint (see Dockerfiles). Setup and File API routes are reachable under `/api/v1/credentials/` and `/api/v1/tenants/` without a path prefix rewrite.

The local Compose file starts services in credential-safe order:

1. `rabbitmq` — host ports `5672` / `15672`
2. `setup-service` (`5004`) — credential authority; bootstraps `ConnectionStrings:FgsSetup` from mounted appsettings (no dependency on other FGS APIs)
3. Credential consumers (wait for Setup `/health`): `audit-service` (`5008`), `user-service` (`5001`), `file-service` (`5005`), `notification-service` (`5002`), `job-service` (`5003`)
4. Messaging workers: `publisher-service` (`5006`), `consumer-service` (`5007`)
5. `nginx` — host ports `8080` / `8443` (waits for gateway upstream health checks)

Scaffold APIs (Billing, Crm, Contract, etc.) are not containerized here; run them with `dotnet run` against Setup when needed.

### Credential bootstrap environment variables

Consuming services load secrets from Setup Service at startup (`GET /api/v1/credentials/resolved`). Configure these bootstrap values (non-secret) in appsettings or Docker env:

| Variable / setting | Purpose |
| --- | --- |
| `SetupService__BaseUrl` | Setup Service URL for `ISetupClient` |
| `CredentialDistribution__InternalServiceKey` | S2S key for `/credentials/resolved` |
| `CredentialConsumer__ServiceName` | Service identity for access audit |
| `CredentialConsumer__RequiredProviders__0` | Provider filter (e.g. `DATABASE`, `SENDGRID`) |
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
| Workorder | `src/JobService/Fgs.Job.API/appsettings.json` + `appsettings.Development.json` |
| Setup | `src/SetupService/Fgs.Setup.API/appsettings.json` + `appsettings.Development.json` |
| Audit | `src/AuditService/Fgs.Audit.API/appsettings.json` + `appsettings.Development.json` |
| File | `src/FileService/Fgs.File.API/appsettings.json` + `appsettings.Development.json` |
| Publisher | `src/PublisherService/Fgs.Publisher.API/appsettings.json` + `appsettings.Development.json` |
| Consumer | `src/ConsumerService/Fgs.Consumer.API/appsettings.json` + `appsettings.Development.json` |

Containers use `ASPNETCORE_ENVIRONMENT=Development`, so ASP.NET Core **merges** `appsettings.json` then `appsettings.Development.json` (same as Visual Studio / `dotnet run`). There are no duplicate Postgres settings in `docker-compose.yml`.

Change connection strings in those JSON files and restart the service container; no image rebuild is required for config-only changes.

`appsettings.Development.json` overrides `Host` to `host.docker.internal` so containers can reach Postgres on the host. Base `appsettings.json` keeps `localhost` for `dotnet run` on the machine when you use a profile without the Development override, or when `host.docker.internal` resolves on your OS.

## Scale Services Locally

Do not add `container_name`; Docker Compose needs generated names for scaling.

```powershell
docker compose up --build --scale user-service=2 --scale notification-service=2 --scale job-service=2 --scale setup-service=2 --scale file-service=2
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

- Use Kubernetes `Service` objects for `user-service`, `notification-service`, `job-service`, `setup-service`, `file-service`, `publisher-service`, and `consumer-service`.
- Put TLS certificates in `kubernetes.io/tls` secrets, or use cert-manager.
- Use NGINX Ingress Controller for path routing and prefix rewrite annotations.
- Keep `/api/v1/users`, `/api/v1/notifications`, `/api/v1/jobs`, `/api/v1/credentials`, `/api/v1/communication-templates`, and `/api/v1/tenants` as the stable external contract.
- Move rate limiting, body size, gzip, timeouts, and security headers into Ingress annotations or a controller ConfigMap.

The production NGINX files remain useful as the reference edge policy when converting to Ingress resources.
