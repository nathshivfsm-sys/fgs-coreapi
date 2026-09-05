using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Fgs.User.Application.Features.UserRoles.Commands.CreateFgsUserRole;
using Fgs.User.Application.Features.UserRoles.Commands.PatchFgsUserRole;
using Fgs.User.Application.Features.UserRoles.Commands.SyncFgsUserRoles;
using Fgs.User.Application.Features.UserRoles.Commands.UpdateFgsUserRole;
using Fgs.User.Application.Features.UserRoles.Dtos;
using Fgs.User.Application.Features.UserRoles.Queries.GetFgsUserRoleById;
using Fgs.User.Application.Features.UserRoles.Queries.ListFgsUserRolesByUserId;
using Fgs.User.Application.Features.UserRoles.Queries.LookupFgsUserRoles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// User-to-role assignment management (CRUD) plus full-set Sync.
/// GET /{userId} lists by user. Per-assignment routes use /item/{id} to avoid Guid conflicts.
/// Entity has no IsActive; PATCH updates FgsRoleId only.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("userrole")]
[Produces("application/json")]
public sealed class UserRoleController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpGet("item/{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsUserRoleDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetFgsUserRoleByIdQuery(id), cancellationToken));

    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsUserRoleDetailDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUserId(Guid userId, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new ListFgsUserRolesByUserIdQuery(userId), cancellationToken));

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsUserRoleLookupDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Lookup(
        [FromQuery] Guid userId,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(new LookupFgsUserRolesQuery(userId), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserCreate)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsUserRoleDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsUserRoleCreateDto request,
        CancellationToken cancellationToken) =>
        CreatedFromApiResponse(await Mediator.Send(new CreateFgsUserRoleCommand(request), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserEdit)]
    [HttpPut("item/{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsUserRoleDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsUserRoleUpdateDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new UpdateFgsUserRoleCommand(id, request), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserEdit)]
    [HttpPatch("item/{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsUserRoleDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsUserRolePatchDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new PatchFgsUserRoleCommand(id, request), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserEdit)]
    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsUserRoleDetailDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Sync(
        [FromBody] FgsUserRoleSyncDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new SyncFgsUserRolesCommand(request), cancellationToken));
}
