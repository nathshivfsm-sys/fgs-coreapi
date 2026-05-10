# FGS Field Service Management — Backend Rules (Cursor)

Single source of truth for architecture, tenancy, identity, and implementation expectations.

---

## 1. Project overview

Build a **multi-tenant SaaS Field Service Management (FSM)** backend for service businesses: HVAC, plumbing, electrical, garage door, trash pickup, house cleaning, painting, lawn care, and similar trades.

**Lifecycle the platform covers (target):**

- Lead / customer management  
- Quote generation  
- Job scheduling / dispatch  
- Technician mobile workflow  
- GPS tracking / real-time dispatch  
- Invoicing / payments  
- Notifications / communications  
- AI-assisted pricing / dispatch  
- External system integrations  

---

## 2. Core architecture

| Concern | Choice |
|--------|--------|
| Style | **Microservices**, **event-driven**, **CQRS**, **Clean Architecture** per service |
| Reliable publish | **Outbox pattern** → event backbone |
| Event backbone | **Kafka / MSK**; major state changes emit **domain events** |
| Primary write DB | **PostgreSQL** |
| Read models / cache / search | **Redis**, **OpenSearch** (projections as needed) |
| Multi-tenancy | **Strict tenant isolation**; tenant-aware schema / partitioning strategy as the platform grows |

**Database table naming (normative):**

- **FGS** is the product acronym; the PostgreSQL schema name stays **`fgs`** (lowercase, as PostgreSQL prefers).
- **Global / platform reference tables:** PascalCase name with prefix **`Glo`** (e.g. `GloCountry`, `GloAccountingIntegrationType`, `GloBusinessType`, `GloLanguage`, `GloMasterEntityType`). No `TenantId` / `CompanyId`.
- **Tenant or company setup tables:** PascalCase name with prefix **`FgsSetup`** (e.g. `FgsSetupTax`, `FgsSetupPostalCode`). Use `TenantId` / `CompanyId` where applicable.
- Do **not** use the legacy spelling **`FSGSetup*`** for new tables. See `.cursor/fgs_setup_tables_1.md` for the full catalog.

**Engineering expectations (when generating code):**

- Clean Architecture: domain isolated from infrastructure  
- Idempotent event consumers  
- Repository / service abstractions where it helps testability  
- **APIs and queries are tenant-aware**; enforce authorization at **service boundaries**  
- Prefer async / event-driven workflows where appropriate  
- Production-grade, scalable defaults  

---

## 3. Tenancy: `tenant_id` and `CompanyId`

### 3.1 Meaning

- **`tenant_id` (UUID)** — Billing / customer / **account** boundary: who pays and who owns the account.  
- **`CompanyId`** — Surrogate key for a subsidiary / branch / **legal entity** (tenant company) under that tenant. Meaningful **only** together with **`tenant_id`** (typically `BIGINT` or `UUID`, assigned by the platform).

**Cardinality:**

- One `tenant_id` may have **one or many** tenant companies (subsidiaries / legal entities).  
- **Most** tenants: a single legal entity → one **`CompanyId`** row created at onboarding (default tenant company).  
- **Multiple** legal entities: same `tenant_id`; each entity has a **distinct** **`CompanyId`** in the tenant-company catalog.

**Example**

- Single entity: `tenant_id = UUID-ABC`, one **`CompanyId`** (default company).  
- Multiple entities (same bill): `tenant_id = UUID-XYZ`, distinct **`CompanyId`** values — e.g. **501** → Houston Plumbing LLC; **502** → Dallas HVAC LLC; **503** → Austin Electric LLC.

### 3.2 Data model (domain tables)

- **Every tenant-domain table** (operational data scoped to a customer) includes **`tenant_id`** and **`CompanyId`**.  
- They are the **first two columns** (convention and indexing).  
- They are **part of the primary key**: typically `PRIMARY KEY (tenant_id, CompanyId, …)` plus entity id or natural key.

**Registry exceptions (not “company-scoped domain rows”):**

- The **`tenant`** row itself defines `tenant_id`; it does not carry `CompanyId`.  
- **`tenant_company`** (or equivalent): `PRIMARY KEY (tenant_id, CompanyId)` — catalog of legal entities under the tenant.

### 3.3 Users and multiple `CompanyId` values

