# Auth (Entra)

- **Owner:** UserService (+ BFF signup orchestration)
- **Purpose:** Login, invite acceptance, token refresh, Entra API Connector
- **APIs:** `/api/v1/auth/*`, `/api/v1/invite/start`, BFF `/api/v1/bff/signup/company`
- **Deps:** Entra OAuth Refit client; credentials from Setup snapshot
- **See:** [../authentication.md](../authentication.md)
- **Change often:** `Features/Auth`, `InviteController`, `SignupController`
