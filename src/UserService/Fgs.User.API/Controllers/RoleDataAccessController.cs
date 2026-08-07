using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.RoleDataAccesses.Commands.CreateFgsRoleDataAccess;
using Fgs.User.Application.Features.RoleDataAccesses.Commands.DeleteFgsRoleDataAccess;
using Fgs.User.Application.Features.RoleDataAccesses.Dtos;
using Fgs.User.Application.Features.RoleDataAccesses.Queries.GetFgsRoleDataAccessById;
using Fgs.User.Application.Features.RoleDataAccesses.Queries.ListFgsRoleDataAccesses;
using MediatR;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Role-to-data-access assignment management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("roledataaccess")]
[Produces("application/json")]
public sealed class RoleDataAccessController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsRoleDataAccessDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetFgsRoleDataAccessByIdQuery(id), cancellationToken));

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsRoleDataAccessSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] long? fgsRoleId = null,
        [FromQuery] long? fgsDataAccessId = null,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(
            new ListFgsRoleDataAccessesQuery(
                new IdentityListQuery(page, pageSize, sortBy, sortDirection),
                new FgsRoleDataAccessListFilters(fgsRoleId, fgsDataAccessId)),
            cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserCreate)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsRoleDataAccessDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsRoleDataAccessCreateDto request,
        CancellationToken cancellationToken) =>
        CreatedFromApiResponse(await Mediator.Send(new CreateFgsRoleDataAccessCommand(request), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserDelete)]
    [HttpDelete("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new DeleteFgsRoleDataAccessCommand(id), cancellationToken));
}
