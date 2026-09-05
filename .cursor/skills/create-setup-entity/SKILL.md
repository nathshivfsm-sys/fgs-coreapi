---
name: create-setup-entity
description: Add a full Setup Service catalog entity (FgsSetup*) with CQRS CRUD, Dapper reads, EF writes, controller, tests, and gateway route. Use when adding setup/master data tables or cloning TechTrade-style modules.
---

# Create Setup entity

Canonical template: `.cursor/SETUP_ENTITY_CRUD_TEMPLATE.md`. Reference code: `FgsSetupTechTrade` (`/api/v1/techtrade`). Optional generator: `src/SetupService/scripts/generate_setup_crud.py`.

## Steps

1. Read the template end-to-end. Do not invent a different folder layout.
2. Entity in Domain (`setup` schema, `ITenantCompanyScoped`, `IsActive`, audit columns).
3. EF configuration + `EntitySchemaRegistry` + migration (`database-change`).
4. Abstractions, Dapper SQL, write service, Features (DTOs, commands, queries, validators), controller, tests — same names as TechTrade.
5. Default: **no HTTP DELETE**; soft delete via PATCH `isActive`. Add DELETE only for modules listed in the template.
6. `[RequirePermission(SetupCreate/SetupEdit)]`. Route singular lowercase.
7. Gateway route + `Compare-ApiRoutes.ps1` if used.

## Verify

- [ ] Matches TechTrade layering (Dapper read / EF write)
- [ ] Tenant+company on every SQL/EF path
- [ ] Tests: validator, command, query
- [ ] Gateway route added
