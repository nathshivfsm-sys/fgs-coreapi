---
name: create-query
description: Add a MediatR query, handler, and read repository for list/get/lookup in an FGS microservice. Use when adding reads, paging, or lookups.
---

# Create query

Clone Setup `ListTechTrades` / `GetTechTradeById` / `LookupTechTrades`.

## Steps

1. `Features/{Area}/Queries/{Name}/` with `IRequest<ApiResponse<T>>`.
2. List uses `PagedResult<T>` + list query record (`SetupListQuery`, `IdentityListQuery`, or service equivalent): `page`, `pageSize`, `sortBy`, `sortDirection`, `search`, `bool? isActive`.
3. Filter `IsActive` only when the nullable flag has a value (Setup convention). Lookup `activeOnly` defaults true.
4. Implementation:
   - Setup: Dapper `I{Entity}ReadRepository` + `ISetupReadConnectionFactory`, whitelist sort columns, `LIMIT/OFFSET`
   - Others: match that service (EF repository vs Dapper)
5. Always filter `TenantId`/`CompanyId` from `ITenantContextAccessor` for tenant tables.
6. Tests: handler + any non-trivial lookup filters.

## Verify

- [ ] No unbounded queries
- [ ] Tenant filters present
- [ ] Unit tests passing
