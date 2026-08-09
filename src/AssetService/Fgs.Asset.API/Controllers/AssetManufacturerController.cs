using Asp.Versioning;
using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetManufacturers.Commands.CreateFgsAssetManufacturer;
using Fgs.Asset.Application.Features.AssetManufacturers.Commands.PatchFgsAssetManufacturer;
using Fgs.Asset.Application.Features.AssetManufacturers.Commands.UpdateFgsAssetManufacturer;
using Fgs.Asset.Application.Features.AssetManufacturers.Dtos;
using Fgs.Asset.Application.Features.AssetManufacturers.Queries.GetFgsAssetManufacturerById;
using Fgs.Asset.Application.Features.AssetManufacturers.Queries.ListAssetManufacturers;
using Fgs.Asset.Application.Features.AssetManufacturers.Queries.LookupAssetManufacturers;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using MediatR;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Asset.API.Controllers;

[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("assetmanufacturer")]
[Produces("application/json")]
public sealed class AssetManufacturerController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsAssetManufacturerDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsAssetManufacturerByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsAssetManufacturerSummaryDto>>), StatusCodes.Status200OK)]
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
            new ListAssetManufacturersQuery(
                new AssetListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new FgsAssetManufacturerListFilters(code, name)),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsAssetManufacturerLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new LookupAssetManufacturersQuery(activeOnly), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.AssetCreate)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsAssetManufacturerDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsAssetManufacturerCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateFgsAssetManufacturerCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.AssetEdit)]
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsAssetManufacturerDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsAssetManufacturerUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateFgsAssetManufacturerCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.AssetEdit)]
    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsAssetManufacturerDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsAssetManufacturerPatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchFgsAssetManufacturerCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
