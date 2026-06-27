param(
    [Parameter(Mandatory = $true)]
    [string]$ServiceName,
    [Parameter(Mandatory = $true)]
    [string]$ServicePrefix,
    [Parameter(Mandatory = $true)]
    [string]$DisplayName
)

$root = Join-Path $PSScriptRoot "..\src\${ServiceName}Service"
$appDir = Join-Path $root "Fgs.$ServicePrefix.Application"
$apiDir = Join-Path $root "Fgs.$ServicePrefix.API"

$di = @"
using System.Reflection;
using Fgs.Foundation.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.$ServicePrefix.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFgs${ServicePrefix}Application(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddFgsFoundation();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);
        return services;
    }
}
"@

$query = @"
using Fgs.Contracts.Api;
using Fgs.Contracts.Health;
using MediatR;

namespace Fgs.$ServicePrefix.Application.Features.Health.Queries.GetServiceHealth;

public sealed record GetServiceHealthQuery : IRequest<ApiResponse<ServiceHealthDto>>;
"@

$handler = @"
using Fgs.Contracts.Api;
using Fgs.Contracts.Health;
using Fgs.Foundation.Api;
using MediatR;

namespace Fgs.$ServicePrefix.Application.Features.Health.Queries.GetServiceHealth;

public sealed class GetServiceHealthQueryHandler : IRequestHandler<GetServiceHealthQuery, ApiResponse<ServiceHealthDto>>
{
    public Task<ApiResponse<ServiceHealthDto>> Handle(GetServiceHealthQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(ApiResponse<ServiceHealthDto>.Ok(
            new ServiceHealthDto("$DisplayName", "healthy", FgsApiVersions.V1)));
}
"@

$healthController = @"
using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Contracts.Health;
using Fgs.$ServicePrefix.Application.Features.Health.Queries.GetServiceHealth;
using Fgs.Foundation.Api;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.$ServicePrefix.API.Controllers;

[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("[controller]")]
public sealed class HealthController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<ServiceHealthDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetServiceHealthQuery(), cancellationToken));
}
"@

New-Item -ItemType Directory -Force -Path (Join-Path $appDir "Features\Health\Queries\GetServiceHealth") | Out-Null
Set-Content -Path (Join-Path $appDir "DependencyInjection.cs") -Value $di
Set-Content -Path (Join-Path $appDir "Features\Health\Queries\GetServiceHealth\GetServiceHealthQuery.cs") -Value $query
Set-Content -Path (Join-Path $appDir "Features\Health\Queries\GetServiceHealth\GetServiceHealthQueryHandler.cs") -Value $handler
Set-Content -Path (Join-Path $apiDir "Controllers\HealthController.cs") -Value $healthController -Force

Write-Host "Scaffolded $ServicePrefix"
