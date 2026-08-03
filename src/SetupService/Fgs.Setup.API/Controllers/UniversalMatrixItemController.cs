using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Commands.CreateFgsUniversalMatrixItem;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Commands.PatchFgsUniversalMatrixItem;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Commands.UpdateFgsUniversalMatrixItem;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Dtos;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Queries.GetFgsUniversalMatrixItemById;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Queries.ListUniversalMatrixItems;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Queries.LookupUniversalMatrixItems;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

/// <summary>
/// Tenant-scoped universal matrix item management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("universalmatrixitem")]
[Produces("application/json")]
public sealed class UniversalMatrixItemController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsUniversalMatrixItemDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsUniversalMatrixItemByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsUniversalMatrixItemSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] long? universalPricingServiceId = null,
        [FromQuery] string? itemName = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListUniversalMatrixItemsQuery(
                new SetupListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new FgsUniversalMatrixItemListFilters(universalPricingServiceId, itemName)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsUniversalMatrixItemLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        [FromQuery] long? universalPricingServiceId = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new LookupUniversalMatrixItemsQuery(activeOnly, universalPricingServiceId),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsUniversalMatrixItemDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsUniversalMatrixItemCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateFgsUniversalMatrixItemCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsUniversalMatrixItemDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsUniversalMatrixItemUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateFgsUniversalMatrixItemCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsUniversalMatrixItemDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsUniversalMatrixItemPatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchFgsUniversalMatrixItemCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
