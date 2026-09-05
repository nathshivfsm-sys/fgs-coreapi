---
name: multi-tenancy
description: Apply FGS tenant/company headers, EF filters, and context accessors. Use when adding tenant-scoped entities, queries, or debugging wrong-company data.
---

# Multi-tenancy

Read [docs/ai/multi-tenancy.md](../../../docs/ai/multi-tenancy.md).

## Steps

1. Tenant tables: `TenantId` + `CompanyId` (`long`), `ITenantCompanyScoped`.
2. DbContext must inherit `FgsTenantFilteredDbContext`.
3. Dapper/SQL: filter both IDs from `ITenantContextAccessor.Current` (do not trust body tenant ids).
4. Host: `UseMultiTenancy = true` on APIs that serve tenant data.
5. Skip-path prefixes only for health/swagger/auth clones (`TenantScope:SkipPathPrefixes`).
6. Do not resolve tenant from JWT as the primary mechanism.

## Verify

- [ ] Filters on EF and Dapper
- [ ] Headers documented for the API
- [ ] No cross-company leak in list queries
