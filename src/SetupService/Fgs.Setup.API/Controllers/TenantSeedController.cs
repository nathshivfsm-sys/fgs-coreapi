using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Idempotency;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Fgs.Setup.Application.Features.TenantProvisioning.Commands.SyncTenantCatalog;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

/// <summary>
/// Seeds missing catalog rows for the current tenant and company.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("tenantseed")]
public sealed class TenantSeedController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [RequirePermission(FgsPermissionCodes.SetupEdit)]
    [Idempotent]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<TenantCatalogSyncResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<TenantCatalogSyncResultDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<TenantCatalogSyncResultDto>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Sync(CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new SyncTenantCatalogCommand(), cancellationToken));
}
