# Credentials

- **Owner:** SetupService (`glo`/`setup` credentials)
- **Purpose:** Encrypted platform/tenant secrets; redistribute to services
- **Entities:** `GloCredential*`, `FgsCredential`
- **Distribution (primary):** Redis `fgs:credentials:snapshot` / channel `fgs:credentials:changed` (Global keys only)
  - Overlapping pub/sub signals coalesce (pending reload)
  - Periodic re-fetch via `CredentialConsumer:SnapshotRefreshIntervalSeconds` (default 300)
- **Distribution (backup for ConsumerService):** outbox → RabbitMQ `setup.CredentialConfigurationChanged` → Consumer reloads via `ICredentialConfigurationProvider`
- **S2S:** `CREDENTIAL_DISTRIBUTION_KEY` / `CredentialDistribution:InternalServiceKey`
  - Develop currently uses shared `fgs-internal-credential-distribution-key` (compose/appsettings default) — rotate before production
- **Resolved API:** filters by `CredentialServiceProviderAllowList` + requesting service name; excludes Tenant keys
- **Optional vault:** AWS Secrets Manager registered but unused for mutate/load (Database + KMS is SoT)
- **Consumers:** `LoadFgsRemoteCredentialsAsync` on every API/worker
- **Events:** credential audit → Consumer → AuditService; configuration-changed → Consumer reload
- **Never** commit production secret values
