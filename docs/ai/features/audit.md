# Audit

- **Owner:** AuditService (`audit`)
- **Purpose:** Credential audit trail and event archive APIs
- **Entities:** `FgsCredentialAudit`, `FgsEvent*`, `FgsArchiveCatalog`
- **APIs:** `/api/v1/credentialaudit`, `/api/v1/event`, `/api/v1/archive` (often AllowAnonymous + internal key)
- **Ingest:** Consumer handles `audit.credential.requested` via `IAuditClient`
