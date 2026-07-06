# Generates ALL Asset Service catalog CRUD files.
$ErrorActionPreference = 'Stop'

$assetRoot = Split-Path $PSScriptRoot -Parent
$genPy = Join-Path $PSScriptRoot 'gen_asset_crud_full.py'

if (-not (Test-Path $genPy)) {
    throw "Generator engine not found: $genPy"
}

Write-Host "Running Asset CRUD generator: $genPy"
python $genPy
if ($LASTEXITCODE -ne 0) {
    throw "Generator failed with exit code $LASTEXITCODE"
}

$manifest = Join-Path $assetRoot 'scripts\_generated_files.txt'
if (Test-Path $manifest) {
    $files = Get-Content $manifest
    Write-Host ""
    Write-Host "Created/updated $($files.Count) files:"
    $files | ForEach-Object { Write-Host "  $_" }
}
else {
    Write-Host "Generation complete (manifest not found)."
}
