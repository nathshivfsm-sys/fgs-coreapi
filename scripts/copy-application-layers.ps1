$ErrorActionPreference = 'Stop'
$root = 'c:\SourceCode\FGS\src'

function Copy-Tree($src, $dst, $from, $to) {
    if (-not (Test-Path $src)) { return }
    Get-ChildItem $src -Recurse -File | ForEach-Object {
        $rel = $_.FullName.Substring($src.Length).TrimStart('\')
        $target = Join-Path $dst $rel
        New-Item -ItemType Directory -Path (Split-Path $target -Parent) -Force | Out-Null
        $c = (Get-Content -LiteralPath $_.FullName -Raw) -replace $from, $to
        $c = $c -replace 'using Fgs\.Setup\.Application\.Features\.TenantProvisioning;', 'using Fgs.Contracts.Clients;'
        $c = $c -replace 'TenantStatusIds', 'TenantStatusIds' # noop
        if ($to -eq 'Fgs.Setup') {
            $c = $c -replace 'using Fgs\.Setup\.Application\.Features\.TenantProvisioning;\r?\n', ''
            $c = $c -replace '\bTenantStatusIds\b', 'Fgs.Contracts.Clients.TenantStatusIds'
            $c = $c -replace 'Fgs\.Contracts\.Clients\.Fgs\.Contracts\.Clients\.TenantStatusIds', 'Fgs.Contracts.Clients.TenantStatusIds'
        }
        Set-Content -LiteralPath $target -Value $c -Encoding UTF8 -NoNewline
    }
}

$setupDirs = @(
    'Features\Credentials',
    'Features\TenantProvisioning',
    'Abstractions\Provisioning'
)
foreach ($d in $setupDirs) {
    Copy-Tree (Join-Path $root "UserService\Fgs.User.Application\$d") (Join-Path $root "SetupService\Fgs.Setup.Application\$d") 'Fgs.User' 'Fgs.Setup'
}

Copy-Tree (Join-Path $root 'UserService\Fgs.User.Application\Abstractions\Storage') (Join-Path $root 'FileService\Fgs.File.Application\Abstractions\Storage') 'Fgs.User' 'Fgs.File'

# Outbox + messaging to Setup
Copy-Tree (Join-Path $root 'UserService\Fgs.User.Infrastructure\Outbox') (Join-Path $root 'SetupService\Fgs.Setup.Infrastructure\Outbox') 'Fgs.User' 'Fgs.Setup'
Copy-Tree (Join-Path $root 'UserService\Fgs.User.Infrastructure\Messaging\OutboxWriter.cs') (Join-Path $root 'SetupService\Fgs.Setup.Infrastructure\Messaging') 'Fgs.User' 'Fgs.Setup'
# OutboxWriter is single file - fix
$owSrc = Join-Path $root 'UserService\Fgs.User.Infrastructure\Messaging\OutboxWriter.cs'
if (Test-Path $owSrc) {
    $dstDir = Join-Path $root 'SetupService\Fgs.Setup.Infrastructure\Messaging'
    New-Item -ItemType Directory -Path $dstDir -Force | Out-Null
    $c = (Get-Content $owSrc -Raw) -replace 'Fgs\.User', 'Fgs.Setup' -replace 'Persistence\.Database\.DbContexts', 'Database'
    Set-Content (Join-Path $dstDir 'OutboxWriter.cs') $c -Encoding UTF8 -NoNewline
}

Write-Host 'Application layers copied.'
