using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Fgs.User.Application.Features.RoleMenus.Commands.SyncFgsRoleMenus;
using Fgs.User.Application.Features.RoleMenus.Dtos;
using Fgs.User.Application.Features.RoleMenus.Queries.ListFgsRoleMenusByRoleId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Role-to-menu assignment management.
/// GET returns all menus for a role. PUT syncs the full set (add/keep/update/remove).
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("rolemenu")]
[Produces("application/json")]
public sealed class RoleMenuController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpGet("{roleId:long}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsRoleMenuDetailDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByRoleId(long roleId, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new ListFgsRoleMenusByRoleIdQuery(roleId), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserEdit)]
    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsRoleMenuDetailDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Sync(
        [FromBody] FgsRoleMenuSyncDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new SyncFgsRoleMenusCommand(request), cancellationToken));
}
