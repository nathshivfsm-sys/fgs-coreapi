# Database

PostgreSQL. Dev may share one instance with **schema-per-service**. Each service has its own connection string name for later DB split.

| Service | Schema(s) | Conn key (typical) |
|---------|-----------|--------------------|
| User | `identity`, `tenant` | `FgsUser` |
| Setup | `setup`, `glo` | `FgsSetup` (+ `FgsSetupReadOnly`) |
| File | `file` | `FgsFile` |
| Audit | `audit` | `FgsAudit` |
| Notification | `notification` | `FgsNotification` |
| Inventory | `inventory` | `FgsInventory` |
| Asset | `asset` | `FgsAsset` |
| Billing | `billing` | `FgsBilling` |
| Crm | `crm` | `FgsCrm` |
| Scheduling | `dispatch` | `FgsDispatch` |
| ServiceAgreement | `svc` | `FgsServiceAgreement` |
| Reporting | `reporting` | `FgsReporting` |
| Integration | `integration` | `FgsIntegration` |

## Naming

- Global: `Glo*` — no `TenantId`
- Tenant: `Fgs*` / `FgsSetup*` — `TenantId` + `CompanyId` (`long`)
- Audit columns: `CreatedOn`, `CreatedBy`, `UpdatedOn`, `UpdatedBy`
- Soft delete: `IsActive`

## Patterns

- Contexts inherit `FgsTenantFilteredDbContext`
- `AddFgsPersistence<TDbContext>()`, `UseFgsNpgsql`
- Setup reads: Dapper; writes: EF + `IUnitOfWork`
- Authoritative maps: `*/Infrastructure/Database/Schemas/EntitySchemaRegistry.cs`
- Human detail: `docs/architecture/DATABASE_OWNERSHIP_MIGRATION.md`

## Outbox tables

`tenant.TenantOutboxMessage`, `glo.GloOutboxMessage`, `setup.SetupOutboxMessage`, `inventory.InventoryOutboxMessage`, `crm.CrmOutboxMessage`.
