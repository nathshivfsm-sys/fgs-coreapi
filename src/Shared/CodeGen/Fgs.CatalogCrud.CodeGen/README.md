# Fgs.CatalogCrud.CodeGen

Generates catalog CRUD artifacts (DTOs, descriptors, validators, controllers) from domain entity metadata.

## Prerequisites

Build the target service domain project first so the scanner can load its assembly:

```powershell
dotnet build src/SetupService/Fgs.Setup.Domain/Fgs.Setup.Domain.csproj
```

## Usage — Setup service

```powershell
dotnet run --project src/Shared/CodeGen/Fgs.CatalogCrud.CodeGen -- --service Setup
```

## Usage — custom service

```powershell
dotnet run --project src/Shared/CodeGen/Fgs.CatalogCrud.CodeGen -- `
  --service Inventory `
  --infrastructure-path src/InventoryService/Fgs.Inventory.Infrastructure `
  --application-path src/InventoryService/Fgs.Inventory.Application `
  --api-path src/InventoryService/Fgs.Inventory.API `
  --domain-project src/InventoryService/Fgs.Inventory.Domain/Fgs.Inventory.Domain.csproj `
  --application-namespace Fgs.Inventory.Application `
  --api-namespace Fgs.Inventory.API `
  --entity-namespace Fgs.Inventory.Domain.Entities `
  --default-schema inventory `
  --exclude EntityToExclude1,EntityToExclude2
```

## Options

| Flag | Description |
|------|-------------|
| `--service` | Service profile name (`Setup` has built-in defaults) |
| `--entity` | Generate a single entity |
| `--dry-run` | List entities without writing files |
| `--exclude` | Comma-separated entity class names to skip |

## Output (per service)

- `{Application}/Features/Generated/Dtos`
- `{Application}/Features/Generated/Descriptors`
- `{Application}/Features/Generated/Validators`
- `{Application}/Common/Catalog/EntityKeys.cs`
- `{Application}/Common/Catalog/EntityRegistry.Generated.cs`
- `{Application}/Common/Catalog/{Service}CatalogEntityRegistration.Generated.cs`
- `{Api}/Controllers/Generated`
