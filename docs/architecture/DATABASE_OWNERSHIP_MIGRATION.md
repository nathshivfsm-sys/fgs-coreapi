# Database Ownership Migration

This document records the service-per-schema database ownership split executed from the monolithic `FgsUserDbContext` (UserService) into per-service DbContexts with fresh baseline migrations.

## Target ownership

| Service | PostgreSQL schema(s) | Connection string key | Baseline migration |
|---------|---------------------|----------------------|-------------------|
| **UserService** | `identity`, `tenant` | `FgsUser` | `20260603225414_InitialIdentityTenant` |
| **SetupService** | `setup`, `glo` | `FgsSetup` | `20260603225519_InitialSetupGlo` |
| **FileService** | `file` | `FgsFile` | `20260603225607_InitialFile` |
| **AuditService** | `audit` | `FgsAudit` | `20260603225652_InitialAudit` |
| **NotificationService** | `notification` | `FgsNotification` | `20260603222551_InitialSchema` |
| **BillingService** | `billing` (placeholder) | `FgsBilling` | `20260603212540_InitialSchema` |
| **CrmService** | `crm` (placeholder) | `FgsCrm` | `20260603212808_InitialSchema` |
| **DispatchService** | `dispatch` (placeholder) | `FgsDispatch` | `20260603213051_InitialSchema` |
| **InventoryService** | `inventory` (placeholder) | `FgsInventory` | `20260603213321_InitialSchema` |
| **ReportingService** | `reporting` (placeholder) | `FgsReporting` | `20260603214016_InitialSchema` |
| **JobService** | `workflow` (placeholder) | `FgsJob` | `20260603214323_InitialSchema` |

> **Dev vs prod:** All services can share one PostgreSQL instance using schema-per-service. Each DbContext uses its own connection string name so physical DB separation later requires only connection string changes. See [`init-postgres.sql`](../Gateway/scripts/init-postgres.sql) for per-service database creation.

## Entity move matrix (summary)

### UserService (`identity` + `tenant`)

| Entity | Schema |
|--------|--------|
| `FgsUser`, `FgsUserRole`, `FgsRole`, `FgsInvitation` | `identity` |
| `FgsTenant`, `FgsTenantCompany`, `FgsTenantServiceSetup`, `FgsLocation` | `tenant` |

### SetupService (`setup` + `glo`)

All former monolith `glo.*` reference tables, all former `setup.*` tenant configuration tables (including interim inventory/billing/dispatch tables still physically in `setup`), and tag tables relocated from deprecated `shared`:

- `FgsTag`, `FgsEntityTag`, `FgsTagEntityType` → `setup`
- All `Glo*` entities → `glo`
- Credentials: `GloCredential*`, `FgsCredential` → `setup` / `glo`
- Outbox: `GloOutboxMessage` → `glo`
- Communication templates: `FgsSetupCommunicationTemplate`, `GloCommunicationTemplate*` → `setup` / `glo`

See `Fgs.Setup.Infrastructure/Database/Schemas/EntitySchemaRegistry.cs` for the authoritative entity→schema map.

### FileService (`file`)

| Entity | Schema |
|--------|--------|
| `FgsFile` | `file` |

### AuditService (`audit`)

| Entity | Schema |
|--------|--------|
| `FgsCredentialAudit` | `audit` |

### NotificationService (`notification`)

| Entity | Schema |
|--------|--------|
| `FgsNotificationHistory` | `notification` |
| `FgsProcessedIntegrationEvent` | `notification` |

**Removed:** duplicate `FgsSetupCommunicationTemplate` table — templates are read from SetupService via Refit.

## Removed cross-service foreign keys

Cross-service FKs were replaced with indexed scalar columns validated via API/events:

| Dependent | Former FK target | Replacement |
|-----------|-------------------|-------------|
| `identity.*` | `tenant.FgsTenantCompany` | `TenantId` / `CompanyId` columns; UserService validates |
| `tenant.FgsTenant` | `glo.GloSetupTenantStatus` | `FgsTenantStatusId` scalar; SetupService owns lookup |
| `setup.*` tenant rows | `tenant.FgsTenantCompany` | Removed `ConfigureTenantCompanySetupFk`; scalar IDs only |
| `setup.FgsSetupGLBreak` | `tenant.FgsLocation` | `AddressId` Guid; resolve via UserService |
| `setup.FgsWarehouse` | `tenant.FgsLocation` | `LocationId` Guid |
| `setup`/`glo` tag tables | `file.FgsFile` (icon) | `IconFileId` scalar; resolve via FileService |
| `audit.FgsCredentialAudit` | `setup.FgsCredential` | `CredentialId` long scalar |

