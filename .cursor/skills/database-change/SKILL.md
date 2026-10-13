---
name: database-change
description: Change FGS PostgreSQL schema via EF Core in the owning service only, including migrations. Use when adding tables, columns, indexes, or running add-migration.
---

# Database change

Read [docs/ai/database.md](../../../docs/ai/database.md). Own schema only.

## Steps

1. Put entities in **owning** `{Service}.Domain`. Map in `{Service}.Infrastructure` configurations + schema registry.
2. Inherit `FgsEntityBase` / `GloEntityBase`; tenant tables implement `ITenantCompanyScoped` or `ITenantScoped`.
3. Generate the migration and SQL scripts together (do not stop at `dotnet ef migrations add`):

```powershell
./scripts/generate-migration-sql.ps1 -ServiceName {Service} -MigrationName {Name}
```

4. Review generated up SQL: schema name, no FKs to other services’ schemas.
5. Replace the Rollback TODO stub with a real idempotent down script: reverse DDL (`IF EXISTS`), then delete that migration’s row from `{schema}."__EFMigrationsHistory"`, inside a transaction. Paths:
   - `Database/Scripts/Execute/{MigrationId}_up.sql`
   - `Database/Scripts/Rollback/{MigrationId}_down.sql`
6. Cross-service IDs are scalars, not EF FKs.
7. Tests for handlers that depend on new fields.
8. Patch-bump `<Version>` in the owning service API csproj. End with a short check-in comment. Do not commit unless asked.

## Verify

- [ ] Migration in the correct service
- [ ] Schema matches `FgsDatabaseSchemas`
- [ ] `{MigrationId}_up.sql` and `{MigrationId}_down.sql` both exist and the down script is not a TODO stub
- [ ] No secrets in snapshot files
- [ ] Owning service `<Version>` patch-bumped
- [ ] Build succeeds
