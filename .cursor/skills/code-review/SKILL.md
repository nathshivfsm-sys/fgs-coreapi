---
name: code-review
description: Review FGS diffs against Clean Architecture, tenant isolation, ApiResponse, and outbox rules. Use when reviewing PRs or local changes.
---

# Code review

Read `docs/ai/architecture.md` and the feature doc for the service.

## Check

- Layering: no `DbContext` in Application/API; controllers stay thin
- Tenant: `TenantId`/`CompanyId` on data + queries; no cross-schema writes
- API: `ApiResponse<T>`, permission attributes, gateway route if public
- Events: outbox not direct RabbitMQ from API
- S2S: Refit contracts, internal service key
- Tests: handler/validator coverage
- Secrets: none in diff

## Output

- Critical / suggestion / note
- Cite files. Do not rewrite unrelated style (Setup `ControllerBase` vs `FgsApiControllerBase`) unless the change already touches that controller.
