using Asp.Versioning;
using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetWarranties.Commands.CreateFgsAssetWarranty;
using Fgs.Asset.Application.Features.AssetWarranties.Commands.PatchFgsAssetWarranty;
using Fgs.Asset.Application.Features.AssetWarranties.Commands.UpdateFgsAssetWarranty;
using Fgs.Asset.Application.Features.AssetWarranties.Dtos;
using Fgs.Asset.Application.Features.AssetWarranties.Queries.GetFgsAssetWarrantyById;
using Fgs.Asset.Application.Features.AssetWarranties.Queries.ListAssetWarranties;
using Fgs.Asset.Application.Features.AssetWarranties.Queries.LookupAssetWarranties;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using MediatR;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Asset.API.Controllers;

[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("assetwarranty")]
[Produces("application/json")]
public sealed class AssetWarrantyController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsAssetWarrantyDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsAssetWarrantyByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsAssetWarrantySummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] long? assetId = null,
        [FromQuery] string? warrantyType = null,

        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListAssetWarrantiesQuery(
                new AssetListQuery(page, pageSize, sortBy, sortDirection, search, null),
                new FgsAssetWarrantyListFilters(assetId, warrantyType)),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsAssetWarrantyLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new LookupAssetWarrantiesQuery(), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.AssetCreate)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsAssetWarrantyDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsAssetWarrantyCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateFgsAssetWarrantyCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.AssetEdit)]
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsAssetWarrantyDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsAssetWarrantyUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateFgsAssetWarrantyCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.AssetEdit)]
    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsAssetWarrantyDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsAssetWarrantyPatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchFgsAssetWarrantyCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
