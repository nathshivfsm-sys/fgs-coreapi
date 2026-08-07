using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventoryItems.Commands.CreateFgsInventoryItem;
using Fgs.Inventory.Application.Features.InventoryItems.Commands.PatchFgsInventoryItem;
using Fgs.Inventory.Application.Features.InventoryItems.Commands.UpdateFgsInventoryItem;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using Fgs.Inventory.Application.Features.InventoryItems.Queries.GetFgsInventoryItemById;
using Fgs.Inventory.Application.Features.InventoryItems.Queries.ListInventoryItems;
using Fgs.Inventory.Application.Features.InventoryItems.Queries.LookupInventoryItems;
using MediatR;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Inventory.API.Controllers;

/// <summary>
/// Tenant-scoped inventory item catalog management (header, alternates, and dependencies in one request).
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("inventoryitem")]
[ApiController]
[Produces("application/json")]
public sealed class InventoryItemController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsInventoryItemDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsInventoryItemByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsInventoryItemSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = true,
        [FromQuery] string? itemCode = null,
        [FromQuery] string? name = null,
        [FromQuery] long? inventoryItemTypeId = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListInventoryItemsQuery(
                new InventoryListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new FgsInventoryItemListFilters(itemCode, name, inventoryItemTypeId)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsInventoryItemLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new LookupInventoryItemsQuery(activeOnly), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.InventoryItemCreate)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsInventoryItemDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsInventoryItemCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateFgsInventoryItemCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.InventoryItemEdit)]
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsInventoryItemDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsInventoryItemUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateFgsInventoryItemCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.InventoryItemEdit)]
    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsInventoryItemDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsInventoryItemPatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchFgsInventoryItemCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
