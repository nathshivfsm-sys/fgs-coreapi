# UserService

ASP.NET Core API for tenant and user signup, backed by PostgreSQL (Entity Framework Core) and optional Azure Service Bus integration events.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- PostgreSQL 14+ with the `citext` extension available (used for case-insensitive email columns)

## Configuration

Edit `UserService.API/appsettings.json` (or use environment variables / user secrets for secrets):

| Area | Setting | Purpose |
|------|---------|---------|
| Database | `ConnectionStrings:UserServiceDb` | Npgsql connection string |
| Microsoft Entra | `Entra:*` | Graph and identity options when provisioning users |
| Messaging | `ServiceBus:ConnectionString` | If empty, integration events are logged only (no publish) |
| Messaging | `ServiceBus:InviteEventsPath` | Service Bus queue/topic path for invite events |

## Run locally

From this directory (`UserService`):

```bash
dotnet run --project UserService.API
```

In Development, OpenAPI and Swagger UI are enabled:

- Swagger UI: `https://localhost:<port>/swagger`
- OpenAPI document: `/openapi/v1.json`

## Database migrations

Migrations live in `UserService.Infrastructure/Persistence/Migrations`. Apply to your database:

```bash
dotnet ef database update --project UserService.Infrastructure --startup-project UserService.API
```

Ensure `ConnectionStrings:UserServiceDb` points at the target database before running.

## Solution layout

| Project | Role |
|---------|------|
| `UserService.API` | HTTP API, filters, Swagger |
| `UserService.Application` | MediatR commands/handlers, validation |
| `UserService.Domain` | Entities and domain events |
| `UserService.Infrastructure` | EF Core, Azure integrations, security helpers |
| `UserService.Tests` | Unit tests |

## API example

- `POST /api/signup/company` — company signup (see `CreateCompanySignupRequest` for the request body)
