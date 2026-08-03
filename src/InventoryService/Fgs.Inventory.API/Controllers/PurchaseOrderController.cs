using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.PurchaseOrders.Commands.CreateFgsPurchaseOrder;
using Fgs.Inventory.Application.Features.PurchaseOrders.Commands.PatchFgsPurchaseOrder;
using Fgs.Inventory.Application.Features.PurchaseOrders.Commands.UpdateFgsPurchaseOrder;
using Fgs.Inventory.Application.Features.PurchaseOrders.Dtos;
using Fgs.Inventory.Application.Features.PurchaseOrders.Queries.GetFgsPurchaseOrderById;
using Fgs.Inventory.Application.Features.PurchaseOrders.Queries.ListPurchaseOrders;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Inventory.API.Controllers;

/// <summary>
/// Tenant-scoped purchase order management (header and line details in one request).
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("purchaseorder")]
[ApiController]
[Produces("application/json")]
public sealed class PurchaseOrderController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsPurchaseOrderDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsPurchaseOrderByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsPurchaseOrderSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] string? purchaseOrderNumber = null,
        [FromQuery] long? vendorId = null,
        [FromQuery] string? purchaseOrderStatus = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListPurchaseOrdersQuery(
                new InventoryListQuery(page, pageSize, sortBy, sortDirection, search, IsActive: null),
                new FgsPurchaseOrderListFilters(purchaseOrderNumber, vendorId, purchaseOrderStatus)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsPurchaseOrderDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsPurchaseOrderCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateFgsPurchaseOrderCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsPurchaseOrderDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsPurchaseOrderUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateFgsPurchaseOrderCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsPurchaseOrderDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsPurchaseOrderPatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchFgsPurchaseOrderCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
