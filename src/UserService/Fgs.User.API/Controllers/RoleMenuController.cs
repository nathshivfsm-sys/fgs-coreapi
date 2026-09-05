using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Fgs.User.Application.Features.RoleMenus.Commands.CreateFgsRoleMenu;
using Fgs.User.Application.Features.RoleMenus.Commands.PatchFgsRoleMenu;
using Fgs.User.Application.Features.RoleMenus.Commands.SyncFgsRoleMenus;
using Fgs.User.Application.Features.RoleMenus.Commands.UpdateFgsRoleMenu;
using Fgs.User.Application.Features.RoleMenus.Dtos;
using Fgs.User.Application.Features.RoleMenus.Queries.GetFgsRoleMenuById;
using Fgs.User.Application.Features.RoleMenus.Queries.ListFgsRoleMenusByRoleId;
using Fgs.User.Application.Features.RoleMenus.Queries.LookupFgsRoleMenus;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Role-to-menu assignment management (CRUD) plus full-set Sync.
/// GET /{roleId} lists by role. Item routes use /item/{id} to avoid colliding with list-by-role.
/// Soft-deactivate via PATCH IsActive. PUT without id replaces the full set for a role.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("rolemenu")]
[Produces("application/json")]
public sealed class RoleMenuController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpGet("item/{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsRoleMenuDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetFgsRoleMenuByIdQuery(id), cancellationToken));

    [HttpGet("{roleId:long}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsRoleMenuDetailDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByRoleId(long roleId, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new ListFgsRoleMenusByRoleIdQuery(roleId), cancellationToken));

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsRoleMenuLookupDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Lookup(
        [FromQuery] long roleId,
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(new LookupFgsRoleMenusQuery(roleId, activeOnly), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserCreate)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsRoleMenuDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsRoleMenuCreateDto request,
        CancellationToken cancellationToken) =>
        CreatedFromApiResponse(await Mediator.Send(new CreateFgsRoleMenuCommand(request), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserEdit)]
    [HttpPut("item/{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsRoleMenuDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsRoleMenuUpdateDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new UpdateFgsRoleMenuCommand(id, request), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserEdit)]
    [HttpPatch("item/{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsRoleMenuDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsRoleMenuPatchDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new PatchFgsRoleMenuCommand(id, request), cancellationToken));

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
