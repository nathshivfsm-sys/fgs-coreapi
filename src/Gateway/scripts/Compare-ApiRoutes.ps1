<#
.SYNOPSIS
  Compares FgsVersionedRoute controller templates against Gateway api-v1-routes configs.
#>
param(
    [string]$RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
)

$ErrorActionPreference = "Stop"

$controllerRoots = @(
    "AssetService\Fgs.Asset.API\Controllers",
    "AuditService\Fgs.Audit.API\Controllers",
    "BffService\Fgs.Bff.API\Controllers",
    "FileService\Fgs.File.API\Controllers",
    "InventoryService\Fgs.Inventory.API\Controllers",
    "NotificationService\Fgs.Notification.API\Controllers",
    "SetupService\Fgs.Setup.API\Controllers",
    "UserService\Fgs.User.API\Controllers"
)

$routes = New-Object System.Collections.Generic.HashSet[string]
foreach ($rel in $controllerRoots) {
    $dir = Join-Path $RepoRoot "src\$rel"
    if (-not (Test-Path $dir)) { continue }
    Get-ChildItem $dir -Filter *.cs -Recurse | ForEach-Object {
        $text = Get-Content $_.FullName -Raw
        [regex]::Matches($text, 'FgsVersionedRoute\("([^"]+)"\)') | ForEach-Object {
            $template = $_.Groups[1].Value
            if ($template -eq '[controller]') { return }
            [void]$routes.Add($template)
        }
    }
}

function Test-RouteCovered([string]$route, [string]$nginx) {
    if ($route -match '^tenant/\{') {
        return $nginx -match 'businesstype'
    }
    if ($route -eq 'tenant') {
        return $nginx -match '/api/v1/tenant'
    }
    if ($route -eq 'internal/users') {
        return $nginx -match 'internal/users'
    }
    if ($route -eq 'bff/signup') {
        return $nginx -match '/api/v1/bff'
    }
    $seg = ($route -split '/')[0]
    return $nginx -match [regex]::Escape($seg)
}

foreach ($name in @("api-v1-routes.conf", "api-v1-routes.prod.conf")) {
    $path = Join-Path $RepoRoot "src\Gateway\conf.d\includes\$name"
    $nginx = Get-Content $path -Raw
    Write-Output "==== $name ===="
    $missing = @()
    foreach ($r in ($routes | Sort-Object)) {
        if (-not (Test-RouteCovered $r $nginx)) {
            $missing += $r
        }
    }
    if ($missing.Count -eq 0) {
        Write-Output "All controller routes covered."
    } else {
        Write-Output "MISSING:"
        $missing | ForEach-Object { Write-Output "  $_" }
    }

    # Orphan nginx segments that look like catalog tokens but have no controller
    $catalogMatch = [regex]::Match($nginx, 'location ~ \^/api/v1/\(([^)]+)\)')
    # Collect all catalog groups
    $catalogGroups = [regex]::Matches($nginx, 'location ~ \^/api/v1/\(([^)]+)\)\(/\|\$\)')
    $nginxTokens = New-Object System.Collections.Generic.HashSet[string]
    foreach ($m in $catalogGroups) {
        foreach ($tok in ($m.Groups[1].Value -split '\|')) {
            [void]$nginxTokens.Add($tok)
        }
    }

    $controllerTokens = New-Object System.Collections.Generic.HashSet[string]
    foreach ($r in $routes) {
        if ($r -match '^tenant/\{' -or $r -eq 'internal/users' -or $r -eq 'bff/signup') { continue }
        [void]$controllerTokens.Add(($r -split '/')[0])
    }

    $orphans = @()
    foreach ($t in ($nginxTokens | Sort-Object)) {
        if (-not $controllerTokens.Contains($t)) {
            $orphans += $t
        }
    }
    if ($orphans.Count -gt 0) {
        Write-Output "ORPHAN catalog tokens:"
        $orphans | ForEach-Object { Write-Output "  $_" }
    } else {
        Write-Output "No orphan catalog tokens."
    }
    Write-Output ""
}

# Prefix locations that rewrite/strip path
Write-Output "==== Prefix/rewrite locations to review ===="
Select-String -Path (Join-Path $RepoRoot "src\Gateway\conf.d\includes\api-v1-routes*.conf") -Pattern "rewrite |location /api/v1/users|location /api/v1/notification" |
    ForEach-Object { Write-Output ("{0}:{1}: {2}" -f $_.Filename, $_.LineNumber, $_.Line.Trim()) }
