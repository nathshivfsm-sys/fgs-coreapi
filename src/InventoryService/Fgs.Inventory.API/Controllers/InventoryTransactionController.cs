using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventoryTransactions.Commands.CreateFgsInventoryTransaction;
using Fgs.Inventory.Application.Features.InventoryTransactions.Dtos;
using Fgs.Inventory.Application.Features.InventoryTransactions.Queries.GetFgsInventoryTransactionById;
using Fgs.Inventory.Application.Features.InventoryTransactions.Queries.ListInventoryTransactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Inventory.API.Controllers;

/// <summary>
/// Immutable tenant-scoped inventory transaction audit log.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("inventorytransaction")]
[ApiController]
[Produces("application/json")]
public sealed class InventoryTransactionController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsInventoryTransactionDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsInventoryTransactionByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsInventoryTransactionSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] long? inventoryItemId = null,
        [FromQuery] string? transactionType = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListInventoryTransactionsQuery(
                new InventoryListQuery(page, pageSize, sortBy, sortDirection, search, IsActive: null),
                new FgsInventoryTransactionListFilters(inventoryItemId, transactionType)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsInventoryTransactionDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsInventoryTransactionCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateFgsInventoryTransactionCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
