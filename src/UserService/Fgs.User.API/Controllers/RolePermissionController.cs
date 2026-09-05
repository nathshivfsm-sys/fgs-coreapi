using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Fgs.User.Application.Features.RolePermissions.Commands.CreateFgsRolePermission;
using Fgs.User.Application.Features.RolePermissions.Commands.PatchFgsRolePermission;
using Fgs.User.Application.Features.RolePermissions.Commands.SyncFgsRolePermissions;
using Fgs.User.Application.Features.RolePermissions.Commands.UpdateFgsRolePermission;
using Fgs.User.Application.Features.RolePermissions.Dtos;
using Fgs.User.Application.Features.RolePermissions.Queries.GetFgsRolePermissionById;
using Fgs.User.Application.Features.RolePermissions.Queries.ListFgsRolePermissionsByRoleId;
using Fgs.User.Application.Features.RolePermissions.Queries.LookupFgsRolePermissions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Role-to-permission assignment management (CRUD) plus full-set Sync.
/// GET /{fgsRoleId} lists by role. Per-assignment routes use /item/{id} to avoid conflicts.
/// Entity has no IsActive; PATCH updates FgsPermissionId only.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("rolepermission")]
[Produces("application/json")]
public sealed class RolePermissionController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpGet("item/{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsRolePermissionDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetFgsRolePermissionByIdQuery(id), cancellationToken));

    [HttpGet("{fgsRoleId:long}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsRolePermissionDetailDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByRoleId(long fgsRoleId, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new ListFgsRolePermissionsByRoleIdQuery(fgsRoleId), cancellationToken));

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsRolePermissionLookupDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Lookup(
        [FromQuery] long fgsRoleId,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(new LookupFgsRolePermissionsQuery(fgsRoleId), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserCreate)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsRolePermissionDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsRolePermissionCreateDto request,
        CancellationToken cancellationToken) =>
        CreatedFromApiResponse(await Mediator.Send(new CreateFgsRolePermissionCommand(request), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserEdit)]
    [HttpPut("item/{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsRolePermissionDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsRolePermissionUpdateDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new UpdateFgsRolePermissionCommand(id, request), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserEdit)]
    [HttpPatch("item/{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsRolePermissionDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsRolePermissionPatchDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new PatchFgsRolePermissionCommand(id, request), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserEdit)]
    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsRolePermissionDetailDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Sync(
        [FromBody] FgsRolePermissionSyncDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new SyncFgsRolePermissionsCommand(request), cancellationToken));
}
