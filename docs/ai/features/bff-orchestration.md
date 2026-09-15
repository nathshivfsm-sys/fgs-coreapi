# BFF orchestration

- **Owner:** BffService
- **Purpose:** Cross-domain workflows (signup), GraphQL read aggregation at `/api/v1/bff/graphql`
- **No domain DB**
- **Refit / HttpClient:** User signup/tenant, Setup (S2S), plus named lookup clients for Setup/User/Asset/Inventory/Billing/CRM
- **Rule:** do not move owning-service CRUD into BFF
- **Clone:** `SignupController` + Application signup feature; `LookupsController` + GraphQL `lookups` for catalog batching

## Batch lookups

- **GraphQL:** `lookups(requests: [LookupRequestInput!]!): [LookupResult!]!` — one bag field (not selection-set per entity). Partial failures set `error` on that key only; others still return `items`.
- **REST discovery (Swagger):** `GET /api/v1/bff/lookups/keys` — same catalog metadata (`key`, `service`, `path`, filters).
- **Catalog:** `LookupCatalog` / `LookupKey` in Application — covers all existing owning-service `GET .../lookup` routes (Glo + Setup tenant + User + Asset + Inventory + Billing Invoice + CRM Customer), plus `PostalCodeCity` (`api/v1/postalcode/cities`).
- **Caching:** no BFF response cache; rely on owning-service Redis.
- **Fan-out:** `Task.WhenAll` via `ILookupGateway` (caller auth + tenant/company headers forwarded).
- **Auth:** GraphQL and keys endpoint require the normal authenticated pipeline; tenant headers still required for tenant keys (do not add `/api/v1/bff/graphql` or `/api/v1/bff/lookups` to `TenantScope:SkipPathPrefixes`).
