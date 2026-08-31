using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Fgs.User.Application.Features.TenantMenus.Commands.SyncFgsTenantMenus;
using Fgs.User.Application.Features.TenantMenus.Dtos;
using Fgs.User.Application.Features.TenantMenus.Queries.ListFgsTenantMenus;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Tenant company menu assignment management.
/// GET returns all menus for the current tenant company. PUT syncs the full set (add/keep/update/remove).
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("tenantmenu")]
[Produces("application/json")]
public sealed class TenantMenuController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsTenantMenuDetailDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new ListFgsTenantMenusQuery(), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserEdit)]
    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsTenantMenuDetailDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Sync(
        [FromBody] FgsTenantMenuSyncDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new SyncFgsTenantMenusCommand(request), cancellationToken));
}
