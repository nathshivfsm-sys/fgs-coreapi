using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.User.Application.Features.UserRoles.Commands.SyncFgsUserRoles;
using Fgs.User.Application.Features.UserRoles.Dtos;
using Fgs.User.Application.Features.UserRoles.Queries.ListFgsUserRolesByUserId;
using MediatR;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// User-to-role assignment management.
/// GET returns all roles for a user. PUT syncs the full set (add/keep/remove).
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("userrole")]
[Produces("application/json")]
public sealed class UserRoleController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsUserRoleDetailDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUserId(Guid userId, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new ListFgsUserRolesByUserIdQuery(userId), cancellationToken));

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
