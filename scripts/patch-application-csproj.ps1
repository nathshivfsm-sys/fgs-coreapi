$packages = @"
  <ItemGroup>
    <PackageReference Include="FluentValidation" Version="12.0.0" />
    <PackageReference Include="FluentValidation.DependencyInjectionExtensions" Version="12.0.0" />
    <PackageReference Include="MediatR" Version="13.0.0" />
    <PackageReference Include="Microsoft.Extensions.DependencyInjection.Abstractions" Version="10.0.8" />
  </ItemGroup>
"@
$services = @('Billing','Crm','Dispatch','Inventory','Job','Integration','Audit','Reporting','Communication','Publisher','Consumer','Contract')
foreach ($prefix in $services) {
    $csproj = Join-Path $PSScriptRoot "..\src\${prefix}Service\Fgs.$prefix.Application\Fgs.$prefix.Application.csproj"
    if (-not (Test-Path $csproj)) { continue }
    $content = Get-Content $csproj -Raw
    if ($content -match 'MediatR') { continue }
    $content = $content -replace '(</PropertyGroup>)', "$packages`r`n`r`n`$1"
    Set-Content $csproj $content -NoNewline
    Write-Host "Patched $prefix Application csproj"
}
