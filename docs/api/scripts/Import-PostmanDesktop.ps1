<#
.SYNOPSIS
    Import FGS Postman collections into Postman desktop (Windows).
.DESCRIPTION
    Bundles all *.postman_*.json files into a single zip and opens it in Postman.
    Postman desktop recognizes the archive and shows the Import preview.
#>
param(
    [string]$ApiDir = (Join-Path $PSScriptRoot "..")
)

$ErrorActionPreference = "Stop"
$ApiDir = (Resolve-Path $ApiDir).Path
$zipPath = Join-Path $ApiDir "FGS-Postman-Import.zip"
$postmanExe = Join-Path $env:LOCALAPPDATA "Postman\Postman.exe"

if (-not (Test-Path $postmanExe)) {
    throw "Postman desktop not found at $postmanExe. Install from https://www.postman.com/downloads/"
}

$files = @(Get-ChildItem $ApiDir -Filter "*.postman_*.json")
if ($files.Count -eq 0) {
    throw "No Postman JSON files found in $ApiDir"
}

if (Test-Path $zipPath) { Remove-Item $zipPath -Force }
Compress-Archive -Path ($files | ForEach-Object FullName) -DestinationPath $zipPath -Force

Write-Host "Import bundle: $zipPath ($($files.Count) files)"
Write-Host "Opening Postman desktop..."

Start-Process -FilePath $postmanExe -ArgumentList "`"$zipPath`""

Write-Host @"

In Postman:
  1. Confirm the Import preview (18 items: 17 collections + 1 environment)
  2. Click Import
  3. Select environment 'FGS Globals (Local)' in the top-right dropdown

If the import dialog did not appear:
  - In Postman click Import (top-left)
  - Drag folder or zip: $ApiDir
  - Or select: $zipPath

"@
