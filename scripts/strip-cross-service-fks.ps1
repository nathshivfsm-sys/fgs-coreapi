$ErrorActionPreference = 'Stop'
$cfgDir = 'c:\SourceCode\FGS\src\SetupService\Fgs.Setup.Infrastructure\Database\Configurations'
Get-ChildItem $cfgDir -Filter '*.cs' | ForEach-Object {
    $c = Get-Content -LiteralPath $_.FullName -Raw
    $orig = $c
    $c = $c -replace '(?m)^\s*entity\.ConfigureTenantCompanySetupFk\([^)]+\);\r?\n', ''
    $c = $c -replace '(?m)^\s*entity\.ConfigureTenantCompanyGuidSetupFk\([^)]+\);\r?\n', ''
    $c = $c -replace '(?ms)\s*entity\.HasOne<FgsTenantCompany>\(\)[\s\S]*?;\r?\n', ''
    $c = $c -replace '(?ms)\s*entity\.HasOne<FgsFile>\(\)[\s\S]*?;\r?\n', ''
    $c = $c -replace '(?ms)\s*entity\.HasOne<FgsLocation>\(\)[\s\S]*?;\r?\n', ''
    if ($c -ne $orig) {
        Set-Content -LiteralPath $_.FullName -Value $c -Encoding UTF8 -NoNewline
    }
}

$userCfg = 'c:\SourceCode\FGS\src\UserService\Fgs.User.Infrastructure\Persistence\Database\Configurations'
if (Test-Path $userCfg) {
    Get-ChildItem $userCfg -Filter '*.cs' | ForEach-Object {
        $c = Get-Content -LiteralPath $_.FullName -Raw
        $orig = $c
        $c = $c -replace '(?ms)\s*entity\.HasOne<FgsTenantCompany>\(\)[\s\S]*?;\r?\n', ''
        $c = $c -replace '(?ms)\s*entity\.HasOne<FgsTenant>\(\)[\s\S]*?;\r?\n', ''
        $c = $c -replace '(?ms)\s*entity\.HasOne<GloSetupTenantStatus>\(\)[\s\S]*?;\r?\n', ''
        $c = $c -replace '(?ms)\s*entity\.HasOne<GloMasterEntityType>\(\)[\s\S]*?;\r?\n', ''
        $c = $c -replace '(?ms)\s*entity\.HasOne<GloRole>\(\)[\s\S]*?;\r?\n', ''
        $c = $c -replace '(?ms)\s*entity\.HasOne<FgsUser>\(\)[\s\S]*?;\r?\n', ''
        if ($c -ne $orig) {
            Set-Content -LiteralPath $_.FullName -Value $c -Encoding UTF8 -NoNewline
        }
    }
}
Write-Host 'Done'
