using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.UserRoles.Commands.CreateFgsUserRole;
using Fgs.User.Application.Features.UserRoles.Commands.DeleteFgsUserRole;
using Fgs.User.Application.Features.UserRoles.Dtos;
using Fgs.User.Application.Features.UserRoles.Queries.GetFgsUserRoleById;
using Fgs.User.Application.Features.UserRoles.Queries.ListFgsUserRoles;
using MediatR;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// User-to-role assignment management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("userrole")]
[Produces("application/json")]
public sealed class UserRoleController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsUserRoleDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetFgsUserRoleByIdQuery(id), cancellationToken));

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsUserRoleSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] Guid? userId = null,
        [FromQuery] long? fgsRoleId = null,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(
            new ListFgsUserRolesQuery(
                new IdentityListQuery(page, pageSize, sortBy, sortDirection),
                new FgsUserRoleListFilters(userId, fgsRoleId)),
            cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserCreate)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsUserRoleDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsUserRoleCreateDto request,
        CancellationToken cancellationToken) =>
        CreatedFromApiResponse(await Mediator.Send(new CreateFgsUserRoleCommand(request), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserDelete)]
    [HttpDelete("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new DeleteFgsUserRoleCommand(id), cancellationToken));
}
