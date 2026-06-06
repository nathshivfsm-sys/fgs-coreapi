$ErrorActionPreference = 'Stop'
$root = 'c:\SourceCode\FGS'
$commonSrc = Join-Path $root 'src\UserService\Fgs.User.Infrastructure\Common'
if (-not (Test-Path $commonSrc)) {
    git -C $root checkout HEAD -- 'src/UserService/Fgs.User.Infrastructure/Common' 2>$null
}
$setupCommon = Join-Path $root 'src\SetupService\Fgs.Setup.Infrastructure\Common'
if (Test-Path $commonSrc) {
    Get-ChildItem $commonSrc -Recurse -File | ForEach-Object {
        $rel = $_.FullName.Substring($commonSrc.Length).TrimStart('\')
        $target = Join-Path $setupCommon $rel
        New-Item -ItemType Directory -Path (Split-Path $target) -Force | Out-Null
        $c = (Get-Content $_.FullName -Raw) -replace 'Fgs\.User', 'Fgs.Setup'
        Set-Content $target $c -Encoding UTF8 -NoNewline
    }
}
# Credential extensions + repository
$files = @(
    'CredentialServiceCollectionExtensions.cs',
    'Persistence/Database/Repositories/CredentialRepository.cs'
)
foreach ($f in $files) {
    $gitPath = "src/UserService/Fgs.User.Infrastructure/$f"
    $content = git -C $root show "HEAD:$gitPath" 2>$null
    if ($content) {
        $dst = Join-Path $root "src\SetupService\Fgs.Setup.Infrastructure\$($f -replace 'Persistence/Database/','Database/' -replace '/','\')"
        $dstDir = Split-Path $dst
        New-Item -ItemType Directory -Path $dstDir -Force | Out-Null
        $content = $content -replace 'Fgs\.User', 'Fgs.Setup' -replace 'Persistence\.Database', 'Database'
        Set-Content $dst $content -Encoding UTF8
    }
}
Write-Host 'Common restored'
