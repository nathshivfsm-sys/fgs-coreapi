---
name: debug-api
description: Diagnose FGS API 401/403/404/tenant-mismatch and local gateway issues. Use when an endpoint fails in local Docker/NGINX or Postman.
---

# Debug API

1. Hit via NGINX (`https://developer.fsm.com`) not the service port, unless isolating the service.
2. **401:** missing/invalid Entra Bearer; pipeline on by default.
3. **403:** `RequirePermission` or company mismatch (non-admin `X-Company-Id` ≠ profile company).
4. **Empty/wrong rows:** missing `X-Tenant-Id`/`X-Company-Id`; EF filters need `ITenantContext`.
5. **404 route:** gateway `api-v1-routes.conf` vs `[FgsVersionedRoute]`.
6. **S2S:** `X-FGS-Internal-Service-Key` + `[AllowAnonymous]` internal controllers.
7. Credentials: services load Redis snapshot after Setup; check `fgs:credentials:snapshot` / required providers.
8. Correlation: `X-Correlation-Id` in logs (Serilog JSON / Datadog).

Do not log tokens. Prefer existing Postman under `docs/api/local/`.
