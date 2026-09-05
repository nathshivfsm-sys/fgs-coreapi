using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.PriceBookItems.Commands.CreateFgsPriceBookItem;
using Fgs.Setup.Application.Features.PriceBookItems.Commands.DeleteFgsPriceBookItem;
using Fgs.Setup.Application.Features.PriceBookItems.Commands.PatchFgsPriceBookItem;
using Fgs.Setup.Application.Features.PriceBookItems.Commands.UpdateFgsPriceBookItem;
using Fgs.Setup.Application.Features.PriceBookItems.Dtos;
using Fgs.Setup.Application.Features.PriceBookItems.Queries.GetFgsPriceBookItemById;
using Fgs.Setup.Application.Features.PriceBookItems.Queries.ListPriceBookItems;
using Fgs.Setup.Application.Features.PriceBookItems.Queries.LookupPriceBookItems;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

/// <summary>
/// Tenant-scoped price book line-item management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("pricebookitem")]
[Produces("application/json")]
public sealed class PriceBookItemController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsPriceBookItemDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsPriceBookItemByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsPriceBookItemSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] long? priceBookId = null,
        [FromQuery] string? itemCode = null,
        [FromQuery] string? itemDescription = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListPriceBookItemsQuery(
                new SetupListQuery(page, pageSize, sortBy, sortDirection, search, IsActive: null),
                new FgsPriceBookItemListFilters(priceBookId, itemCode, itemDescription)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsPriceBookItemLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] long? priceBookId = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new LookupPriceBookItemsQuery(priceBookId), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.SetupCreate)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsPriceBookItemDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsPriceBookItemCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateFgsPriceBookItemCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.SetupEdit)]
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsPriceBookItemDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsPriceBookItemUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateFgsPriceBookItemCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.SetupEdit)]
    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsPriceBookItemDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsPriceBookItemPatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchFgsPriceBookItemCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.SetupEdit)]
    [HttpDelete("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsPriceBookItemDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteFgsPriceBookItemCommand(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
