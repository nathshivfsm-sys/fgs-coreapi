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
| **SchedulingService** | `dispatch` (placeholder) | `FgsDispatch` | `20260603213051_InitialSchema` |
| **InventoryService** | `inventory` | `FgsInventory` | `20260627155750_AddInventoryCoreEntities` |
| **ReportingService** | `reporting` (placeholder) | `FgsReporting` | `20260603214016_InitialSchema` |
| **IntegrationService** | `integration` (placeholder) | `FgsIntegration` | `AddFgsTenantCompanyCache` |
| **AssetService** | `asset` | `FgsAsset` | `InitialSchema` |
| **ServiceAgreementService** | `svc` | `FgsServiceAgreement` | `InitialSchema` |

> **Dev vs prod:** All services can share one PostgreSQL instance using schema-per-service. Each DbContext uses its own connection string name so physical DB separation later requires only connection string changes. See [`init-postgres.sql`](../Gateway/scripts/init-postgres.sql) for per-service database creation.

## Entity move matrix (summary)

### UserService (`identity` + `tenant`)

| Entity | Schema |
|--------|--------|
| `FgsUser`, `FgsUserRole`, `FgsRole`, `FgsInvitation` | `identity` |
| `FgsTenant`, `FgsTenantCompany`, `FgsTenantServiceSetup`, `FgsLocation` | `tenant` |

### SetupService (`setup` + `glo`)

All former monolith `glo.*` reference tables and former `setup.*` tenant configuration tables (excluding inventory master/transaction tables, now in `inventory`), and tag tables relocated from deprecated `shared`:

- `FgsTag`, `FgsEntityTag`, `FgsTagEntityType` → `setup`
- All `Glo*` entities → `glo`
- Credentials: `GloCredential*`, `FgsCredential` → `setup` / `glo`
- Outbox: `GloOutboxMessage` → `glo`
- Communication templates: `FgsSetupCommunicationTemplate`, `GloCommunicationTemplate*` → `setup` / `glo`
- Price book: `FgsPriceBook`, `FgsPriceBookItem` → `setup`

See `Fgs.Setup.Infrastructure/Database/Schemas/EntitySchemaRegistry.cs` for the authoritative entity→schema map.

### InventoryService (`inventory`)

| Entity | Schema |
|--------|--------|
| `FgsInventoryItemType`, `FgsInventoryCategory`, `FgsInventorySubCategory`, `FgsVendor`, `FgsInventoryLocation`, `FgsInventoryItem`, `FgsInventoryItemAlternate`, `FgsInventoryItemDependency`, `FgsInventoryStock`, `FgsVendorInventoryItem`, `FgsInventoryTransaction`, `FgsPurchaseOrder`, `FgsPurchaseOrderDetail` | `inventory` |

No cross-schema FKs. `setup.FgsVehicle.InventoryLocationId` references `inventory.FgsInventoryLocation` as a scalar column only (validated via Inventory API/SQL).

### FileService (`file`)

| Entity | Schema |
|--------|--------|
| `FgsFile` | `file` |

### AuditService (`audit`)

| Entity | Schema |
|--------|--------|
| `FgsCredentialAudit` | `audit` |
| `FgsEvent`, `FgsEventDetail`, `FgsEventAttachment`, `FgsArchiveCatalog` | `audit` |

Postgres enums: `audit.record_type`, `audit.event_source`, `audit.event_detail_type`.

### NotificationService (`notification`)

| Entity | Schema |
|--------|--------|
| `FgsEmailHistory` | `notification` |
| `FgsSmsHistory` | `notification` |
| `FgsProcessedIntegrationEvent` | `notification` |

Postgres enums: `notification.notification_status`, `notification.source_application`.

**Removed:** `FgsNotificationHistory` (replaced by channel-specific `FgsEmailHistory` / `FgsSmsHistory`).

**Removed:** duplicate `FgsSetupCommunicationTemplate` table — templates are read from SetupService via Refit.

## Cache tables (cross-schema decoupling)

Each service owns a local `FgsTenantCompanyCache` (and Setup additionally owns `GloCredentialProviderTypeCache` / `GloResolutionTypeCache`). Tenant company cache rows are populated during **tenant provisioning** as the **first** seed step (`GloSeedTableMapping` / `GloSeedTableColumnMapping` with `SeedOrder` 1–9 in `Initial_Migration_Seed.sql`): `tenant.FgsTenantCompany` → each schema’s `FgsTenantCompanyCache` via `TenantDataSeedingEngine`. Global credentials use `glo.GloCredential` (no tenant/company scope); platform tenant seed scripts are not required.

