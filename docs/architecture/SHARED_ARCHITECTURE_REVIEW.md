# FGS Shared Architecture Review & Refactoring Report

**Date:** 2026-05-30 (updated 2026-06-04 for database ownership split)  
**Scope:** `src/Shared/*`, UserService, NotificationService, JobService, SetupService, FileService, AuditService  
**Target:** Enterprise microservices on .NET 10 with Clean Architecture, CQRS, DDD, SOLID

> **Database ownership (2026-06):** The monolithic `FgsUserDbContext` was split into per-service DbContexts (`identity`/`tenant` → User, `setup`/`glo` → Setup, `file` → File, `audit` → Audit, `notification` → Notification). See [DATABASE_OWNERSHIP_MIGRATION.md](./DATABASE_OWNERSHIP_MIGRATION.md).

---

## Executive Summary

The FGS codebase had **no shared libraries** despite documented target architecture. Cross-cutting concerns were duplicated across UserService and PlatformService, integration event contracts were copy-pasted (drift risk), and UserService acted as a **modular monolith** (~107 domain entities spanning identity, tenant, inventory, billing, and setup).

This refactoring introduces **eight focused shared building-block projects** under `src/Shared/` and wires all three microservices to them. The solution builds successfully (`src/FGS.slnx`).

---

## 1. Architecture Review Findings

### Before

| Area | State |
|------|-------|
| Shared projects | None |
| `ApiResponse<T>` | Duplicated in User + Platform Application |
| Middleware | CorrelationId duplicated; ExceptionHandling User-only |
| Integration events | Duplicated per service; manual sync comments |
| Base entities | Only in UserService.Domain |
| Repository/UoW | Only in UserService |
| Tenant context | No `ITenantContext`; manual threading |
| Observability | Basic health checks only; no OpenTelemetry |
| Solution | Three isolated `.slnx` files |

### After

| Project | Responsibility |
|---------|----------------|
| **Fgs.Kernel** | Domain primitives: `FgsEntityBase`, `GloEntityBase`, `IDomainEvent`, `DomainException`, `ISpecification` |
| **Fgs.Foundation** | Cross-cutting: `ApiResponse`, middleware, MediatR behaviors, correlation, `UseFgsFoundationMiddleware()` |
| **Fgs.Contracts** | Integration events, routing keys, exchanges, template codes |
| **Fgs.Messaging** | `IOutboxWriter`, `IMessagePublisher`, `RabbitMqOptions`, JSON serializers |
| **Fgs.Persistence** | `IRepository`, `IUnitOfWork` abstractions |
| **Fgs.Security** | `JwtClaimTypes`, `JwtOptions` |
| **Fgs.MultiTenancy** | `ITenantContext`, `TenantResolutionMiddleware`, `TenantScopeConstants` (0/0 platform scope) |
| **Fgs.Observability** | Standard health endpoints (`/health`, `/health/ready`, `/health/live`) |

---

## 2. Shared Project Restructuring (Implemented)

```text
src/
├── FGS.slnx                          # Unified solution
├── Shared/
│   ├── Kernel/Fgs.Kernel/
│   ├── Foundation/Fgs.Foundation/
│   ├── Contracts/Fgs.Contracts/
│   ├── Messaging/Fgs.Messaging/
│   ├── Persistence/Fgs.Persistence/
│   ├── Security/Fgs.Security/
│   ├── MultiTenancy/Fgs.MultiTenancy/
│   └── Observability/Fgs.Observability/
├── UserService/
├── PlatformService/
└── WorkOrderService/
```

### Dependency Rules

```text
Fgs.Kernel          → (no dependencies)
Fgs.Contracts       → (no dependencies)
Fgs.Persistence     → (no dependencies)
Fgs.Security        → (no dependencies)
Fgs.Messaging       → Fgs.Contracts
Fgs.MultiTenancy    → Fgs.Kernel, Fgs.Security
Fgs.Foundation      → Fgs.Kernel, Fgs.MultiTenancy
Fgs.Observability   → Fgs.Foundation

Microservice.Domain       → Fgs.Kernel
Microservice.Application  → Kernel, Foundation, Contracts, Persistence, Messaging
Microservice.Infrastructure → Application + Messaging, Security, MultiTenancy
Microservice.API          → Foundation, MultiTenancy, Observability
```

---

## 3. Components That Remain in Shared Projects

