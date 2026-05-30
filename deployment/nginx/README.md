# Moved: FGS API Gateway

The NGINX API Gateway has moved to **[src/Gateway](../../src/Gateway/README.md)**.

```powershell
cd src/Gateway
.\scripts\generate-local-cert.ps1
docker compose up --build
```

This folder is kept only as a pointer for older docs and scripts that referenced `deployment/nginx`.
