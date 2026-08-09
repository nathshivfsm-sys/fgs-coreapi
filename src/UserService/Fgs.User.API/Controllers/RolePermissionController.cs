using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.RolePermissions.Commands.CreateFgsRolePermission;
using Fgs.User.Application.Features.RolePermissions.Commands.DeleteFgsRolePermission;
using Fgs.User.Application.Features.RolePermissions.Dtos;
using Fgs.User.Application.Features.RolePermissions.Queries.GetFgsRolePermissionById;
using Fgs.User.Application.Features.RolePermissions.Queries.ListFgsRolePermissions;
using MediatR;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Role-to-permission assignment management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("rolepermission")]
[Produces("application/json")]
public sealed class RolePermissionController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsRolePermissionDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetFgsRolePermissionByIdQuery(id), cancellationToken));

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsRolePermissionSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] long? fgsRoleId = null,
        [FromQuery] long? fgsPermissionId = null,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(
            new ListFgsRolePermissionsQuery(
                new IdentityListQuery(page, pageSize, sortBy, sortDirection),
                new FgsRolePermissionListFilters(fgsRoleId, fgsPermissionId)),
            cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserCreate)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsRolePermissionDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsRolePermissionCreateDto request,
        CancellationToken cancellationToken) =>
        CreatedFromApiResponse(await Mediator.Send(new CreateFgsRolePermissionCommand(request), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserDelete)]
    [HttpDelete("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new DeleteFgsRolePermissionCommand(id), cancellationToken));
}
