# Setup catalogs

- **Owner:** SetupService (`setup`, `glo`)
- **Purpose:** Tenant master data (trades, taxes, pricing, job types, sales, vehicles, templates, …)
- **Pattern:** Dapper read + EF write; template `.cursor/SETUP_ENTITY_CRUD_TEMPLATE.md`
- **Reference:** `FgsSetupTechTrade` → `/api/v1/techtrade`
- **AuthZ:** `SETUP.*`; provisioning often `[AllowAnonymous]` for internal flows
- **Skill:** `create-setup-entity`
- **Gateway:** keep routes in sync with controllers
