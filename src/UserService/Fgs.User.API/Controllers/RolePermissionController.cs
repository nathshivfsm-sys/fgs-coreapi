using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.User.Application.Features.RolePermissions.Commands.SyncFgsRolePermissions;
using Fgs.User.Application.Features.RolePermissions.Dtos;
using Fgs.User.Application.Features.RolePermissions.Queries.ListFgsRolePermissionsByRoleId;
using MediatR;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Role-to-permission assignment management.
/// GET returns all permissions for a role. PUT syncs the full set (add/keep/remove).
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("rolepermission")]
[Produces("application/json")]
public sealed class RolePermissionController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpGet("{fgsRoleId:long}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsRolePermissionDetailDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByRoleId(long fgsRoleId, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new ListFgsRolePermissionsByRoleIdQuery(fgsRoleId), cancellationToken));

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
