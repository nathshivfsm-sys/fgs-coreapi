using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.Permissions.Commands.CreateFgsPermission;
using Fgs.User.Application.Features.Permissions.Commands.PatchFgsPermission;
using Fgs.User.Application.Features.Permissions.Commands.UpdateFgsPermission;
using Fgs.User.Application.Features.Permissions.Dtos;
using Fgs.User.Application.Features.Permissions.Queries.GetFgsPermissionById;
using Fgs.User.Application.Features.Permissions.Queries.ListFgsPermissions;
using Fgs.User.Application.Features.Permissions.Queries.LookupFgsPermissions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Global permission catalog management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("permission")]
[Produces("application/json")]
public sealed class PermissionController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsPermissionDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetFgsPermissionByIdQuery(id), cancellationToken));

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsPermissionSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] string? permissionCode = null,
        [FromQuery] string? module = null,
        [FromQuery] string? resource = null,
        [FromQuery] string? action = null,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(
            new ListFgsPermissionsQuery(
                new IdentityListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new FgsPermissionListFilters(permissionCode, module, resource, action)),
            cancellationToken));

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsPermissionLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(new LookupFgsPermissionsQuery(activeOnly), cancellationToken));

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsPermissionDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsPermissionCreateDto request,
        CancellationToken cancellationToken) =>
        CreatedFromApiResponse(await Mediator.Send(new CreateFgsPermissionCommand(request), cancellationToken));

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsPermissionDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsPermissionUpdateDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new UpdateFgsPermissionCommand(id, request), cancellationToken));

    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsPermissionDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsPermissionPatchDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new PatchFgsPermissionCommand(id, request), cancellationToken));
}
