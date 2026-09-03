---
name: create-api
description: Add or extend an FGS REST controller with versioned routes, ApiResponse, permissions, and NGINX. Use when adding endpoints, controllers, or public API routes.
---

# Create API

Read [docs/ai/api-conventions.md](../../../docs/ai/api-conventions.md). Clone a neighbor in the **same service**.

## Steps

1. Identify owning service (`docs/ai/services.md`). Do not add CRUD to BFF (BFF orchestrates cross-domain only).
2. Implement command/query first (skills `create-command` / `create-query`).
3. Controller:
   - `[ApiVersion(FgsApiVersions.V1)]` `[FgsVersionedRoute("singularsegment")]`
   - Prefer `FgsApiControllerBase` + `FromApiResponse` for **new** files
   - If editing Setup/Asset/Inventory existing controllers, keep `StatusCode(response.StatusCode, response)`
4. List/lookup/create/update/patch as neighbors. Mutations: `[RequirePermission]`. Retry-safe POST: `[Idempotent]`.
5. Add NGINX location in `src/Gateway/conf.d/includes/api-v1-routes.conf` (and `.prod.conf` if the prod file lists that service).
6. Tests for handlers/validators. Optional controller tests only if the service already has them.

## Verify

- [ ] Route `/api/v1/{segment}` matches gateway
- [ ] Returns `ApiResponse<T>`
- [ ] Tenant headers still required for tenant data
- [ ] `dotnet test` on the service Tests project
