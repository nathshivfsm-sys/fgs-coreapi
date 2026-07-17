# Clean Architecture Scorecard

**Date:** 2026-07-16  
**Scope:** All microservices under `src/`, plus Shared and Gateway.  
**Normative docs:** [`.cursor/rules.md`](../../.cursor/rules.md), [`MICROSERVICE_TEMPLATE.md`](MICROSERVICE_TEMPLATE.md), [`SHARED_ARCHITECTURE_REVIEW.md`](SHARED_ARCHITECTURE_REVIEW.md), [`.cursor/SETUP_ENTITY_CRUD_TEMPLATE.md`](../../.cursor/SETUP_ENTITY_CRUD_TEMPLATE.md).

**Scoring:** Pass / Partial / Fail / N/A  
**Overall rule:** Pass = structure + deps sound with only soft debt; Partial = scaffold or mixed compliance; Fail = systemic controller or cross-service DB breach.

**Repo-wide soft debt (not Fail alone):** anemic Domain entities; outbox writes from Application handlers (`IOutboxWriter`); Application projects often reference `Fgs.Persistence` (EF transitively, but no `DbContext` usage found in Application code).

**DI note:** Documented order is Application → Infrastructure → MultiTenancy → Observability. Mature hosts call `AddFgsApiHost` first (registers MultiTenancy early when enabled), then Application → Infrastructure → Observability. `AddFgsFoundation` correctly enters via Application only.

---

## Checklist legend

| # | Criterion |
|---|-----------|
| 1 | Layer projects present (API / Application / Domain / Infrastructure + Tests) |
| 2 | Dependency direction (Domain → Kernel; Application has no DbContext; API thin) |
| 3 | DI order (Foundation via Application; MultiTenancy timing) |
| 4 | CQRS layout (Features; `ApiResponse<T>` where HTTP) |
| 5 | No cross-service DB |
| 6 | S2S HTTP via Refit / Contracts |
| 7 | Controllers: `FgsApiControllerBase` + `FromApiResponse` |
| 8 | Maturity (informational) |

---

## Per-service scorecard

| Service | Overall | 1 | 2 | 3 | 4 | 5 | 6 | 7 | Maturity | Evidence |
|---------|---------|---|---|---|---|---|---|---|----------|----------|
| **User** | Partial | Pass | Partial | Partial | Pass | Pass | Pass | Partial | Full CRUD (auth, RBAC, signup, webhooks) | Layers + Tests. Domain→Kernel; App refs Persistence but no DbContext usage. Own `identity`/`tenant`. Refit Entra + credential clients. Most controllers `FgsApiControllerBase`; Signup/Invite/Dashboard use `ControllerBase`. |
| **Setup** | Fail | Pass | Partial | Partial | Pass | Fail | Pass | Fail | Full CRUD (catalogs + provisioning) | Strong Features/`ApiResponse`. **Seeds `inventory` schema** (cross-service write). ~40 controllers use `ControllerBase` + `StatusCode` instead of `FromApiResponse`. Refit to User/File OK. |
| **File** | Pass | Pass | Partial | Partial | Pass | Pass | Pass | Pass | Focused CRUD (attachments/storage) | Own `file` schema. Controllers `FgsApiControllerBase` + `FromApiResponse`. Soft: Persistence on App; MT early via host. |
| **Audit** | Pass | Pass | Partial | Partial | Pass | Pass | N/A | Pass | Thin (credential audit + health) | Own `audit` schema. Controllers compliant. Soft: Persistence on App; MT early. |
| **Notification** | Partial | Pass | Partial | Partial | Partial | Pass | N/A | Pass | Dispatch/providers | Own `notification`. Features + parallel app folders; Infra also registers MediatR. Controllers OK. |
| **Inventory** | Partial | Pass | Partial | Partial | Pass | Pass | N/A | Partial | Partial CRUD (Vendor, InventoryLocation) | Own `inventory`. Features/`ApiResponse`. Vendor/Location controllers use plain `ControllerBase`. |
| **Billing** | Partial | Pass | Pass | Pass | Partial | Pass | N/A | Pass | Scaffold | Health-only API; Domain + DbContext schema present; App DI-only. |
| **Crm** | Partial | Pass | Pass | Pass | Partial | Pass | N/A | Pass | Scaffold | Same pattern as Billing (`crm` schema). |
| **Scheduling** | Partial | Pass | Pass | Pass | Partial | Pass | N/A | Pass | Scaffold | Health-only; `dispatch` schema. |
| **Reporting** | Partial | Pass | Pass | Pass | Partial | Pass | N/A | Pass | Scaffold | Health-only; `reporting` schema. |
| **Integration** | Partial | Pass | Pass | Pass | Partial | Pass | N/A | Pass | Scaffold | Health-only; `integration` schema. |
| **Asset** | Partial | Pass | Partial | Partial | Pass | Pass | N/A | Fail | Full-ish CRUD (multiple feature areas) | Own `asset`. Features/`ApiResponse` good. Most controllers `ControllerBase` + `StatusCode` (Health uses `FgsApi`). |
| **ServiceAgreement** | Partial | Pass | Pass | Pass | Partial | Pass | N/A | Pass | Scaffold | Health-only; `svc` schema. |
| **Communication** | Partial | Partial | Partial | Pass | Partial | N/A | N/A | Pass | Placeholder | Domain empty / no Kernel ref; no Features/DbContext; Infra = credentials; Health OK. |
| **Consumer** | Partial | Partial | Partial | Pass | Partial | N/A | Pass | Pass | Worker | Domain empty. Message Features return `Task` (OK for messaging). Refit Notification/Audit via Contracts. |
| **Publisher** | Partial | Partial | Partial | Pass | Partial | Fail | N/A | Pass | Outbox publisher | Domain empty. **Reads other services’ outbox schemas/DBs**. Health only. |
| **Shared** | N/A | N/A | N/A | N/A | N/A | N/A | N/A | N/A | Platform libs | Kernel, Foundation, Persistence, Contracts, Messaging, MultiTenancy, Observability, Security, Credentials. |
| **Gateway** | N/A | N/A | N/A | N/A | N/A | N/A | N/A | N/A | nginx edge | No CA layer projects. |

