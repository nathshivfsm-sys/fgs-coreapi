$services = @('Billing','Crm','Scheduling','Inventory','Integration','Audit','Reporting','Communication','Publisher','Consumer','ServiceAgreement','Asset')
$packages = @"
  <ItemGroup>
    <PackageReference Include="FluentValidation" Version="12.0.0" />
    <PackageReference Include="FluentValidation.DependencyInjectionExtensions" Version="12.0.0" />
    <PackageReference Include="MediatR" Version="13.0.0" />
    <PackageReference Include="Microsoft.Extensions.DependencyInjection.Abstractions" Version="10.0.8" />
  </ItemGroup>
"@
foreach ($prefix in $services) {
    $csproj = Join-Path $PSScriptRoot "..\src\${prefix}Service\Fgs.$prefix.Application\Fgs.$prefix.Application.csproj"
    if (-not (Test-Path $csproj)) { continue }
    $content = @"
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\Fgs.$prefix.Domain\Fgs.$prefix.Domain.csproj" />
    <ProjectReference Include="..\..\Shared\Kernel\Fgs.Kernel\Fgs.Kernel.csproj" />
    <ProjectReference Include="..\..\Shared\Foundation\Fgs.Foundation\Fgs.Foundation.csproj" />
    <ProjectReference Include="..\..\Shared\Contracts\Fgs.Contracts\Fgs.Contracts.csproj" />
    <ProjectReference Include="..\..\Shared\Persistence\Fgs.Persistence\Fgs.Persistence.csproj" />
  </ItemGroup>
$packages
</Project>
"@
    Set-Content $csproj $content -NoNewline
    Write-Host "Fixed $prefix"
}
