# Tenants & companies

- **Owner:** UserService (`tenant`)
- **Purpose:** Tenant account, companies, locations, menus, service setup
- **Entities:** `FgsTenant`, `FgsTenantCompany`, `FgsLocation`, `TenantOutboxMessage`
- **APIs:** `/api/v1/tenant`, `/api/v1/company`, related setup endpoints
- **Rules:** `CompanyId` = company number under tenant; users bind to one company
- **Events:** `tenant.provision.requested` (Consumer → Setup)
- **Clone:** Tenant/Company controllers + provision outbox path
