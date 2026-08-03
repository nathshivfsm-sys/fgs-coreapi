using Asp.Versioning;
using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetAttributeValues.Commands.CreateFgsAssetAttributeValue;
using Fgs.Asset.Application.Features.AssetAttributeValues.Commands.PatchFgsAssetAttributeValue;
using Fgs.Asset.Application.Features.AssetAttributeValues.Commands.UpdateFgsAssetAttributeValue;
using Fgs.Asset.Application.Features.AssetAttributeValues.Dtos;
using Fgs.Asset.Application.Features.AssetAttributeValues.Queries.GetFgsAssetAttributeValueById;
using Fgs.Asset.Application.Features.AssetAttributeValues.Queries.ListAssetAttributeValues;
using Fgs.Asset.Application.Features.AssetAttributeValues.Queries.LookupAssetAttributeValues;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Asset.API.Controllers;

[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("assetattributevalue")]
[Produces("application/json")]
public sealed class AssetAttributeValueController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsAssetAttributeValueDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsAssetAttributeValueByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsAssetAttributeValueSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] long? assetId = null,
        [FromQuery] long? assetAttributeId = null,

        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListAssetAttributeValuesQuery(
                new AssetListQuery(page, pageSize, sortBy, sortDirection, search, null),
                new FgsAssetAttributeValueListFilters(assetId, assetAttributeId)),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsAssetAttributeValueLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new LookupAssetAttributeValuesQuery(), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsAssetAttributeValueDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsAssetAttributeValueCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateFgsAssetAttributeValueCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsAssetAttributeValueDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsAssetAttributeValueUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateFgsAssetAttributeValueCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsAssetAttributeValueDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsAssetAttributeValuePatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchFgsAssetAttributeValueCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
