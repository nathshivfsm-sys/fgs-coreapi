# Multi-tenancy

| Concept | Type | Meaning |
|---------|------|---------|
| `TenantId` | `long` | Account / billing boundary |
| `CompanyId` | `long` | Legal entity under tenant (`FgsTenantCompany.CompanyNumber`) |

## Runtime

1. Client sends `X-Tenant-Id` + `X-Company-Id`
2. `HeaderTenantResolver` builds context
3. With auth: ActiveUser validates Entra oid profile against that scope and sets `ITenantContextAccessor.Current`
4. EF global filters on `ITenantScoped` / `ITenantCompanyScoped` (+ optional `IsActive`)

Platform operations use tenant/company `0`.

## Doc vs code

Older `.cursor/rules.md` describes UUID `tenant_id` and multi-company user membership. **Current code uses `long` IDs and single-company users.** Prefer this document.