| Component | Project | Notes |
|-----------|---------|-------|
| `FgsEntityBase`, `GloEntityBase`, `FgsTenantCompanySetupEntityBase` | Kernel | All services inherit these |
| `ApiResponse<T>`, `ApiStatusCodes` | Foundation | Unified API envelope |
| `CorrelationIdMiddleware`, `ExceptionHandlingMiddleware`, `RequestResponseLoggingMiddleware`, `SecurityHeadersMiddleware` | Foundation | Via `UseFgsFoundationMiddleware()` |
| `ValidationBehavior`, `LoggingBehavior` | Foundation | MediatR pipeline |
| `ICorrelationContext`, `HttpCorrelationContext` | Foundation | Correlation propagation |
| `IExceptionStatusMapper` | Foundation | Service-specific exception mapping |
| Integration event records + routing keys | Contracts | Single source of truth |
| `IRepository`, `IUnitOfWork` | Persistence | Abstractions only |
| `IOutboxWriter`, `RabbitMqOptions`, JSON serializers | Messaging | Shared broker config |
| `JwtClaimTypes`, `JwtOptions` | Security | Claim constants + options shape |
| `ITenantContext`, `TenantResolutionMiddleware` | MultiTenancy | Header + JWT claim resolution |
| `MapFgsHealthChecks()` | Observability | Consistent health endpoints |

---

## 4. Components That Must Stay in Microservices

| Component | Owner | Reason |
|-----------|-------|--------|
| `FgsUser`, `FgsTenant`, `FgsTenantCompany`, `FgsLocation` (identity/tenant) | UserService.Domain | Bounded context ownership |
| `FgsUserDbContext`, EF configurations, migrations | UserService.Infrastructure | User DB (`identity`, `tenant`) only |
| `FgsSetupDbContext`, glo/setup entities, provisioning, credentials | SetupService | Reference + tenant setup data |
| `Repository<T>`, `UnitOfWork` implementations | Each service Infrastructure | Per-service DbContext |
| `OutboxWriter` / `GloOutboxStore` | SetupService (glo outbox) | Cross-service events via RabbitMQ |
| `JwtTokenService`, `EntraExternalIdService` | UserService.Infrastructure | User identity implementation |
| Credential feature (commands, handlers, AWS KMS) | SetupService | Credential store in Setup DB |
| Notification subsystem | PlatformService | Platform bounded context |
| `NotificationQueueWorker`, `IntegrationEventMapper` | PlatformService.Infrastructure | Consumer-side logic |
| Feature commands/queries/handlers | Each service Application | CQRS — never shared |
| Service-specific validators | Each service Application | FluentValidation per feature |
| `ConfigurationKeys`, `ApplicationUrlDefaults` | UserService.Application | User-specific config keys |

---

## 5. Middleware Standardization

All APIs now register middleware consistently:

```csharp
app.UseFgsFoundationMiddleware(options =>
{
    options.OmitRequestBodyLoggingForPath = path =>
        path.StartsWithSegments("/api/credentials", StringComparison.OrdinalIgnoreCase);
    options.UseSecurityHeaders = true;
    options.UseRequestResponseLogging = true;
    options.UseExceptionHandling = true;
});
app.UseFgsTenantResolution();
app.MapFgsHealthChecks();
```

Service-specific exception mapping via `IExceptionStatusMapper` (e.g. `CredentialSecretsExceptionMapper` in UserService).

**Recommended middleware order:** ForwardedHeaders → Foundation (Correlation → SecurityHeaders → Logging → Exception) → TenantResolution → HTTPS → Auth → Endpoints.

---

## 6. Messaging Architecture Recommendations

### Implemented
- Unified `RabbitMqOptions` in `Fgs.Messaging`
- Shared integration event contracts in `Fgs.Contracts`
- `IntegrationEventJsonSerializerOptions` with flexible `long` parsing (legacy GUID company IDs → 0)
- `IOutboxWriter` abstraction in `Fgs.Messaging`

### Recommended Next Steps
1. **Extract `RabbitMqPublisher` + `RabbitMqConnectionFactory`** into `Fgs.Messaging` with service-specific topology hooks
2. **Add inbox pattern** (`FgsProcessedIntegrationEvent` → shared abstraction + Platform keeps implementation)
3. **Version integration events** — add `SchemaVersion` field to all contract records
4. **Dead-letter handling** — shared DLQ retry policy interface; Platform already has DLQ queue names in options
5. **Outbox polling** — extract generic `OutboxProcessorBackgroundService<TOutboxEntity>` with service-specific entity mapping
6. **CloudEvents envelope** — optional wrapper for cross-cloud portability

---

## 7. Multi-Tenancy Recommendations

### Implemented
- `ITenantContext` / `ITenantContextAccessor`
- `HeaderAndClaimTenantResolver` (X-Tenant-Id, X-Company-Id, JWT claims)
- `TenantScopeConstants.PlatformTenantId = 0`, `PlatformCompanyId = 0` for global credentials

### Recommended Next Steps
1. **Seed platform sentinel rows** — `FgsTenant(Id=0)` + `FgsTenantCompany(TenantId=0, CompanyNumber=0)` for FK compliance
2. **Enforce tenant on all queries** — global query filters in EF or specification pattern
3. **Propagate tenant in all integration events** — already present; add validation at publish time
4. **Tenant-aware logging** — enrich `ILogger` scope with `TenantId`/`CompanyId` in `TenantResolutionMiddleware`
5. **Authorization policies** — `RequireTenantAccess`, `RequirePlatformAdmin` using `ITenantContext`

