param(
    [Parameter(Mandatory = $true)]
    [string]$ServiceName,

    [Parameter(Mandatory = $true)]
    [string]$MigrationName,

    [string]$RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
)

$serviceMap = @{
    User         = @{ Infra = 'UserService\Fgs.User.Infrastructure'; Api = 'UserService\Fgs.User.API'; Conn = 'FgsUser' }
    Setup        = @{ Infra = 'SetupService\Fgs.Setup.Infrastructure'; Api = 'SetupService\Fgs.Setup.API'; Conn = 'FgsSetup' }
    File         = @{ Infra = 'FileService\Fgs.File.Infrastructure'; Api = 'FileService\Fgs.File.API'; Conn = 'FgsFile' }
    Audit        = @{ Infra = 'AuditService\Fgs.Audit.Infrastructure'; Api = 'AuditService\Fgs.Audit.API'; Conn = 'FgsAudit' }
    Notification = @{ Infra = 'NotificationService\Fgs.Notification.Infrastructure'; Api = 'NotificationService\Fgs.Notification.API'; Conn = 'FgsNotification' }
    Billing      = @{ Infra = 'BillingService\Fgs.Billing.Infrastructure'; Api = 'BillingService\Fgs.Billing.API'; Conn = 'FgsBilling' }
    Crm          = @{ Infra = 'CrmService\Fgs.Crm.Infrastructure'; Api = 'CrmService\Fgs.Crm.API'; Conn = 'FgsCrm' }
    Scheduling   = @{ Infra = 'SchedulingService\Fgs.Scheduling.Infrastructure'; Api = 'SchedulingService\Fgs.Scheduling.API'; Conn = 'FgsDispatch' }
    Inventory    = @{ Infra = 'InventoryService\Fgs.Inventory.Infrastructure'; Api = 'InventoryService\Fgs.Inventory.API'; Conn = 'FgsInventory' }
    Reporting    = @{ Infra = 'ReportingService\Fgs.Reporting.Infrastructure'; Api = 'ReportingService\Fgs.Reporting.API'; Conn = 'FgsReporting' }
    Integration      = @{ Infra = 'IntegrationService\Fgs.Integration.Infrastructure'; Api = 'IntegrationService\Fgs.Integration.API'; Conn = 'FgsIntegration' }
    Asset            = @{ Infra = 'AssetService\Fgs.Asset.Infrastructure'; Api = 'AssetService\Fgs.Asset.API'; Conn = 'FgsAsset' }
    ServiceAgreement = @{ Infra = 'ServiceAgreementService\Fgs.ServiceAgreement.Infrastructure'; Api = 'ServiceAgreementService\Fgs.ServiceAgreement.API'; Conn = 'FgsServiceAgreement' }
}

if (-not $serviceMap.ContainsKey($ServiceName)) {
    throw "Unknown service '$ServiceName'. Valid: $($serviceMap.Keys -join ', ')"
}

$cfg = $serviceMap[$ServiceName]
$infraProject = Join-Path $RepoRoot "src\$($cfg.Infra)\$($cfg.Infra.Split('\')[-1]).csproj"
$apiProject = Join-Path $RepoRoot "src\$($cfg.Api)\$($cfg.Api.Split('\')[-1]).csproj"
$scriptsRoot = Join-Path $RepoRoot "src\$($cfg.Infra)\Database\Scripts"
$executeDir = Join-Path $scriptsRoot 'Execute'
$rollbackDir = Join-Path $scriptsRoot 'Rollback'

Push-Location (Join-Path $RepoRoot 'src')
try {
    Write-Host "Adding migration $MigrationName..."
    dotnet ef migrations add $MigrationName -p $infraProject -s $apiProject
    if ($LASTEXITCODE -ne 0) { throw 'dotnet ef migrations add failed' }

    $migrationFile = Get-ChildItem (Join-Path $RepoRoot "src\$($cfg.Infra)\Database\Migrations") -Filter "*_${MigrationName}.cs" |
        Sort-Object LastWriteTime -Descending |
        Select-Object -First 1
    if (-not $migrationFile) { throw "Could not find migration file for $MigrationName" }

    $migrationId = $migrationFile.BaseName
    $upSql = Join-Path $executeDir "${migrationId}_up.sql"
    $downSql = Join-Path $rollbackDir "${migrationId}_down.sql"

    New-Item -ItemType Directory -Force -Path $executeDir, $rollbackDir | Out-Null

    Write-Host "Generating $upSql..."
    dotnet ef migrations script -i -o $upSql -p $infraProject -s $apiProject
    if ($LASTEXITCODE -ne 0) { throw 'dotnet ef migrations script failed' }

    if (-not (Test-Path $downSql)) {
        @"
-- Rollback for $migrationId
-- TODO: author idempotent DROP statements mirroring schema objects created in ${migrationId}_up.sql
"@ | Set-Content $downSql -Encoding UTF8
        Write-Host "Created placeholder rollback: $downSql"
    }

    Write-Host "Done. MigrationId=$migrationId"
}
finally {
    Pop-Location
}
