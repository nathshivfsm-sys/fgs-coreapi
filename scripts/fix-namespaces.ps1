$ErrorActionPreference = 'Stop'
Get-ChildItem 'c:\SourceCode\FGS\src\SetupService' -Recurse -Filter '*.cs' | ForEach-Object {
    $c = Get-Content -LiteralPath $_.FullName -Raw
    $n = $c `
        -replace 'Fgs\.Setup\.Infrastructure\.Persistence\.Database', 'Fgs.Setup.Infrastructure.Database' `
        -replace 'FgsUserDbContextConfigurationExtensions', 'FgsSetupDbContextConfigurationExtensions' `
        -replace 'FgsUserDbContext', 'FgsSetupDbContext'
    if ($n -ne $c) { Set-Content -LiteralPath $_.FullName -Value $n -Encoding UTF8 -NoNewline }
}
Get-ChildItem 'c:\SourceCode\FGS\src\FileService' -Recurse -Filter '*.cs' | ForEach-Object {
    $c = Get-Content -LiteralPath $_.FullName -Raw
    $n = $c -replace 'Fgs\.File\.Infrastructure\.Persistence\.Database', 'Fgs.File.Infrastructure.Database'
    if ($n -ne $c) { Set-Content -LiteralPath $_.FullName -Value $n -Encoding UTF8 -NoNewline }
}
Write-Host 'Done'
