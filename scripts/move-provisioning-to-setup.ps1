$ErrorActionPreference = 'Stop'
$root = 'c:\SourceCode\FGS\src'

function Copy-ReplaceDir($src, $dst, $from, $to) {
    if (-not (Test-Path $src)) { return }
    New-Item -ItemType Directory -Path $dst -Force | Out-Null
    Get-ChildItem $src -Recurse -File | ForEach-Object {
        $rel = $_.FullName.Substring($src.Length).TrimStart('\')
        $target = Join-Path $dst $rel
        $dir = Split-Path $target -Parent
        New-Item -ItemType Directory -Path $dir -Force | Out-Null
        $c = (Get-Content -LiteralPath $_.FullName -Raw) -replace $from, $to
        Set-Content -LiteralPath $target -Value $c -Encoding UTF8 -NoNewline
    }
}

Copy-ReplaceDir `
    (Join-Path $root 'UserService\Fgs.User.Infrastructure\Provisioning') `
    (Join-Path $root 'SetupService\Fgs.Setup.Infrastructure\Provisioning') `
    'Fgs\.User' 'Fgs.Setup'

$consumer = Join-Path $root 'UserService\Fgs.User.Infrastructure\Background\TenantProvisionConsumerService.cs'
if (Test-Path $consumer) {
    $dstConsumer = Join-Path $root 'SetupService\Fgs.Setup.Infrastructure\Background\TenantProvisionConsumerService.cs'
    New-Item -ItemType Directory -Path (Split-Path $dstConsumer) -Force | Out-Null
    $c = (Get-Content $consumer -Raw) -replace 'Fgs\.User', 'Fgs.Setup'
    Set-Content $dstConsumer $c -Encoding UTF8 -NoNewline
    Remove-Item $consumer -Force
}

# Credentials + Security
Copy-ReplaceDir `
    (Join-Path $root 'UserService\Fgs.User.Infrastructure\Credentials') `
    (Join-Path $root 'SetupService\Fgs.Setup.Infrastructure\Credentials') `
    'Fgs\.User' 'Fgs.Setup'
Copy-ReplaceDir `
    (Join-Path $root 'UserService\Fgs.User.Infrastructure\Security') `
    (Join-Path $root 'SetupService\Fgs.Setup.Infrastructure\Security') `
    'Fgs\.User' 'Fgs.Setup'

# Storage to File
Copy-ReplaceDir `
    (Join-Path $root 'UserService\Fgs.User.Infrastructure\Storage') `
    (Join-Path $root 'FileService\Fgs.File.Infrastructure\Storage') `
    'Fgs\.User' 'Fgs.File'

Write-Host 'Provisioning and storage moved.'
