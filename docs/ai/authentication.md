# Authentication

- Provider: **Microsoft Entra External ID** (CIAM) JWT Bearer
- Registration: `AddFgsEntraAuthentication` / `AddFgsApiSecurity` / User `AddFgsUserFacingSecurity`
- Settings from credential distribution (`ENTRA_EXTERNAL_ID` → `EntraExternalId:*`), not committed secrets
- Claims used: Entra `oid` (object id); `sub` as user id in context; `tenant_id`/`company_id` claims are secondary
- Tenant scope for APIs: headers + active-user profile validation
- S2S: `X-FGS-Internal-Service-Key` (skips active-user profile path)
- No local/platform JWT issuer in code

## Key User endpoints (anonymous)

| Route | Purpose |
|-------|---------|
| `POST /api/v1/signup/company` | Identity signup |
| `GET /api/v1/invite/start` | Invite → Entra |
| `POST /api/v1/auth/login`, `.../entra/token`, `.../refresh` | Auth flows |
| `POST /api/v1/auth/entra/connector` | Entra API Connector |
| `GET /api/v1/internal/users/auth-profile` | S2S profile |

BFF: `POST /api/v1/bff/signup/company` (orchestrated, idempotent).

See also `docs/entra-api-connector-setup.md`.
