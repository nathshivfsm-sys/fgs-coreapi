---
name: database-change
description: Change FGS PostgreSQL schema via EF Core in the owning service only, including migrations. Use when adding tables, columns, indexes, or running add-migration.
---

# Database change

Read [docs/ai/database.md](../../../docs/ai/database.md). Own schema only.

## Steps

1. Put entities in **owning** `{Service}.Domain`. Map in `{Service}.Infrastructure` configurations + schema registry.
2. Inherit `FgsEntityBase` / `GloEntityBase`; tenant tables implement `ITenantCompanyScoped` or `ITenantScoped`.
3. Add migration from the Infrastructure project using that service’s `*DbContextDesignFactory` (search `DesignFactory`).
4. Example (Setup): run `dotnet ef migrations add {Name} --project src/SetupService/Fgs.Setup.Infrastructure --startup-project src/SetupService/Fgs.Setup.API`.
5. Review generated SQL: schema name, no FKs to other services’ schemas.
6. Cross-service IDs are scalars, not EF FKs.
7. Tests for handlers that depend on new fields.

## Verify

- [ ] Migration in the correct service
- [ ] Schema matches `FgsDatabaseSchemas`
- [ ] No secrets in snapshot files
- [ ] Build succeeds
