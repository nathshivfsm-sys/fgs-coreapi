using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.PublicEndpoints.Commands.CreateFgsPublicEndpoint;
using Fgs.User.Application.Features.PublicEndpoints.Commands.PatchFgsPublicEndpoint;
using Fgs.User.Application.Features.PublicEndpoints.Commands.UpdateFgsPublicEndpoint;
using Fgs.User.Application.Features.PublicEndpoints.Dtos;
using Fgs.User.Application.Features.PublicEndpoints.Queries.GetFgsPublicEndpointById;
using Fgs.User.Application.Features.PublicEndpoints.Queries.ListFgsPublicEndpoints;
using Fgs.User.Application.Features.PublicEndpoints.Queries.LookupFgsPublicEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Tenant-scoped public endpoint catalog management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("publicendpoint")]
[Produces("application/json")]
public sealed class PublicEndpointController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsPublicEndpointDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetFgsPublicEndpointByIdQuery(id), cancellationToken));

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsPublicEndpointSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] string? endpointType = null,
        [FromQuery] string? environmentCode = null,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(
            new ListFgsPublicEndpointsQuery(
                new IdentityListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new FgsPublicEndpointListFilters(endpointType, environmentCode)),
            cancellationToken));

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsPublicEndpointLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(new LookupFgsPublicEndpointsQuery(activeOnly), cancellationToken));

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsPublicEndpointDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsPublicEndpointCreateDto request,
        CancellationToken cancellationToken) =>
        CreatedFromApiResponse(await Mediator.Send(new CreateFgsPublicEndpointCommand(request), cancellationToken));

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsPublicEndpointDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsPublicEndpointUpdateDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new UpdateFgsPublicEndpointCommand(id, request), cancellationToken));

    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsPublicEndpointDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsPublicEndpointPatchDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new PatchFgsPublicEndpointCommand(id, request), cancellationToken));
}
