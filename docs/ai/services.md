# Services

| Service | Schemas | Maturity | Notes |
|---------|---------|----------|-------|
| User | `identity`, `tenant` | Mature | Auth, users, roles, tenants, companies, invites |
| Setup | `setup`, `glo` | Mature | Catalogs, credentials, provisioning; many `ControllerBase` |
| File | `file` | Mature | Attachments / tenant storage |
| Audit | `audit` | Thin API | Credential audit + events |
| Notification | `notification` | Real | Dispatch; auth pipeline off |
| Inventory | `inventory` | Mature | Items, vendors, PO, stock |
| Asset | `asset` | Mature | Assets + attributes |
| Billing | `billing` | Partial | Invoice API; richer domain |
| Crm | `crm` | Partial | Customer API; leads/estimates mostly domain-only |
| Scheduling | `dispatch` | Partial | Appointment API; work orders domain-heavy |
| ServiceAgreement | `svc` | Partial | ServiceAgreement API |
| Reporting | `reporting` | Scaffold | Health + cache stub |
| Integration | `integration` | Scaffold | Health + payload entity |
| Communication | — | Scaffold | Empty domain |
| Consumer | — | Worker | Event handlers via Refit |
| Bff | — | Orchestrator | Signup + GraphQL stub; no domain DB |
| Gateway | — | NGINX | Public edge |

## Refit clients (`Fgs.Contracts.Clients`)

`IUserSignupClient`, `IUserTenantClient`, `IUserCompanyClient`, `IUserAuthProfileClient`, `IFileTenantClient`, `ISetupClient`, `INotificationDispatchClient`, `IAuditClient`, `IEntraOAuthClient`.