---

## 8. Security Recommendations

| Area | Status | Recommendation |
|------|--------|----------------|
| JWT claim types | Centralized in `Fgs.Security` | Gateway validates TLS; services validate JWT via `AddFgsJwtAuthentication` |
| Secret handling | UserService AWS Secrets Manager | Never log credential routes (implemented via omit path) |
| Exception handling | Generic + mapper | No stack traces to clients in production |
| Security headers | Added in Foundation | Add CSP, HSTS in production config |
| Multi-tenant isolation | Partial | Enforce at repository/query level, not just API params |
| `[Authorize]` on CredentialsController | Missing | Add platform-admin policy for `(0,0)` credentials |

---

## 9. Observability Recommendations

### Implemented
- `/health`, `/health/ready`, `/health/live` via `MapFgsHealthChecks()`
- Correlation ID in all request logs

### Recommended Next Steps
1. **OpenTelemetry** — add `Fgs.Observability` exporters (OTLP) for traces, metrics, logs
2. **Structured logging** — Serilog enrichers for CorrelationId, TenantId, CompanyId
3. **RabbitMQ metrics** — publish/consume counters, DLQ depth
4. **Outbox lag metric** — time since oldest unpublished message
5. **Health check dependencies** — PostgreSQL, RabbitMQ, AWS in `/health/ready`

---

## 10. SOLID Principle Violations (Identified)

| Principle | Violation | Severity |
|-----------|-----------|----------|
| **SRP** | UserService owns identity + tenant + inventory + billing + setup (~107 entities) | Critical |
| **OCP** | Duplicated middleware/behaviors before refactor | High (mitigated) |
| **DIP** | Integration events duplicated per service before refactor | High (mitigated) |
| **ISP** | Platform Application has many placeholder reporting interfaces | Low |
| **LSP** | N/A significant | — |

---

## 11. Clean Architecture Violations (Identified)

