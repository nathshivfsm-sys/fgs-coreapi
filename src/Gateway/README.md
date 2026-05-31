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
    platform-service.Dockerfile
    workorder-service.Dockerfile
  logs/
  scripts/
    generate-local-cert.ps1
    generate-local-cert.sh
```

PostgreSQL and RabbitMQ are **not** part of this Compose file. Start them on the host before bringing up the gateway stack (for example `docker compose up -d` in `src/UserService` for RabbitMQ, plus your local PostgreSQL instance).

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
| `/api/v1/platform` | `platform-service:5002` | `/api/v1/` |
| `/api/v1/platform/{path}` | `platform-service:5002` | `/api/v1/{path}` |
| `/api/v1/workorders` | `workorder-service:5003` | `/api/v1/` |
| `/api/v1/workorders/{path}` | `workorder-service:5003` | `/api/v1/{path}` |

OAuth and invitation URLs are exposed through the gateway (register the same values in Microsoft Entra):

| Setting | Local gateway value |
| --- | --- |
| `EntraExternalId:RedirectUri` | `https://localhost:8443/api/v1/auth/entra/callback` |
| `Invitation:InviteBaseUrl` | `https://localhost:8443/api/v1/invite/start` |

Both upstreams use `least_conn`, keepalive connections, passive health checks with `max_fails` and `fail_timeout`, and Docker health checks against each service's `/health` endpoint.

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
curl.exe -k https://localhost:8443/api/v1/platform/health
curl.exe -k https://localhost:8443/api/v1/workorders/health
```

The local Compose file starts:

- `nginx`, published on host ports `8080` and `8443`.
- `user-service`, private on container port `5001`.
- `platform-service`, private on container port `5002`.
- `workorder-service`, private on container port `5003`.

### Application configuration (Postgres, RabbitMQ, Entra)

Each API container mounts the **same** files you edit for local `dotnet run`:

| Service | Mounted files |
| --- | --- |
| User | `src/UserService/Fgs.User.API/appsettings.json` + `appsettings.Development.json` |
| Platform | `src/PlatformService/Fgs.Platform.API/appsettings.json` + `appsettings.Development.json` |
| Workorder | `src/WorkOrderService/Fgs.WorkOrder.API/appsettings.json` + `appsettings.Development.json` |

Containers use `ASPNETCORE_ENVIRONMENT=Development`, so ASP.NET Core **merges** `appsettings.json` then `appsettings.Development.json` (same as Visual Studio / `dotnet run`). There are no duplicate Postgres or RabbitMQ settings in `docker-compose.yml`.

Change connection strings or RabbitMQ in those JSON files and restart the service container; no image rebuild is required for config-only changes.

`appsettings.Development.json` overrides `Host` / `HostName` to `host.docker.internal` so containers can reach Postgres and RabbitMQ on the host. Base `appsettings.json` keeps `localhost` for `dotnet run` on the machine when you use a profile without the Development override, or when `host.docker.internal` resolves on your OS.

## Scale Services Locally

Do not add `container_name`; Docker Compose needs generated names for scaling.

```powershell
docker compose up --build --scale user-service=2 --scale platform-service=2 --scale workorder-service=2
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

- Use Kubernetes `Service` objects for `user-service`, `platform-service`, and `workorder-service`.
- Put TLS certificates in `kubernetes.io/tls` secrets, or use cert-manager.
- Use NGINX Ingress Controller for path routing and prefix rewrite annotations.
- Keep `/api/v1/users`, `/api/v1/platform`, and `/api/v1/workorders` as the stable external contract.
- Move rate limiting, body size, gzip, timeouts, and security headers into Ingress annotations or a controller ConfigMap.

The production NGINX files remain useful as the reference edge policy when converting to Ingress resources.
