$services = @('Billing','Crm','Scheduling','Inventory','Integration','Audit','Reporting','Communication','Publisher','Consumer','ServiceAgreement','Asset')
foreach ($prefix in $services) {
    $program = Join-Path $PSScriptRoot "..\src\${prefix}Service\Fgs.$prefix.API\Program.cs"
    if (-not (Test-Path $program)) { continue }
    $content = Get-Content $program -Raw
    if ($content -match "AddFgs${prefix}Application") { continue }
    $using = "using Fgs.$prefix.Application;`r`n"
    if ($content -notmatch [regex]::Escape($using.Trim())) {
        $content = $content -replace '(using Fgs\.[^\r\n]+;\r?\n)(?=var builder)', "$using`$1"
    }
    $content = $content -replace "(builder\.Services\.AddFgs${prefix}Infrastructure)", "builder.Services.AddFgs${prefix}Application();`r`n`$1"
    Set-Content $program $content -NoNewline
    Write-Host "Patched $prefix API Program"
}
