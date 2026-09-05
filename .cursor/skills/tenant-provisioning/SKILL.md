---
name: tenant-provisioning
description: >-
  Wire Glo* catalog tables into tenant onboarding via GloSeedTableMapping /
  GloSeedTableColumnMapping and TenantDataSeedingEngine. Use when adding or
  changing tenant provisioning seeds, Glo→Fgs catalog copies, SeedOrder,
  JOINED_PARENT soft-paths, or menu/role/catalog provisioning.
---

# Tenant provisioning seeding

Read [docs/ai/features/tenants-companies.md](../../../docs/ai/features/tenants-companies.md)
and clone a neighbor mapping in
[`Initial_Migration_Seed.sql`](../../../src/SetupService/Fgs.Setup.Infrastructure/Database/Seeds/Initial_Migration_Seed.sql)
(e.g. `ALL_GloRole`, `ALL_GloMenu`).

## Flow

```text
TenantProvisionRequested (outbox)
  → Consumer → Setup POST /api/v1/tenantprovisioning
  → TenantProvisioningOrchestrator
  → TenantDataSeedingEngine (active GloSeedTableMapping by SeedOrder)
  → FileService bucket → tenant Active
```

## Steps (new Glo* → Fgs* pair)

1. **Own the global catalog** in Setup (`glo.Glo*`) — entity, EF config, migration, idempotent SQL seed in `Database/Seeds`.
2. **Own the tenant copy** in the target service (`Fgs*`, correct schema) — entity, migration, APIs as needed. Cross-service IDs are **scalars**, never EF FKs across schemas/services.
3. **Add seed mapping** in `Initial_Migration_Seed.sql`:
   - `glo.GloSeedTableMapping` row (`SeedCode`, source/target db/schema/table, `SeedOrder`, `IsActive`)
   - `glo.GloSeedTableColumnMapping` rows (TENANT_ID / COMPANY_ID / CURRENT_TIMESTAMP / SEED_CREATED_BY / STATIC / direct column copies)
4. **Pick SeedOrder** after dependencies (e.g. identity cache → roles `15` → menus `16` → catalogs `100+`).
5. **Flat vs remap:**
   - Flat `INSERT…SELECT` via `TenantSeedSqlBuilder` when source columns map 1:1 (or global Id kept as scalar, e.g. `GloMenu.Id` → `FgsTenantMenu.MenuId`).
   - When a target FK must resolve to a **tenant-local** row (e.g. `GloRole.Id` → `FgsRole.Id` by `RoleCode`), do **not** invent a flat mapping — use an existing JOINED_PARENT soft-path pattern in `TenantDataSeedingEngine` / `TenantJoinedChildSeedHelper`, or add a dedicated soft-path.
6. Keep inserts **idempotent** (`WHERE NOT EXISTS` on `SeedCode` / target column name).
7. If public HTTP was added for the Fgs table, update Gateway `api-v1-routes.conf` (+ prod) per `create-api`.

## Verify

- [ ] Mapping + column mappings present and column names match both tables
- [ ] SeedOrder after required parents
- [ ] No cross-schema EF FKs
- [ ] Owning-service migration + seed SQL build/tests green
- [ ] Gateway routes updated when new public endpoints exist
