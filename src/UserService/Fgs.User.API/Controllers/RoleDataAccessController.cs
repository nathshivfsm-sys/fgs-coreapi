using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Fgs.User.Application.Features.RoleDataAccesses.Commands.CreateFgsRoleDataAccess;
using Fgs.User.Application.Features.RoleDataAccesses.Commands.PatchFgsRoleDataAccess;
using Fgs.User.Application.Features.RoleDataAccesses.Commands.SyncFgsRoleDataAccesses;
using Fgs.User.Application.Features.RoleDataAccesses.Commands.UpdateFgsRoleDataAccess;
using Fgs.User.Application.Features.RoleDataAccesses.Dtos;
using Fgs.User.Application.Features.RoleDataAccesses.Queries.GetFgsRoleDataAccessById;
using Fgs.User.Application.Features.RoleDataAccesses.Queries.ListFgsRoleDataAccessesByRoleId;
using Fgs.User.Application.Features.RoleDataAccesses.Queries.LookupFgsRoleDataAccesses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Role-to-data-access assignment management (CRUD) plus full-set Sync.
/// GET /{fgsRoleId} lists by role. Per-assignment routes use /item/{id} to avoid conflicts.
/// Entity has no IsActive; PATCH updates FgsDataAccessId only.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("roledataaccess")]
[Produces("application/json")]
public sealed class RoleDataAccessController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpGet("item/{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsRoleDataAccessDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetFgsRoleDataAccessByIdQuery(id), cancellationToken));

    [HttpGet("{fgsRoleId:long}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsRoleDataAccessDetailDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByRoleId(long fgsRoleId, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new ListFgsRoleDataAccessesByRoleIdQuery(fgsRoleId), cancellationToken));

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsRoleDataAccessLookupDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Lookup(
        [FromQuery] long fgsRoleId,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(new LookupFgsRoleDataAccessesQuery(fgsRoleId), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserCreate)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsRoleDataAccessDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsRoleDataAccessCreateDto request,
        CancellationToken cancellationToken) =>
        CreatedFromApiResponse(await Mediator.Send(new CreateFgsRoleDataAccessCommand(request), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserEdit)]
    [HttpPut("item/{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsRoleDataAccessDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsRoleDataAccessUpdateDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new UpdateFgsRoleDataAccessCommand(id, request), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserEdit)]
    [HttpPatch("item/{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsRoleDataAccessDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsRoleDataAccessPatchDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new PatchFgsRoleDataAccessCommand(id, request), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserEdit)]
    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsRoleDataAccessDetailDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Sync(
        [FromBody] FgsRoleDataAccessSyncDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new SyncFgsRoleDataAccessesCommand(request), cancellationToken));
}
