# Entra External ID API Connector Setup

Configure the `Fgs_SignUpSignIn` user flow so Entra embeds `tenant_id` and `company_id` in access tokens.

## Prerequisites

- UserService deployed and reachable at the gateway URL (e.g. `https://developer.fsm.com`).
- Company signup creates `FgsUser` records with the admin email before Entra sign-up.

## Steps (Entra portal)

1. **Register custom user attributes** (External Identities → User attributes):
   - `tenantId` (String)
   - `companyId` (String)

2. **Create API Connector** (External Identities → API connectors):
   - **Endpoint URL:** `https://<gateway-host>/api/v1/auth/entra/connector`
   - **Authentication:** None
   - **Method:** POST

3. **Attach connector to user flow** `Fgs_SignUpSignIn`:
   - Step: **Before sending the token (preview)**
   - Select the connector created above

4. **Application claims** (user flow → Application claims):
   - Map connector response fields to access token claims:
     - `tenant_id`
     - `company_id`

5. **Redirect URI**: SPA auth callback registered in Entra (e.g. `https://<ui-host>/auth/callback`). Must match `Application:UiAuthCallbackUrl`.

## Connector behavior

Entra POSTs JSON with the sign-up email (and optionally `objectId`). UserService:

- Validates email against a pre-provisioned `FgsUser` from company signup (pending or accepted invitation).
- Returns `tenant_id` and `company_id` on success.
- Returns `ShowBlockPage` for unknown emails.

## Verification

1. Complete company signup and invitation flow.
2. Decode the access token — confirm `tenant_id` and `company_id` claims.
3. Call `GET /api/v1/auth/me` with the token — profile matches the database.
