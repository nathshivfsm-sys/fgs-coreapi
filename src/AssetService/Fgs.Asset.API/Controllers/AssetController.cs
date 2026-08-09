using Asp.Versioning;
using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.Assets.Commands.CreateFgsAsset;
using Fgs.Asset.Application.Features.Assets.Commands.PatchFgsAsset;
using Fgs.Asset.Application.Features.Assets.Commands.UpdateFgsAsset;
using Fgs.Asset.Application.Features.Assets.Dtos;
using Fgs.Asset.Application.Features.Assets.Queries.GetFgsAssetById;
using Fgs.Asset.Application.Features.Assets.Queries.ListAssets;
using Fgs.Asset.Application.Features.Assets.Queries.LookupAssets;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Idempotency;
using Fgs.Foundation.Paging;
using MediatR;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Asset.API.Controllers;

[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("asset")]
[Produces("application/json")]
public sealed class AssetController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsAssetDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsAssetByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsAssetSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = true,
        [FromQuery] string? assetNumber = null,
        [FromQuery] long? serviceLocationId = null,
        [FromQuery] long? assetStatusId = null,

        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListAssetsQuery(
                new AssetListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new FgsAssetListFilters(assetNumber, serviceLocationId, assetStatusId)),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsAssetLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new LookupAssetsQuery(activeOnly), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.AssetCreate)]
    [Idempotent]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsAssetDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsAssetCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateFgsAssetCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.AssetEdit)]
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsAssetDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsAssetUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateFgsAssetCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.AssetEdit)]
    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsAssetDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsAssetPatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchFgsAssetCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
