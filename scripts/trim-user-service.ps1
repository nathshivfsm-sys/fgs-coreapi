$ErrorActionPreference = 'Stop'
$keepPrefixes = @('FgsUser', 'FgsRole', 'FgsInvitation', 'FgsTenant', 'FgsLocation')
$src = 'c:\SourceCode\FGS\src\UserService\Fgs.User.Infrastructure\Persistence\Database\Configurations'
$dst = 'c:\SourceCode\FGS\src\UserService\Fgs.User.Infrastructure\Database\Configurations'
New-Item -ItemType Directory -Path $dst -Force | Out-Null
Get-ChildItem $src -Filter '*.cs' | ForEach-Object {
    $match = $false
    foreach ($p in $keepPrefixes) {
        if ($_.BaseName.StartsWith($p)) { $match = $true; break }
    }
    if ($match) {
        $c = (Get-Content -LiteralPath $_.FullName -Raw) -replace 'Fgs\.User\.Infrastructure\.Persistence\.Database', 'Fgs.User.Infrastructure.Database'
        Set-Content -LiteralPath (Join-Path $dst $_.Name) -Value $c -Encoding UTF8 -NoNewline
    }
}

$entDir = 'c:\SourceCode\FGS\src\UserService\Fgs.User.Domain\Entities'
$keepFiles = @(
    'FgsUser.cs', 'FgsUserRole.cs', 'FgsRole.cs', 'FgsInvitation.cs',
    'FgsTenant.cs', 'FgsTenantCompany.cs', 'FgsTenantServiceSetup.cs', 'FgsLocation.cs',
    'VendorTypes.cs', 'WarehouseTypes.cs', 'VehicleOwnershipTypes.cs'
)
Get-ChildItem $entDir -Filter '*.cs' | Where-Object { $keepFiles -notcontains $_.Name } | Remove-Item -Force

# Remove old persistence tree (migrations, scripts, duplicate configs)
$pers = 'c:\SourceCode\FGS\src\UserService\Fgs.User.Infrastructure\Persistence'
if (Test-Path $pers) { Remove-Item -LiteralPath $pers -Recurse -Force }

Write-Host 'User service trimmed.'
