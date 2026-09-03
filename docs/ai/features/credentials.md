# Credentials

- **Owner:** SetupService (`glo`/`setup` credentials)
- **Purpose:** Encrypted platform/tenant secrets; redistribute to services
- **Entities:** `GloCredential*`, `FgsCredential`
- **Distribution:** Redis `fgs:credentials:snapshot` / channel `fgs:credentials:changed`
- **Optional vault:** AWS Secrets Manager when configured
- **Consumers:** `LoadFgsRemoteCredentialsAsync` on every API/worker
- **Events:** credential audit → Consumer → AuditService
- **Never** commit secret values
