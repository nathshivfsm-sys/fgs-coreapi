# Notifications

- **Owner:** NotificationService (`notification`)
- **Purpose:** Email/SMS dispatch and history
- **Entities:** `FgsEmailHistory`, `FgsSmsHistory`, templates cache, processed events
- **APIs:** `/api/v1/notification` (anonymous dispatch; auth pipeline off)
- **Deps:** SendGrid SDK; Setup templates via `ISetupClient`
- **Ingest:** Consumer → `INotificationDispatchClient` for invite email
