<#
.SYNOPSIS
    Import FGS Postman collections into Postman desktop (Windows).
.DESCRIPTION
    Bundles local and/or EC2 collection + environment pairs into a zip.
.PARAMETER Target
    local, ec2, or all (default all).
#>
param(
    [string]$ApiDir = (Join-Path $PSScriptRoot ".."),
    [ValidateSet('local', 'ec2', 'all')]
    [string]$Target = 'all'
)

$ErrorActionPreference = "Stop"
$ApiDir = (Resolve-Path $ApiDir).Path
$zipPath = Join-Path $ApiDir "FGS-Postman-Import.zip"
$postmanExe = Join-Path $env:LOCALAPPDATA "Postman\Postman.exe"

if (-not (Test-Path $postmanExe)) {
    throw "Postman desktop not found at $postmanExe. Install from https://www.postman.com/downloads/"
}

$importFiles = @()
if ($Target -in @('local', 'all')) {
    $importFiles += @(
        (Join-Path $ApiDir "local\FGS.postman_collection.json")
        (Join-Path $ApiDir "local\FGS-Globals.postman_environment.json")
    )
}
if ($Target -in @('ec2', 'all')) {
    $importFiles += @(
        (Join-Path $ApiDir "ec2\FGS.postman_collection.json")
        (Join-Path $ApiDir "ec2\FGS-Globals.postman_environment.json")
    )
}

$missing = @($importFiles | Where-Object { -not (Test-Path $_) })
if ($missing.Count -gt 0) {
    throw "Missing import files:`n$($missing -join "`n")`nRun Generate-PostmanCollections.ps1 first."
}

if (Test-Path $zipPath) { Remove-Item $zipPath -Force }
Compress-Archive -Path $importFiles -DestinationPath $zipPath -Force

Write-Host "Import bundle: $zipPath ($($importFiles.Count) files, target=$Target)"
Write-Host "Opening Postman desktop..."

Start-Process -FilePath $postmanExe -ArgumentList "`"$zipPath`""

Write-Host @"

In Postman:
  1. Confirm the Import preview
  2. Click Import
  3. Local: open **FGS Local Docker** + environment **FGS Globals (Local Docker)**
  4. EC2:   open **FGS EC2 Dev** + environment **FGS Globals (EC2 Dev)**

Import one folder only:
  powershell -ExecutionPolicy Bypass -File docs/api/scripts/Import-PostmanDesktop.ps1 -Target local
  powershell -ExecutionPolicy Bypass -File docs/api/scripts/Import-PostmanDesktop.ps1 -Target ec2

"@
