# FGS API — Postman Collections

Postman assets are split into two folders — import **one or both** depending on where you deploy.

```
docs/api/
├── local/          ← Docker Desktop (https://developer.fsm.com)
│   ├── FGS.postman_collection.json
│   └── FGS-Globals.postman_environment.json
├── ec2/            ← EC2 dev (http://100.54.14.213)
│   ├── FGS.postman_collection.json
│   └── FGS-Globals.postman_environment.json
├── sources/        ← Curated inputs for the generator (BFF, Entra token)
└── scripts/
    ├── Generate-PostmanCollections.ps1
    └── Import-PostmanDesktop.ps1
```

## Import into Postman (desktop)

**All (local + EC2):**

```powershell
powershell -ExecutionPolicy Bypass -File docs/api/scripts/Import-PostmanDesktop.ps1
```

**Local only:**

```powershell
powershell -ExecutionPolicy Bypass -File docs/api/scripts/Import-PostmanDesktop.ps1 -Target local
```

**EC2 only:**

```powershell
powershell -ExecutionPolicy Bypass -File docs/api/scripts/Import-PostmanDesktop.ps1 -Target ec2
```

**Manual:** Import the folder `docs/api/local/` or `docs/api/ec2/` (collection + environment together).

## Quick start

| Target | Collection | Environment |
|--------|------------|-------------|
| Local Docker | **FGS Local Docker** | **FGS Globals (Local Docker)** |
| EC2 Dev | **FGS EC2 Dev** | **FGS Globals (EC2 Dev)** |

1. Import the matching folder.
2. Select the environment in Postman (top-right).
3. Run **00 - Authentication & Token** before protected APIs.
4. Set `entraClientSecret` in the environment.

## Collection hierarchy (both targets)

```
FGS Local Docker / FGS EC2 Dev
├── 00 - Authentication & Token
│   ├── Entra Token (Existing User)
│   ├── 00 - Authentication Flow
│   └── 01 - UI Login Flow
├── User Service
├── BFF Service        (EC2 only)
├── Setup Service
└── … (local includes all services; EC2 includes user + setup + bff)
```

## URL patterns

| Target | Gateway (baked into collection) | Example |
|--------|----------------------------------|---------|
| Local | `https://developer.fsm.com` | `/api/v1/billingcategory` |
| EC2 | `http://100.54.14.213` | `/setup-service/api/v1/billingcategory` |

Environments hold secrets (`accessToken`, `tenantId`, `redirectUri`, etc.), not gateway URLs.

## Regenerate

After controller changes:

```powershell
powershell -ExecutionPolicy Bypass -File docs/api/scripts/Generate-PostmanCollections.ps1
```

Outputs:
- `docs/api/local/FGS.postman_collection.json`
- `docs/api/ec2/FGS.postman_collection.json`

Environment files in `local/` and `ec2/` are maintained manually (not overwritten by the generator).

## EC2 notes

- Only **setup**, **user**, and **bff** are in the EC2 collection.
- Entra `redirectUri`: `http://100.54.14.213/user-service/api/v1/auth/entra/callback`
- Production Entra requires **HTTPS** for redirect URIs.

See also: [Entra API Connector setup](../entra-api-connector-setup.md)