---

## Maturity buckets

| Bucket | Services |
|--------|----------|
| Mature CA shape | User, Setup*, File, Asset*, Inventory* (*controller and/or cross-DB debt) |
| Thin but structured | Audit, Notification, Consumer |
| Scaffold / placeholder | Billing, Crm, Scheduling, Reporting, Integration, ServiceAgreement, Communication, Publisher |
| N/A for CA layers | Shared libraries, Gateway |

---

## Top violations (follow-ups; not fixed in this pass)

1. **Setup controllers** — migrate ~40 `ControllerBase` + `StatusCode(response…)` → `FgsApiControllerBase` + `FromApiResponse` / `CreatedFromApiResponse`.
2. **Asset (+ Inventory Vendor/Location) controllers** — same base-class / `FromApiResponse` migration.
3. **Setup cross-schema seeding** — stop Setup writing `inventory.*`; move tenant inventory seed into InventoryService (API/event/outbox).
4. **Publisher cross-DB outbox** — document as intentional exception or isolate via per-service outbox APIs / shared contract without foreign schema SQL.
5. **Application → `Fgs.Persistence`** — split abstractions (no EF package) from EF implementations so Application cannot transitively take EF Core.
6. **DI order docs** — register MultiTenancy after Infrastructure, or document host-early MultiTenancy as the standard.
7. **Empty Domains** — add Kernel ref or delete unused Domain projects for Communication/Consumer/Publisher.
8. **Notification Infra MediatR** — remove duplicate `AddMediatR` from Infrastructure; keep handlers in Application Features only.

---

## Cleanup performed with this scorecard

High-confidence dead code removed (no runtime path impact):

- UserService: Compile-removed `Security/**` and `Storage/**` duplicates; orphan `IS3ObjectKeyBuilder`; unused `IFgsUserRoleResolver` / `FgsUserRoleResolver` + DI.
- NotificationService: unused `INotificationPreferenceService` + `PlaceholderNotificationPreferenceService` + DI.
- Prior pass: legacy `EntraExternalIdService` (HttpClient) removed in favor of `EntraExternalIdRefitService`.