- A **single user** may belong to **multiple** `CompanyId` values under the same `tenant_id`.  
- **Identity** table: one row per person per tenant, e.g. **`PRIMARY KEY (tenant_id, user_id)`** (no single `CompanyId` on the PK — the person is not duplicated per subsidiary).  
- **Membership** table (domain): **`PRIMARY KEY (tenant_id, CompanyId, user_id)`** — one row per user per subsidiary they are allowed to work in; multiple rows = multiple `CompanyId` assignments.  
- Authorization resolves **allowed `CompanyId` set** from membership + roles; **company access is never implicit** from sharing a tenant alone.

### 3.4 Queries and APIs

- **Domain reads/writes:** **`tenant_id` and `CompanyId` are the first two parameters** (route, query, or handler signature) after cancellation/context where applicable.  
- Data is **separated** by **`(tenant_id, CompanyId)`**; cross–`CompanyId` access requires **explicit** RBAC (e.g. tenant-level role that grants access to all companies under that tenant).

### 3.5 Domain events

All **domain events** MUST include:

- `tenant_id`  
- `CompanyId`  

### 3.6 Access control

- **`tenant_id`** is the **hard** security and billing boundary.  
- **Company relationships do not bypass authorization:** same tenant does **not** grant access to another `CompanyId` by default.  
- **Tenant-level users** may access **all** company-scoped entities **only if** role / policy allows.  
- **Company-scoped users** access **only** assigned companies (via membership and policy).  
- Enforce **company-level scope** on every applicable request.

---

## 4. Multi-tenant isolation (summary)

- Isolation by **`tenant_id`** is mandatory.  
- **`CompanyId`** scope is mandatory for company-scoped domain operations.  
- Hierarchy / “franchise” convenience **never** replaces **explicit** RBAC and policy checks.

---

## 5. Authentication and identity

### 5.1 Identity provider

- **Microsoft Entra External ID / Azure AD B2C**  
- **Entra:** authentication only (proof of identity).  
- **This platform:** authorization, tenant membership, roles, permissions, and **mapping** from Entra to internal users.

### 5.2 Canonical external key

- Link users using Entra’s **object identifier** in tokens (**`oid` claim**), with **issuer** (`iss`), stored internally (e.g. `auth_identity`: unique on `(issuer, object_id)`). Treat **`oid`** as the **canonical** external key unless product security docs define otherwise.

### 5.3 Invite registration flow (normative sequence)

All steps below are **required semantics** for the MVP invite path.

1. **Company signup** — caller completes tenant onboarding (self-serve or provisioned).  
2. **Backend creates** **Tenant**, **Admin User**, **Invite**, and **default tenant company** (initial **`CompanyId`**) per tenancy rules.  
3. **Invite email sent to the admin** (invited party) with link containing an **opaque** token (raw token not stored; **hash** + metadata in DB).  
4. **User opens invite link** — browser hits your app with token (prefer POST/short-lived exchange so the token is not logged in referrers).  
5. **Validate invite token** — load by hash; check status, expiry, revocation, tenant/user linkage; never trust the URL alone.  
   - **If invalid or expired:** respond with **error UX** (generic copy acceptable for security); **do not** continue to Entra.  
6. **Redirect to Microsoft Entra External ID** — start OIDC **authorization code** flow (signup/sign-in user flow as configured).  
7. **User registers or authenticates** at Entra.  
8. **Entra redirects to your auth callback** with `code` (and `state`).  
9. **Backend exchanges authorization code for tokens** (server-side, with **PKCE** verifier if used).  
10. **Validate ID token** (signature, `iss`, `aud`, `nonce`, lifetime).  
11. **Extract claims** used by this platform: at minimum **`oid`**, **email** (policy-dependent claim), **name** (for profile/display).  
12. **Validate invite context** — reload invite + user from DB; ensure still pending; match **Entra email** to **invited email** / internal user email (**normalized**).  
13. **Persist OID** — upsert **`auth_identity`** (`issuer`, `object_id` from `oid`) for the internal **`user_id`** from the invite (transactional with next steps).  
14. **Mark invite accepted** — single-use: status **accepted**, timestamps as designed; concurrent attempts must fail safely.  
15. **Create app session or internal JWT** carrying `tenant_id`, allowed `CompanyId` context (from invite + membership), and roles when available.  
16. **Redirect to post-login destination** (e.g. **dashboard**).

### 5.4 Future login flow (normative sequence)

1. **User visits login page.**  
2. **User enters email / username / phone** (whatever identifiers you support for lookup).  
3. **Backend looks up internal user** (and tenant) from that identifier — scope query to avoid cross-tenant leakage.  
   - **If not found:** return **generic error** (same message as invalid password style) to reduce **account enumeration**.  
