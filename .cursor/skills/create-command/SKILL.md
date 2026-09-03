---
name: create-command
description: Add a MediatR command, validator, handler, and write-service method in an FGS microservice. Use when creating or updating data via CQRS.
---

# Create command

Clone: Setup `Features/TechTrades/Commands/CreateTechTrade`, User `InviteFgsUser`, or Asset `CreateFgsAsset`.

## Steps

1. Folder: `{Service}.Application/Features/{Area}/Commands/{Name}/`
2. `sealed record {Name}Command(...) : IRequest<ApiResponse<TDto>>`
3. Handler: call `I{Entity}WriteService` (or equivalent). No `DbContext`. Return `ApiResponse<T>.Ok(..., Created)` or Fail.
4. `AbstractValidator<{Name}Command>` when the neighbor has one; async uniqueness via read repository.
5. Infrastructure write service: set `TenantId`/`CompanyId` from `ITenantContext` / `IFgsUserContext`; stamp audit columns; `IUnitOfWork.SaveChanges`.
6. Unique violation `23505` → `InvalidOperationException` (409).
7. If other services must react: enqueue outbox in the **same** save (`implement-outbox`).
8. Tests: validator + handler with mocked write service.

## Verify

- [ ] Matches existing command naming in that area
- [ ] Tenant scope set on create
- [ ] Unit tests added and passing
