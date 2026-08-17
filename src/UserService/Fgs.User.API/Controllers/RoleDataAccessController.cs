using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.User.Application.Features.RoleDataAccesses.Commands.SyncFgsRoleDataAccesses;
using Fgs.User.Application.Features.RoleDataAccesses.Dtos;
using Fgs.User.Application.Features.RoleDataAccesses.Queries.ListFgsRoleDataAccessesByRoleId;
using MediatR;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Role-to-data-access assignment management.
/// GET returns all data-access assignments for a role. PUT syncs the full set (add/keep/remove).
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("roledataaccess")]
[Produces("application/json")]
public sealed class RoleDataAccessController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpGet("{fgsRoleId:long}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsRoleDataAccessDetailDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByRoleId(long fgsRoleId, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new ListFgsRoleDataAccessesByRoleIdQuery(fgsRoleId), cancellationToken));

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
