using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventorySerials.Commands.CreateFgsInventorySerial;
using Fgs.Inventory.Application.Features.InventorySerials.Commands.PatchFgsInventorySerial;
using Fgs.Inventory.Application.Features.InventorySerials.Commands.UpdateFgsInventorySerial;
using Fgs.Inventory.Application.Features.InventorySerials.Dtos;
using Fgs.Inventory.Application.Features.InventorySerials.Queries.GetFgsInventorySerialById;
using Fgs.Inventory.Application.Features.InventorySerials.Queries.ListInventorySerials;
using Fgs.Inventory.Application.Features.InventorySerials.Queries.LookupInventorySerials;
using Fgs.Inventory.Domain.Enums;
using MediatR;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Inventory.API.Controllers;

/// <summary>
/// Tenant-scoped individual serialized inventory unit management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("inventoryserial")]
[ApiController]
[Produces("application/json")]
public sealed class InventorySerialController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsInventorySerialDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsInventorySerialByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsInventorySerialSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] long? inventoryItemId = null,
        [FromQuery] FgsInventorySerialStatus? inventorySerialStatus = null,
        [FromQuery] string? serialNumber = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListInventorySerialsQuery(
                new InventoryListQuery(page, pageSize, sortBy, sortDirection, search, IsActive: null),
                new FgsInventorySerialListFilters(inventoryItemId, inventorySerialStatus, serialNumber)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsInventorySerialLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] long? inventoryItemId = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new LookupInventorySerialsQuery(inventoryItemId), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.InventoryItemCreate)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsInventorySerialDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsInventorySerialCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateFgsInventorySerialCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.InventoryItemEdit)]
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsInventorySerialDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsInventorySerialUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateFgsInventorySerialCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.InventoryItemEdit)]
    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsInventorySerialDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsInventorySerialPatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchFgsInventorySerialCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