| Schema | Cache table(s) | Column naming |
|--------|----------------|---------------|
| `setup` | `FgsTenantCompanyCache`, `GloCredentialProviderTypeCache`, `GloResolutionTypeCache` | `Code` / `Name` on tenant company cache |
| `identity`, `billing`, `crm`, `dispatch`, `inventory`, `notification`, `reporting`, `integration` | `FgsTenantCompanyCache` | `CompanyCode` / `CompanyName` |

Setup `FgsCredential` / `FgsResolutionCode` FKs point at glo cache tables (not `glo.*` directly). Setup tenant-company-scoped entities FK to `setup.FgsTenantCompanyCache`.

## Removed cross-service foreign keys

Cross-service FKs were replaced with cache tables, indexed scalar columns, or API/event validation:

| Dependent | Former FK target | Replacement |
|-----------|-------------------|-------------|
| `identity.*` | `tenant.FgsTenantCompany` | `identity.FgsTenantCompanyCache` + validation |
| `tenant.FgsTenant` | `glo.GloSetupTenantStatus` | `FgsTenantStatusId` scalar; SetupService owns lookup |
| `setup.*` tenant rows | `tenant.FgsTenantCompany` | `setup.FgsTenantCompanyCache` FK |
| `setup.FgsCredential` | `glo.GloCredentialProviderType` | `setup.GloCredentialProviderTypeCache` FK |
| `setup.FgsResolutionCode` | `glo.GloResolutionType` | `setup.GloResolutionTypeCache` FK |
| `setup.FgsSetupGLBreak` | `tenant.FgsLocation` / `file.FgsFile` | `AddressId` Guid; no cross-schema FK |
| `setup.FgsWarehouse` | `tenant.FgsLocation` | `AddressId` Guid (renamed from `LocationId`) |
| `setup`/`glo` tag tables | `file.FgsFile` (icon) | `IconFileId` scalar; resolve via FileService |
| `audit.FgsCredentialAudit` | `setup.FgsCredential` | `CredentialId` long scalar |

**Preserved:** `setup`↔`glo` data coupling via cache seed (`Glo_Cache_Tables_Seed.sql`) and in-process glo tables on the same DbContext.

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
| Global + glo reference data + seed mappings | SetupService | `Fgs.Setup.Infrastructure/Database/Seeds/Initial_Migration_Seed.sql` |
| Glo cache tables (provider + resolution types) | SetupService | `Fgs.Setup.Infrastructure/Database/Seeds/Glo_Cache_Tables_Seed.sql` |
| Glo seed rollback | SetupService | `Fgs.Setup.Infrastructure/Database/Seeds/Initial_Migration_Seed_Down.sql` |
| Permission catalog (`identity.FgsPermission`) | UserService | `Fgs.User.Infrastructure/Database/Seeds/FgsPermission_Seed.sql` |
| Inventory reference seed (item types + default location) | InventoryService | `Fgs.Inventory.Infrastructure/Database/Seeds/Initial_Inventory_Reference_Seed.sql` |
| Inventory reference seed rollback | InventoryService | `Fgs.Inventory.Infrastructure/Database/Seeds/Initial_Inventory_Reference_Seed_Down.sql` |

**Run order (greenfield):** all service `dotnet ef database update` → `Initial_Migration_Seed.sql` → `Glo_Cache_Tables_Seed.sql` → `Initial_Inventory_Reference_Seed.sql`. `FgsTenantCompanyCache` rows are filled on first tenant provision (`SeedOrder` 1–11), before glo→tenant catalog copy (`SeedOrder` 100+). Inventory categories seed to `inventory.FgsInventoryCategory` via `GLO_INVENTORY_CATEGORY_TO_FGS_INVENTORY_CATEGORY`; subcategories via dedicated engine logic targeting `inventory` schema.

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
  → TenantDataSeedingEngine: FgsTenantCompanyCache all schemas (SeedOrder 1-9)
  → TenantDataSeedingEngine: glo/setup catalog (SeedOrder 100+)
  → IUserTenantClient.GetCompaniesAsync (file bucket company list)
  → IFileTenantClient.ProvisionBucket(tenantId)
  → IUserTenantClient.UpdateStatus(Active) + TenantProvisionCompletedEvent
```

## Deprecated

- **`shared` schema** — all tables relocated; `__EFMigrationsHistory` is per-service.
- **Monolithic UserService migrations** — 19 legacy migrations removed; replaced by service baselines above.

## Future extraction roadmap

When domain microservices mature, move tables currently interim-stored in `setup` to their owning schemas with a second migration wave:

1. ~~Inventory tables → `inventory` schema (InventoryService)~~ **Done**
2. Billing tables → `billing` schema (BillingService)
3. Dispatch tables → `dispatch` schema (SchedulingService)
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