**Preserved:** `setup`↔`glo` FKs within SetupService (single DbContext, tightly coupled reference data).

## Migration artifacts

Each owning service stores:

```text
Infrastructure/Database/
├── Migrations/
├── Scripts/
│   ├── Execute/    # {MigrationId}_up.sql
│   └── Rollback/   # {MigrationId}_down.sql
└── Seeds/
```

Generate new migrations with [`scripts/generate-migration-sql.ps1`](../../scripts/generate-migration-sql.ps1):

```powershell
./scripts/generate-migration-sql.ps1 -ServiceName Setup -MigrationName AddSomeFeature
```

## Seed script locations

| Seed | Owner | Path |
|------|-------|------|
| Platform tenant (Id 0) | UserService | `Fgs.User.Infrastructure/Database/Seeds/Platform_Tenant_Seed.sql` |
| Global + glo reference data | SetupService | `Fgs.Setup.Infrastructure/Database/Seeds/Initial_Migration_Seed.sql` |
| Glo seed rollback | SetupService | `Fgs.Setup.Infrastructure/Database/Seeds/Initial_Migration_Seed_Down.sql` |

## Refit / integration contracts

Added in `Fgs.Contracts`:

| Contract | Purpose |
|----------|---------|
| `IUserTenantClient` | Tenant status updates, tenant/company reads during provisioning |
| `IFileTenantClient` | S3 bucket provisioning during tenant onboarding |
| `ISetupTemplateClient` | Communication template reads for NotificationService |
| `TenantProvisionCompletedEvent` | Published when Setup finishes seeding + File bucket creation |
| `TenantProvisionRequestedEvent` | Consumed by SetupService to start provisioning |

### Provisioning flow

```text
RabbitMQ: TenantProvisionRequested
  → SetupService: IUserTenantClient.UpdateStatus(Provisioning)
  → TenantDataSeedingEngine seeds glo/setup tables (Setup DB only)
  → IFileTenantClient.ProvisionBucket(tenantId)
  → IUserTenantClient.UpdateStatus(Active) + TenantProvisionCompletedEvent
```

## Deprecated

- **`shared` schema** — all tables relocated; `__EFMigrationsHistory` is per-service.
- **Monolithic UserService migrations** — 19 legacy migrations removed; replaced by service baselines above.

## Future extraction roadmap

When domain microservices mature, move tables currently interim-stored in `setup` to their owning schemas with a second migration wave:

1. Inventory tables → `inventory` schema (InventoryService)
2. Billing tables → `billing` schema (BillingService)
3. Dispatch tables → `dispatch` schema (DispatchService)
4. CRM tables → `crm` schema (CrmService)

Placeholder migrations exist today; no domain entities until extraction.

## Validation checklist

| Check | How |
|-------|-----|
| Entity assignment | Compare each service `EntitySchemaRegistry` vs tables above |
| Schema single-owner | No overlapping schema constants across DbContexts |
| Cross-schema FKs removed | Query `pg_constraint` per schema after migrate |
| Seeds in owner | Glo seed only in Setup; platform tenant seed only in User |
| Execute/Rollback pairs | Every migration ID has both `{id}_up.sql` and `{id}_down.sql` |
| No cross-service DB access | Cross-service via Refit/events only |
| DB-per-service ready | Each service `dotnet ef database update` with isolated connection string |

## Existing environment cutover

Deployed environments cannot replay deleted UserService migrations. Use a **baseline cutover**:

1. Apply new service baselines to empty schemas, or
2. Run one-time DDL to move tables + drop cross-service FKs, then insert baseline rows into each service `__EFMigrationsHistory` without replaying DDL.

Greenfield dev environments can bootstrap from scratch using per-service `dotnet ef database update` and seed scripts.
