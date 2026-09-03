# Authorization

- Default: authenticated user (fallback policy)
- Fine-grained: `[RequirePermission(FgsPermissionCodes.*)]` → `PermissionAuthorizationFilter`
- `TENANT_ADMIN` bypasses permission checks
- Internal-key calls without a user profile are allowed by the filter (S2S)
- Named ASP.NET `AddPolicy` catalogs are not used beyond the fallback

## RBAC storage (`identity`)

`FgsUser`, `FgsRole`, `FgsPermission`, `FgsUserRole`, `FgsRolePermission`, `FgsRoleDataAccess`, `FgsDataAccess`, `FgsDataAccessScope`, menus

Seed: `UserService/.../Seeds/FgsPermission_Seed.sql`

## Company scope

- User has one `CompanyId`
- Non-admins: header company must match profile
- Tenant admins may select another company via `X-Company-Id`
