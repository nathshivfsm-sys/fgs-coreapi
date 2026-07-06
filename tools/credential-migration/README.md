# Credential Migration Tooling

Migrate secrets from legacy `appsettings.json` values into Setup Service `GloCredential` records.

## Prerequisites

1. Setup Service running with **bootstrap** KMS configured (`AwsCredentials:KmsKeyArn` or `KMS_KEY_ARN`, plus `AccessKeyId`/`SecretAccessKey` or `AWS_ACCESS_KEY_ID`/`AWS_SECRET_ACCESS_KEY` for local dev). Setup cannot use the AWS credential stored in `GloCredential` to decrypt credentials — that would be circular.
2. Provider types seeded (`Initial_Migration_Seed.sql` or `seed-provider-types.sql`).
3. `CredentialDistribution:InternalServiceKey` configured on Setup and consuming services.

## Scripts

| Script | Purpose |
|--------|---------|
| [`seed-provider-types.sql`](seed-provider-types.sql) | Idempotent `GloCredentialProviderType` INSERT/UPDATE + cache sync |
| [`post-credentials.ps1`](post-credentials.ps1) | POST all global credentials; `-UpdateDatabaseOnly` PUTs RDS strings; `-UpdateAwsOnly` PUTs AWS keys + `KmsKeyArn` for consumers; `-UpdateRedisOnly` PUTs shared Redis cache settings |

```powershell
# Seed provider types (requires psql or Docker)
docker run --rm -e PGPASSWORD -v "${PWD}/seed-provider-types.sql:/seed.sql:ro" postgres:16-alpine `
  psql -h <host> -p 5432 -U <user> -d <database> -f /seed.sql

# Post credentials (Setup API on http://localhost:5071)
.\post-credentials.ps1 -BaseUrl http://localhost:5071

# Update only DATABASE connections (all services -> RDS)
.\post-credentials.ps1 -BaseUrl http://localhost:5071 -UpdateDatabaseOnly

# Update AWS credential (keys + KmsKeyArn for File/User consumers)
.\post-credentials.ps1 -BaseUrl http://localhost:5071 -UpdateAwsOnly

# Update shared Redis cache settings (all services)
.\post-credentials.ps1 -BaseUrl http://localhost:5071 -UpdateRedisOnly

# Create only the REDIS credential (after seed-provider-types.sql)
.\post-credentials.ps1 -BaseUrl http://localhost:5071 -RedisOnly
```

All platform `DATABASE` keys use AWS RDS `fgs_dev_db`, including `FgsAsset`, `FgsAssetReadOnly`, `FgsDispatch` (Scheduling), and `FgsServiceAgreement`.

If `platform-databases` was created before these keys were added, refresh the credential without re-posting everything:

```powershell
.\post-credentials.ps1 -BaseUrl http://localhost:5071 -UpdateDatabaseOnly
```

## Example payloads

### Database (platform-databases bundled credential)

All service connection strings live in one global `DATABASE` credential (`platform-databases`). Include readonly keys alongside write keys:

```http
POST /api/v1/credentials
Content-Type: application/json

{
  "scope": "Global",
  "providerCode": "DATABASE",
  "credentialName": "platform-databases",
  "payload": "{\"FgsUser\":\"Host=localhost;Port=5432;Database=fgs_dev_db;Username=postgres;Password=postgres\",\"FgsUserReadOnly\":\"Host=localhost;Port=5432;Database=fgs_dev_db;Username=postgres;Password=postgres\",\"FgsSetup\":\"Host=localhost;Port=5432;Database=fgs_dev_db;Username=postgres;Password=postgres\",\"FgsSetupReadOnly\":\"Host=localhost;Port=5432;Database=fgs_dev_db;Username=postgres;Password=postgres\"}"
}
```

For a single named connection, either use `ConnectionString` + `ConnectionStringName`, or map each key directly (e.g. `FgsUser`, `FgsUserReadOnly`).

### SendGrid

```json
{
  "scope": "Global",
  "providerCode": "SENDGRID",
  "credentialName": "platform-sendgrid",
  "payload": "{\"ApiKey\":\"<from-appsettings>\",\"FromAddress\":\"noreply@example.com\",\"FromName\":\"FGS\"}"
}
```

### RabbitMQ

```json
{
  "scope": "Global",
  "providerCode": "RABBITMQ",
  "credentialName": "platform-rabbitmq",
  "payload": "{\"Username\":\"fgs\",\"Password\":\"<from-appsettings>\"}"
}
```

### Redis (shared cache for all services)

```json
{
  "scope": "Global",
  "providerCode": "REDIS",
  "credentialName": "platform-redis",
  "payload": "{\"Enabled\":true,\"ConnectionString\":\"redis:6379\",\"InstanceName\":\"fgs:\",\"DefaultAbsoluteExpirationMinutes\":30}"
}
```

Maps to the `Redis` configuration section (`Redis:ConnectionString`, `Redis:Enabled`, etc.) consumed by services that call `AddFgsRedisCache`.

### Entra client secret

```json
{
  "scope": "Global",
  "providerCode": "ENTRA_EXTERNAL_ID",
  "credentialName": "platform-entra",
  "payload": "{\"ClientSecret\":\"<from-appsettings>\"}"
}
```

### AWS keys

```json
{
  "scope": "Global",
  "providerCode": "AWS",
  "credentialName": "platform-aws",
  "payload": "{\"AccessKeyId\":\"<key>\",\"SecretAccessKey\":\"<secret>\",\"KmsKeyArn\":\"<kms-key-arn>\"}"
}
```

## Environment variables (bootstrap fallback)

| Variable | Purpose |
|----------|---------|
| `FGS_SETUP_DB` | Setup Service database bootstrap |
| `FGS_USER_DB` | User Service DB fallback during migration |
| `FGS_FILE_DB` | File Service DB fallback |
| `FGS_NOTIFICATION_DB` | Notification Service DB fallback |
| `FGS_ASSET_DB` | Asset Service DB fallback |
| `FGS_ASSET_DB_READONLY` | Asset Service read-only DB fallback |
| `FGS_DISPATCH_DB` | Scheduling Service DB fallback |
| `FGS_SVC_DB` | Service Agreement Service DB fallback |
| `KMS_KEY_ARN` | Setup Service KMS bootstrap only (consumers read `KmsKeyArn` from Setup AWS credential) |
| `CREDENTIAL_DISTRIBUTION_KEY` | S2S key for `/credentials/resolved` |

After migration, remove secrets from committed appsettings and rely on Setup credential storage plus env overrides for local bootstrap only.
