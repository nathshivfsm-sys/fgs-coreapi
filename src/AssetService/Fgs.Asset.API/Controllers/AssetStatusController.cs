using Asp.Versioning;
using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetStatuses.Commands.CreateFgsAssetStatus;
using Fgs.Asset.Application.Features.AssetStatuses.Commands.PatchFgsAssetStatus;
using Fgs.Asset.Application.Features.AssetStatuses.Commands.UpdateFgsAssetStatus;
using Fgs.Asset.Application.Features.AssetStatuses.Dtos;
using Fgs.Asset.Application.Features.AssetStatuses.Queries.GetFgsAssetStatusById;
using Fgs.Asset.Application.Features.AssetStatuses.Queries.ListAssetStatuses;
using Fgs.Asset.Application.Features.AssetStatuses.Queries.LookupAssetStatuses;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Asset.API.Controllers;

[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("assetstatus")]
[Produces("application/json")]
public sealed class AssetStatusController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsAssetStatusDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsAssetStatusByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsAssetStatusSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = true,
        [FromQuery] string? code = null,
        [FromQuery] string? name = null,

        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListAssetStatusesQuery(
                new AssetListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new FgsAssetStatusListFilters(code, name)),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsAssetStatusLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new LookupAssetStatusesQuery(activeOnly), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsAssetStatusDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsAssetStatusCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateFgsAssetStatusCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsAssetStatusDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsAssetStatusUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateFgsAssetStatusCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsAssetStatusDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsAssetStatusPatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchFgsAssetStatusCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
