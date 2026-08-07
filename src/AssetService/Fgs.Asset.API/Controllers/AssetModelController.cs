using Asp.Versioning;
using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetModels.Commands.CreateFgsAssetModel;
using Fgs.Asset.Application.Features.AssetModels.Commands.PatchFgsAssetModel;
using Fgs.Asset.Application.Features.AssetModels.Commands.UpdateFgsAssetModel;
using Fgs.Asset.Application.Features.AssetModels.Dtos;
using Fgs.Asset.Application.Features.AssetModels.Queries.GetFgsAssetModelById;
using Fgs.Asset.Application.Features.AssetModels.Queries.ListAssetModels;
using Fgs.Asset.Application.Features.AssetModels.Queries.LookupAssetModels;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using MediatR;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Asset.API.Controllers;

[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("assetmodel")]
[Produces("application/json")]
public sealed class AssetModelController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsAssetModelDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsAssetModelByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsAssetModelSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = true,
        [FromQuery] string? modelNumber = null,
        [FromQuery] long? assetTypeId = null,
        [FromQuery] long? assetManufacturerId = null,

        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListAssetModelsQuery(
                new AssetListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new FgsAssetModelListFilters(modelNumber, assetTypeId, assetManufacturerId)),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsAssetModelLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new LookupAssetModelsQuery(activeOnly), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.AssetCreate)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsAssetModelDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsAssetModelCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateFgsAssetModelCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.AssetEdit)]
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsAssetModelDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsAssetModelUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateFgsAssetModelCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.AssetEdit)]
    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsAssetModelDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsAssetModelPatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchFgsAssetModelCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
