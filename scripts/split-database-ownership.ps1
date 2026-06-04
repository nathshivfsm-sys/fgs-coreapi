# Splits UserService domain assets into Setup, File, and Audit services.
$ErrorActionPreference = 'Stop'
$root = 'c:\SourceCode\FGS\src'

$userEntities = @(
    'FgsUser.cs', 'FgsUserRole.cs', 'FgsRole.cs', 'FgsInvitation.cs',
    'FgsTenant.cs', 'FgsTenantCompany.cs', 'FgsTenantServiceSetup.cs', 'FgsLocation.cs',
    'VendorTypes.cs', 'WarehouseTypes.cs', 'VehicleOwnershipTypes.cs'
)
$fileEntities = @('FgsFile.cs')
$auditEntities = @('FgsCredentialAudit.cs')

function Copy-WithNamespace {
    param(
        [string]$SourceDir,
        [string]$TargetDir,
        [string]$FromNs,
        [string]$ToNs,
        [string[]]$Files
    )
    New-Item -ItemType Directory -Path $TargetDir -Force | Out-Null
    foreach ($file in $Files) {
        $src = Join-Path $SourceDir $file
        if (-not (Test-Path $src)) { continue }
        $content = Get-Content -LiteralPath $src -Raw -Encoding UTF8
        $content = $content.Replace($FromNs, $ToNs)
        Set-Content -LiteralPath (Join-Path $TargetDir $file) -Value $content -Encoding UTF8 -NoNewline
    }
}

function Copy-AllExcept {
    param(
        [string]$SourceDir,
        [string]$TargetDir,
        [string]$FromNs,
        [string]$ToNs,
        [string[]]$Exclude
    )
    New-Item -ItemType Directory -Path $TargetDir -Force | Out-Null
    Get-ChildItem -Path $SourceDir -Filter '*.cs' | Where-Object { $Exclude -notcontains $_.Name } | ForEach-Object {
        $content = Get-Content -LiteralPath $_.FullName -Raw -Encoding UTF8
        $content = $content.Replace($FromNs, $ToNs)
        Set-Content -LiteralPath (Join-Path $TargetDir $_.Name) -Value $content -Encoding UTF8 -NoNewline
    }
}

# Entities
$entitySrc = Join-Path $root 'UserService\Fgs.User.Domain\Entities'
Copy-AllExcept $entitySrc (Join-Path $root 'SetupService\Fgs.Setup.Domain\Entities') 'Fgs.User.Domain' 'Fgs.Setup.Domain' ($userEntities + $fileEntities + $auditEntities)
Copy-WithNamespace $entitySrc (Join-Path $root 'FileService\Fgs.File.Domain\Entities') 'Fgs.User.Domain' 'Fgs.File.Domain' $fileEntities
Copy-WithNamespace $entitySrc (Join-Path $root 'AuditService\Fgs.Audit.Domain\Entities') 'Fgs.User.Domain' 'Fgs.Audit.Domain' $auditEntities

# Configurations
$configSrc = Join-Path $root 'UserService\Fgs.User.Infrastructure\Persistence\Database\Configurations'
$userConfigPrefixes = @('FgsUser', 'FgsRole', 'FgsInvitation', 'FgsTenant', 'FgsLocation')
$fileConfigs = @('FgsFileConfiguration.cs')
$auditConfigs = @('FgsCredentialAuditConfiguration.cs')

New-Item -ItemType Directory -Path (Join-Path $root 'SetupService\Fgs.Setup.Infrastructure\Database\Configurations') -Force | Out-Null
New-Item -ItemType Directory -Path (Join-Path $root 'FileService\Fgs.File.Infrastructure\Database\Configurations') -Force | Out-Null
New-Item -ItemType Directory -Path (Join-Path $root 'AuditService\Fgs.Audit.Infrastructure\Database\Configurations') -Force | Out-Null

Get-ChildItem -Path $configSrc -Filter '*.cs' | ForEach-Object {
    $name = $_.Name
    $content = Get-Content -LiteralPath $_.FullName -Raw -Encoding UTF8
    if ($name -eq 'FgsUserDbContextConfigurationExtensions.cs') {
        $content = $content.Replace('Fgs.User.Infrastructure.Persistence.Database', 'Fgs.Setup.Infrastructure.Database')
        $content = $content.Replace('Fgs.User.Domain', 'Fgs.Setup.Domain')
        Set-Content -LiteralPath (Join-Path $root 'SetupService\Fgs.Setup.Infrastructure\Database\Configurations\FgsSetupDbContextConfigurationExtensions.cs') -Value $content -Encoding UTF8 -NoNewline
        return
    }
    if ($fileConfigs -contains $name) {
        $content = $content.Replace('Fgs.User', 'Fgs.File')
        Set-Content -LiteralPath (Join-Path $root "FileService\Fgs.File.Infrastructure\Database\Configurations\$name") -Value $content -Encoding UTF8 -NoNewline
        return
    }
    if ($auditConfigs -contains $name) {
        $content = $content.Replace('Fgs.User', 'Fgs.Audit')
        Set-Content -LiteralPath (Join-Path $root "AuditService\Fgs.Audit.Infrastructure\Database\Configurations\$name") -Value $content -Encoding UTF8 -NoNewline
        return
    }
    $isUser = $false
    foreach ($p in $userConfigPrefixes) {
        if ($name.StartsWith($p)) { $isUser = $true; break }
    }
    if ($isUser) { return }
    $content = $content.Replace('Fgs.User', 'Fgs.Setup')
    Set-Content -LiteralPath (Join-Path $root "SetupService\Fgs.Setup.Infrastructure\Database\Configurations\$name") -Value $content -Encoding UTF8 -NoNewline
}

# Copy seeds to Setup
$seedSrc = Join-Path $root 'UserService\Fgs.User.Infrastructure\Persistence\Database\Seed'
$seedDst = Join-Path $root 'SetupService\Fgs.Setup.Infrastructure\Database\Seeds'
if (Test-Path $seedSrc) {
    New-Item -ItemType Directory -Path $seedDst -Force | Out-Null
    Copy-Item -Path (Join-Path $seedSrc '*') -Destination $seedDst -Recurse -Force
    # Keep platform tenant seed in User
    $platformSeed = Join-Path $seedDst 'Platform_Tenant_Seed.sql'
    if (Test-Path $platformSeed) {
        $userSeedDst = Join-Path $root 'UserService\Fgs.User.Infrastructure\Database\Seeds'
        New-Item -ItemType Directory -Path $userSeedDst -Force | Out-Null
        Move-Item -LiteralPath $platformSeed -Destination (Join-Path $userSeedDst 'Platform_Tenant_Seed.sql') -Force
        $platformCs = Join-Path $seedDst 'PlatformTenantSeeder.cs'
        if (Test-Path $platformCs) {
            Move-Item -LiteralPath $platformCs -Destination (Join-Path $userSeedDst 'PlatformTenantSeeder.cs') -Force
        }
    }
}

Write-Host 'Split complete.'
