# Configuration

Document **section names only**. Never paste secrets.

## Common

`CredentialDistribution`, `CredentialConsumer`, `Datadog`, `Serilog`, `Logging`, `AllowedHosts`, `Swagger`, `ConnectionStrings`, `Resilience`, `TenantScope`, `Redis`, `RabbitMq`, `Outbox`, `AwsCredentials`, `AwsBootstrap`


## Auth (runtime)

`EntraExternalId` — loaded from credential snapshot (`Global:ENTRA_EXTERNAL_ID:*`)

## Service URLs

`UserService:BaseUrl`, `SetupService:BaseUrl`, `FileService:BaseUrl`, `NotificationService:BaseUrl`, `AuditService:BaseUrl`, …

## Feature-specific

`Signup`, `Invitation`, `Application`, `AuditOutbox`, `AttachmentValidation`, `FileService` (file options), `Consumer`