4. **Redirect to Entra login** (OIDC); use **`login_hint`** when appropriate to reduce friction.  
5. **User authenticates** at Entra.  
6. **Entra returns tokens** to your callback (same code path as invite callback or dedicated login callback).  
7. **Backend extracts `oid`** (and `iss`) from the validated ID token.  
8. **Lookup internal user by OID** via **`auth_identity`**.  
   - **If mapping missing:** run **reconciliation** path or **controlled error** (see §5.6 — no unsafe auto-link on ambiguous matches).  
9. **Create session** (or internal JWT) as in invite completion.  
10. **Redirect to dashboard** (or deep link).

### 5.5 Sequence diagrams (verbatim reference)

Invite registration (ASCII):

```text
Company Signup
    |
    v
Backend Creates Tenant/User/Invite
    |
    v
Invite Email Sent to Admin
    |
    v
User Clicks Invite Link
    |
    v
Validate Invite Token
    |
    +--> Invalid/Expired -> Show Error
    |
    v
Redirect to Microsoft Entra External ID
    |
    v
User Registers / Authenticates
    |
    v
Entra Redirects to Auth Callback
    |
    v
Backend Exchanges Auth Code for Token
    |
    v
Extract Claims (OID, Email, Name)
    |
    v
Validate Invite Context + Email Match
    |
    v
Persist OID to User Record
    |
    v
Mark Invite Accepted
    |
    v
Create App Session / JWT
    |
    v
Redirect to Dashboard
```

Future login (ASCII):

```text
User Visits Login Page
    |
    v
Enter Email / Username / Phone
    |
    v
Backend Looks Up User
    |
    +--> Not Found -> Generic Error
    |
    v
Redirect to Entra Login Flow
    |
    v
User Authenticates
    |
    v
Entra Returns Token
    |
    v
Backend Extracts OID
    |
    v
Lookup User by OID
    |
    +--> Missing OID Mapping -> Reconciliation / Error
    |
    v
Create Session
    |
    v
Redirect to Dashboard
```

### 5.6 Invite, callback, and session rules

- **Never trust the invite token alone** — validate against persisted state (hash, status, expiry, user linkage).  
- **Opaque invite tokens** stored in DB as **hash** + metadata; raw token only in transit to the user.  
- **Single-use** invite after acceptance; enforce with DB transaction / unique constraints as needed.  
- **Expiry** enforced at validation and optionally background job (`expired` status).  
- On first registration via invite, **invited email must match** Entra email (**normalized** comparison).  
- **OIDC `state`** (and **PKCE**) during Entra redirect — bind callback to the invite or login session; include **correlation ID** for tracing.  
- **Callback idempotency** — duplicate callbacks (double submit, two tabs) must not create duplicate identities or double-accept invites; rely on unique `(issuer, object_id)` and invite status.  
- **Reconcile missing OID mappings** on a later login only under **explicit, safe** rules (avoid ambiguous multi-user email matches; prefer admin or verified recovery when in doubt).  
- **“Persist OID to User record”** means persist on **`auth_identity`** (and optional profile fields on **`users`**) keyed to the internal **`user_id`** from the invite flow — not a second competing user row.

---

## 6. Core domain services (target decomposition)

- User Service  
- Tenant / Setup Service  
- Customer Service  
- Lead Service  
- Job Service  
- Scheduling Service  
- Quote Service  
- Invoice Service  
- Payment Service  
- Notification Service  
- File Service  
- Search Service  
- Integration Service  
- AI Agent Service  
- Policy Engine Service  

---

## 7. Critical non-functional requirements

- **Sub-2 second** job assignment latency (dispatch path)  
- Scale to **very large** technician counts  
- **Strict** tenant isolation  
- **Strong consistency** for financial operations  
- **No event loss:** outbox + Kafka (or equivalent) discipline  
- Real-time / high-volume **technician GPS** updates where applicable  
- **RBAC** everywhere it matters  
- **Full auditability** of state transitions  

---

## 8. Suggested MVP scope

1. Tenant / user / invite management (incl. subsidiary / `CompanyId` model)  
2. Entra authentication integration (invite + callback + OID linkage + login/reconciliation)  
3. Customer management  
4. Job management  
5. Basic scheduling / dispatch  
6. Technician assignment  
7. Real-time job status updates  
8. Invoice generation  
9. Payment processing integration  

---

## 9. Current immediate focus

Implement **tenant signup + invite + Entra registration**:

- Company signup endpoint  
- Tenant / user / invite / default tenant company (**initial `CompanyId`**) creation  
- Invite email generation  
- Invite validation endpoint  
- Entra redirect and callback handlers  
- OID linkage to internal users  
- Login and reconciliation flow  
