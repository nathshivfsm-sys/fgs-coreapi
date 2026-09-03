# Knowledge validation

Validated against the repository on 2026-09-03. Application/business code was **not** modified.

## Verified architecture

- .NET 10 microservices, Clean Architecture layers, MediatR CQRS, `ApiResponse<T>`
- Shared libs under `src/Shared/` (9 areas)
- Edge: NGINX Gateway (not YARP)
- Events: outbox → RabbitMQ (not Kafka)

## Services

See [services.md](services.md). Mature REST: User, Setup, File, Audit, Notification, Inventory, Asset. Partial: Billing, Crm, Scheduling, ServiceAgreement. Scaffold: Reporting, Integration, Communication. Workers: Publisher, Consumer. Orchestrator: BFF.

## Database

Schema-per-service ownership confirmed via `FgsDatabaseSchemas` / registries. Audit columns `CreatedOn`/`CreatedBy`/`UpdatedOn`/`UpdatedBy`. Soft delete `IsActive`.

## Authentication

Entra External ID JWT Bearer. Credentials via Redis snapshot. S2S internal service key. No platform JWT issuer.

## Authorization

Fallback authenticated user + `[RequirePermission]`. RBAC tables in `identity`. `TENANT_ADMIN` bypass.

## Multi-tenancy

`X-Tenant-Id` / `X-Company-Id` (`long`). `FgsTenantFilteredDbContext`. Single-company users.

## Messaging

RabbitMQ + outbox poller in Publisher. Consumer Redis idempotency. No inbox table.

## Testing

xUnit/Moq/FluentAssertions. No Testcontainers/WebApplicationFactory suite.

## Deployment

Local Gateway compose; CI → ECR; CD primarily EC2 SSM. ECS reusable workflow present but unused by current build callers.

## Skills created (12)

`create-api`, `create-command`, `create-query`, `create-setup-entity`, `database-change`, `implement-outbox`, `implement-consumer`, `authorization`, `multi-tenancy`, `add-unit-tests`, `code-review`, `debug-api`

**Skipped (not applicable):** `add-integration-tests`, standalone `authentication`, standalone `add-migration` (folded into `database-change`).

## Rules created (10)

`agent-workflow`, `project-architecture`, `dotnet-coding-standards`, `api-development`, `cqrs-mediatr`, `database-standards`, `multi-tenancy`, `security`, `messaging`, `testing`

## Documentation created

- `docs/ai/` core refs (14 markdown files including this after write)
- `docs/ai/features/` (16 domains + index)
- Preserved existing `.cursor/*.md` and `docs/architecture/*`

## Path checks

Referenced solution, gateway routes, Setup CRUD template/script, `FgsApiControllerBase`, `ApiResponse`, `OutboxWriter`, permission seed, Datadog doc: **all exist**.

## Known inconsistencies

1. `.cursor/rules.md` aspirational (Kafka, UUID tenancy, multi-company membership) vs code (RabbitMQ, `long`, single company).
2. Controllers mixed: `FgsApiControllerBase` vs `ControllerBase`+`StatusCode` (Setup/Asset/Inventory/Billing/Crm/…).
3. Setup still seeds/writes `inventory` in places (cross-service DB debt).
4. Publisher reads other services’ outbox schemas (intentional worker exception).
5. Scorecard date/maturity may lag (e.g. Billing/Crm/Scheduling now have partial APIs).
6. `ApiResponse` lives in **Contracts** (older Shared review said Foundation).
7. Some docs mention ProblemDetails; runtime envelope is `ApiResponse`.
8. Scheduling Appointment lacks `RequirePermission` unlike peers.

## Missing / uncertain

- Exact Entra claim names for tenant/company inside issued access tokens beyond connector DTO
- Prod physical DB topology (schemas vs separate instances)
- Whether ECS will replace EC2 CD
- Full inventory of every Setup catalog table (use EntitySchemaRegistry)
- OpenSearch / AI / GPS features from product vision — **not implemented** in this backend tree