1. **UserService.Infrastructure/Common/** acted as hidden shared library (JWT, RabbitMQ, Geo, Time) — partially extracted
2. **Single DbContext** in UserService spans multiple bounded contexts (identity, tenant, inventory, billing)
3. **Platform duplicated `FgsSetupCommunicationTemplate`** entity — consider read-model or API contract instead
4. **WorkOrderService** had no shared infrastructure — now references Foundation/Contracts/Persistence

---

## 12. DDD Violations (Identified)

1. **UserService is not a single bounded context** — it aggregates identity, tenant lifecycle, global catalog (`Glo*`), inventory, billing setup, credentials
2. **No aggregate roots** — entities are mostly anemic; `AggregateRoot` abstraction added to Kernel for future use
3. **Domain events not raised from entities** — outbox writes happen in application handlers directly
4. **Shared kernel too large in UserService.Domain** — `Glo*` catalog should move to Platform or dedicated Catalog service

---

## 13. Microservice Anti-Patterns (Identified)

| Anti-Pattern | Present? | Mitigation |
|--------------|----------|------------|
| Shared database | Yes — UserService single PostgreSQL multi-schema | Split schemas per service over time |
| Distributed monolith | Risk — User publishes, Platform consumes same DB concepts | Contracts project (done) |
| Smart endpoints, dumb pipes | Partial — direct DB in both services for templates | Event-driven only for notifications |
| No API gateway | Mitigated | NGINX edge gateway in `src/Gateway/` |
| Chatty integration | Low currently | Keep async events, avoid sync cross-service calls |
| Common library dumping ground | Was imminent | Eight focused projects with strict rules |

---

## 14. Performance & Scalability

| Area | Finding | Recommendation |
|------|---------|----------------|
| DI lifetimes | CorrelationContext = Scoped ✓, RabbitMqPublisher = Singleton ✓ | Document in each service |
| Middleware order | Standardized | Avoid duplicate logging |
| Outbox polling | Single background service in UserService | Batch publish; index on unpublished |
| RabbitMQ | Publisher confirms not verified | Enable publisher confirms for critical events |
| Memory | `MemorySecretCache` in UserService | Replace with Redis for multi-instance |
| EF retry | `EnableRetryOnFailure` on UserService ✓ | Keep execution strategy for transactions |

---

## 15. Production Readiness

| Item | Status |
|------|--------|
| Unified solution build | ✅ `dotnet build src/FGS.slnx` |
| Platform tests | ✅ 22/22 passed |
| User tests | ⚠️ 111/112 passed (1 pre-existing Entra URL test failure) |
| Shared contracts | ✅ Single source |
| Standard middleware | ✅ |
| Health endpoints | ✅ |
| OpenTelemetry | ✅ ASP.NET Core + HTTP tracing (OTLP optional) |
| API Gateway | ✅ NGINX in `src/Gateway/` |
| Container images | Existing per-service docker-compose |

---

## 16. Refactoring Roadmap (Recommended Priority)

### Phase 1 — Complete (This PR)
- [x] Create eight shared projects
- [x] Extract Kernel entities, Foundation middleware/behaviors, Contracts events
- [x] Wire UserService + PlatformService + WorkOrderService skeleton
- [x] Unified `FGS.slnx`
- [x] Standard `UseFgsFoundationMiddleware()` + `UseFgsTenantResolution()`

### Phase 2 — Complete
- [x] Move `RabbitMqPublisher` / `RabbitMqConnectionFactory` to `Fgs.Messaging`
- [x] Generic outbox processor abstraction (`IOutboxStore`, `OutboxBatchProcessor`, `OutboxPollingBackgroundService`)
- [x] OpenTelemetry in `Fgs.Observability` (`AddFgsObservability(configuration, serviceName)`)
- [x] JWT authentication middleware in `Fgs.Security` (`AddFgsJwtAuthentication`)
- [x] Global credentials in `glo.GloCredential` (no tenant/company scope); tenant validators allow `>= 0` where applicable

**New shared APIs:**
- `Fgs.Messaging`: `AddFgsRabbitMqPublisher()`, `AddFgsRabbitMqConnectionFactory()`, `AddFgsOutboxProcessor()`
- `Fgs.Security`: `AddFgsJwtAuthentication(configuration)`
- `Fgs.Observability`: `AddFgsObservability(configuration, serviceName)` with optional `Observability:OtlpEndpoint`

**UserService wiring:** `GloOutboxStore`, `UserOutboxRoutingResolver`, platform tenant seed on startup, JWT auth pipeline.

**PlatformService wiring:** shared `RabbitMqConnectionFactory` + `PlatformRabbitMqTopologyInitializer` (DLQ/notification topology stays service-specific).

### Phase 3 — Domain Decomposition (Revised Scope)

**Constraints (team decision):**
- Keep all `Glo*` tables in UserService — do not extract catalog to a new service
- Do not add new microservices in this phase
- API Gateway is **NGINX** (already implemented); lives at `src/Gateway/` (moved from `deployment/nginx/`)

**Remaining Phase 3 work:**
- [x] Consolidate tenant setup tables into PostgreSQL `setup` schema (from billing, crm, dispatch, integration, inventory, notification)
- [ ] Separate DbContext per bounded context inside UserService (same PostgreSQL, existing schemas)
- [x] NGINX API Gateway under `src/Gateway/` (routes `/api/users`, `/api/platform`, `/api/workorders`)

**Deferred (not in current scope):**
- Extract `Glo*` catalog to Platform/Catalog service
- New InventoryService / BillingService projects
- Separate databases per bounded context

### Phase 4 — Enterprise Hardening
- [ ] Event schema versioning + compatibility tests
- [ ] Centralized authorization policies
- [ ] Redis distributed cache for secrets
- [ ] CI pipeline for `FGS.slnx` with contract tests between services

---

## 17. Usage Guide for New Services

When creating a new microservice (e.g. WorkOrderService):

1. Reference `Fgs.Kernel`, `Fgs.Foundation`, `Fgs.Contracts`, `Fgs.Persistence` from Application
2. Call `services.AddFgsFoundation()` in Application DI
3. Call `services.AddFgsMultiTenancy()` + `services.AddFgsObservability()` in API
4. Use `app.UseFgsFoundationMiddleware()` + `app.UseFgsTenantResolution()` + `app.MapFgsHealthChecks()`
5. Put integration events only in `Fgs.Contracts` — never duplicate
6. Keep all feature commands/queries in the service Application layer

---

## 18. Files Removed (Duplicated)

**UserService:** `ApiResponse.cs`, IntegrationEvents/*, `ValidationBehaviour.cs`, `LoggingBehaviour.cs`, base entity files, middleware copies, `RabbitMqOptions.cs`, `JwtOptions.cs`, `JwtClaimTypes.cs`, `ICorrelationContext.cs`, `IRepository.cs`, `IUnitOfWork.cs`, `IOutboxWriter.cs`, `RabbitMqPublisher.cs`, `IRabbitMqPublisher.cs`, `OutboxProcessorService.cs`, `OutboxOptions.cs`

**PlatformService:** `ApiResponse.cs`, IntegrationEvents/*, behaviors, middleware copy, `RabbitMqOptions.cs`, `CommunicationTemplateCodes.cs`, `IntegrationEventJsonSerializerOptions.cs`, `RabbitMqConnectionFactory.cs`

---

*This document should be updated as Phase 2+ work completes.*
